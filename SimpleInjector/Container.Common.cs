﻿// Copyright (c) Simple Injector Contributors. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace SimpleInjector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using SimpleInjector.Advanced;
    using SimpleInjector.Diagnostics;
    using SimpleInjector.Diagnostics.Debugger;
    using SimpleInjector.Internals;
    using SimpleInjector.Lifestyles;
    using SimpleInjector.ProducerBuilders;

    /// <summary>
    /// The container. Create an instance of this type for registration of dependencies.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Thread-safety:</b>
    /// Resolving instances can be done safely from multiple threads concurrently, but registration needs to
    /// be done from one single thread.
    /// </para>
    /// <para>
    /// It is therefore safe to call <see cref="GetInstance"/>, <see cref="GetAllInstances"/>,
    /// <see cref="IServiceProvider.GetService">GetService</see>, <see cref="GetRegistration(System.Type)"/> and
    /// <see cref="GetCurrentRegistrations()"/> and anything related to resolving instances from multiple thread
    /// concurrently. It is however <b>unsafe</b> to call
    /// <see cref="Register{TService, TImplementation}(Lifestyle)">RegisterXXX</see>,
    /// <see cref="ExpressionBuilding"/>, <see cref="ExpressionBuilt"/>, <see cref="ResolveUnregisteredType"/>,
    /// <see cref="AddRegistration"/> or anything related to registering from multiple threads concurrently.
    /// </para>
    /// </remarks>
    [DebuggerTypeProxy(typeof(ContainerDebugView))]
    public partial class Container : ApiObject, IDisposable
    {
        internal readonly Dictionary<object, Dictionary<Type, WeakReference>> LifestyleRegistrationCache =
            new Dictionary<object, Dictionary<Type, WeakReference>>();

        private static long counter;

        private readonly object locker = new object();
        private readonly List<IInstanceInitializer> instanceInitializers = new();
        private readonly List<ContextualResolveInterceptor> resolveInterceptors = new();

        private readonly long containerId;

        // Collection of (both conditional and unconditional) instance producers that are explicitly
        // registered by the user and implicitly registered through unregistered type resolution.
        private readonly Dictionary<Type, IRegistrationEntry> explicitRegistrations = new(64);

        private readonly Dictionary<Type, CollectionResolver> collectionResolvers = new();

        // This list contains all instance producers that not yet have been explicitly registered in the container.
        private readonly ConditionalHashSet<InstanceProducer> externalProducers = new();

        // Flag to signal that the container can't be altered by using any of the Register methods.
        private bool locked;
        private string? stackTraceThatLockedTheContainer;
        private bool disposed;
        private string? stackTraceThatDisposedTheContainer;

        private EventHandler<UnregisteredTypeEventArgs>? resolveUnregisteredType;
        private EventHandler<ExpressionBuildingEventArgs>? expressionBuilding;
        private EventHandler<ExpressionBuiltEventArgs>? expressionBuilt;

        private readonly IInstanceProducerBuilder[] producerBuilders;
        private readonly IInstanceProducerBuilder unregisteredConcreteTypeProducerBuilder;

        /// <summary>Initializes a new instance of the <see cref="Container"/> class.</summary>
        public Container()
        {
            this.containerId = Interlocked.Increment(ref counter);

            this.ContainerScope = new ContainerScope(this);

            this.Collection = new ContainerCollectionRegistrator(this);
            this.Options = new ContainerOptions(this);

            this.SelectionBasedLifestyle = new LifestyleSelectionBehaviorProxyLifestyle(this.Options);

            // NOTE: The order of these builders is crucial.
            this.producerBuilders = new IInstanceProducerBuilder[]
            {
                new DependencyMetadataInstanceProducerBuilder(this),
                new UnregisteredTypeResolutionInstanceProducerBuilder(
                    container: this,
                    shouldResolveUnregisteredTypes: () => this.resolveUnregisteredType != null,
                    resolveUnregisteredType: e => this.resolveUnregisteredType?.Invoke(this, e)),
                new CollectionInstanceProducerBuilder(this)
            };

            this.unregisteredConcreteTypeProducerBuilder =
                new UnregisteredConcreteTypeInstanceProducerBuilder(this);
        }

        // Wrapper for instance initializer delegates
        private interface IInstanceInitializer
        {
            bool AppliesTo(Type implementationType, InitializerContext context);

            Action<T> CreateAction<T>(InitializerContext context);
        }

        /// <summary>Gets the container options.</summary>
        /// <value>The <see cref="ContainerOptions"/> instance for this container.</value>
        public ContainerOptions Options { get; }

        /// <summary>Gets the container scope that manages the lifetime of singletons and other
        /// container-controlled instances. Use this property to register actions that need to be called
        /// and instances that need to be disposed when the container gets disposed.</summary>
        /// <value>The <see cref="ContainerOptions"/> instance for this container.</value>
        public ContainerScope ContainerScope { get; }

        /// <summary>
        /// Gets a value indicating whether the container is currently being verified on the current thread.
        /// </summary>
        /// <value>True in case the container is currently being verified on the current thread; otherwise
        /// false.</value>
        public bool IsVerifying
        {
            get
            {
                // Need to check, because IsVerifying will throw when its ThreadLocal<T> is disposed.
                this.ThrowWhenDisposed();

                return this.isVerifying.Value;
            }

            private set
            {
                this.isVerifying.Value = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the container is locked from making any new registrations. The
        /// container is automatically locked when the first instance is resolved (e.g. by calling
        /// <see cref="Container.GetInstance">GetInstance</see>).
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the specified container is locked; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLocked
        {
            get
            {
                if (this.locked)
                {
                    return true;
                }

                // By using a lock, we have the certainty that all threads will see the new value for 'locked'
                // immediately.
                lock (this.locker)
                {
                    return this.locked;
                }
            }
        }

        /// <summary>
        /// Gets the intermediate lifestyle that forwards CreateRegistration calls to the lifestyle that is
        /// returned from the registered container.Options.LifestyleSelectionBehavior.
        /// </summary>
        internal LifestyleSelectionBehaviorProxyLifestyle SelectionBasedLifestyle { get; }

        internal bool HasRegistrations =>
            this.explicitRegistrations.Count > 0 || this.collectionResolvers.Count > 0;

        internal bool HasResolveInterceptors => this.resolveInterceptors.Count > 0;

        internal bool IsDisposed => this.disposed;

        /// <summary>
        /// Returns an array with the current registrations. This list contains all explicitly registered
        /// types, and all implicitly registered instances. Implicit registrations are  all concrete
        /// unregistered types that have been requested, all types that have been resolved using
        /// unregistered type resolution (using the <see cref="ResolveUnregisteredType"/> event), and
        /// requested unregistered collections. Note that the result of this method may change over time,
        /// because of these implicit registrations.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method has a performance characteristic of O(n). Prevent from calling this in a performance
        /// critical path of the application.
        /// </para>
        /// <para>
        /// <b>Note:</b> This method is <i>not</i> guaranteed to always return the same
        /// <see cref="InstanceProducer"/> instance for a given registration. It will however either
        /// always return a producer that is able to return the expected instance. Because of this, do not
        /// compare sets of instances returned by different calls to <see cref="GetCurrentRegistrations()"/>
        /// by reference. The way of comparing lists is by the actual type. The type of each instance is
        /// guaranteed to be unique in the returned list.
        /// </para>
        /// </remarks>
        /// <returns>An array of <see cref="InstanceProducer"/> instances.</returns>
        public InstanceProducer[] GetCurrentRegistrations() =>
            this.GetCurrentRegistrations(includeInvalidContainerRegisteredTypes: false);

        /// <summary>
        /// Returns an array with the current registrations for root objects. Root objects are registrations
        /// that are in the root of the object graph, meaning that no other registration is depending on it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method has a performance characteristic of O(n). Prevent from calling this in a performance
        /// critical path of the application.
        /// </para>
        /// <para>
        /// This list contains the root objects of all explicitly registered types, and all implicitly
        /// registered instances. Implicit registrations are all concrete unregistered types that have been
        /// requested, all types that have been resolved using unregistered type resolution (using the
        /// <see cref="ResolveUnregisteredType"/> event), and requested unregistered collections. Note that
        /// the result of this method may change over time, because of these implicit registrations.
        /// </para>
        /// <para>
        /// <b>Note:</b> This method is <i>not</i> guaranteed to always return the same
        /// <see cref="InstanceProducer"/> instance for a given registration. It will however either
        /// always return a producer that is able to return the expected instance. Because of this, do not
        /// compare sets of instances returned by different calls to <see cref="GetCurrentRegistrations()"/>
        /// by reference. The way of comparing lists is by the actual type. The type of each instance is
        /// guaranteed to be unique in the returned list.
        /// </para>
        /// </remarks>
        /// <returns>An array of <see cref="InstanceProducer"/> instances.</returns>
        /// <exception cref="InvalidOperationException">Thrown when this method is called before
        /// <see cref="Verify()"/> has been successfully called.</exception>
        public InstanceProducer[] GetRootRegistrations()
        {
            if (!this.SuccesfullyVerified)
            {
                throw new InvalidOperationException(
                    StringResources.GetRootRegistrationsCanNotBeCalledBeforeVerify());
            }

            return this.GetRootRegistrations(includeInvalidContainerRegisteredTypes: false);
        }

        /// <summary>Releases all instances that are cached by the <see cref="Container"/> object.</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal InstanceProducer[] GetRootRegistrations(bool includeInvalidContainerRegisteredTypes)
        {
            var currentRegistrations = this.GetCurrentRegistrations(
                includeInvalidContainerRegisteredTypes: includeInvalidContainerRegisteredTypes);

            // Get the non-root producers. Producers of a controlled collection are considered to be roots.
            var nonRootProducers =
                from registration in currentRegistrations
                let relationships = registration.IsContainerControlledCollection()
                    ? Enumerable.Empty<KnownRelationship>()
                    : registration.GetRelationships()
                from relationship in relationships
                select relationship.Dependency;

            return currentRegistrations.Except(nonRootProducers, InstanceProducer.EqualityComparer).ToArray();
        }

        internal InstanceProducer[] GetCurrentRegistrations(
            bool includeInvalidContainerRegisteredTypes, bool includeExternalProducers = true)
        {
            var producers =
                from entry in this.explicitRegistrations.Values
                from producer in entry.CurrentProducers
                select producer;

            producers = producers.Concat(this.rootProducerCache.Values);

            if (includeExternalProducers)
            {
                producers = producers.Concat(this.externalProducers.GetLivingItems());
            }

            // Filter out the invalid registrations (see the IsValid property for more information).
            producers =
                from producer in producers.Distinct(InstanceProducer.EqualityComparer)
                where producer != null
                where includeInvalidContainerRegisteredTypes || producer.IsValid
                select producer;

            return producers.ToArray();
        }

        internal Expression OnExpressionBuilding(
            Registration registration, Type implementationType, Expression instanceCreatorExpression)
        {
            if (this.expressionBuilding != null)
            {
                var relationships = new KnownRelationshipCollection(registration.GetRelationships().ToList());

                var e = new ExpressionBuildingEventArgs(
                    implementationType,
                    instanceCreatorExpression,
                    registration.Lifestyle,
                    knownRelationships: relationships);

                this.expressionBuilding(this, e);

                // Optimization.
                if (relationships.HasChanged)
                {
                    registration.ReplaceRelationships(e.KnownRelationships);
                }

                return e.Expression;
            }

            return instanceCreatorExpression;
        }

        internal ExpressionBuiltEventArgs? OnExpressionBuilt(
            InstanceProducer instanceProducer, Expression expression)
        {
            if (this.expressionBuilt != null)
            {
                var relationships =
                    new KnownRelationshipCollection(instanceProducer.GetRelationships().ToList());

                var e = new ExpressionBuiltEventArgs(
                    instanceProducer.ServiceType,
                    expression,
                    instanceProducer,
                    replacedRegistration: instanceProducer.Registration,
                    knownRelationships: relationships);

                this.expressionBuilt(this, e);

                if (relationships.HasChanged)
                {
                    instanceProducer.ReplaceRelationships(e.KnownRelationships);
                }

                return e;
            }
            else
            {
                return null;
            }
        }

        /// <summary>Prevents any new registrations to be made to the container.</summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void LockContainer()
        {
            if (!this.locked)
            {
                // Performance optimization: The real locking is moved to another method to allow this method
                // to be in-lined.
                this.NotifyAndLock();
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ThrowWhenDisposed()
        {
            if (this.disposed)
            {
                // Performance optimization: Throwing moved to another method to allow this method to be
                // in-lined.
                this.ThrowContainerDisposedException();
            }
        }

        internal Func<object> WrapWithResolveInterceptor(
            InstanceProducer instanceProducer, Func<object> producer)
        {
            if (this.HasResolveInterceptors)
            {
                var context = new InitializationContext(instanceProducer, instanceProducer.Registration);

                foreach (ResolveInterceptor interceptor in this.GetResolveInterceptorsFor(context))
                {
                    producer = ApplyResolveInterceptor(interceptor, context, producer);
                }
            }

            return producer;
        }

        internal void ThrowWhenContainerIsLockedOrDisposed()
        {
            this.ThrowWhenDisposed();

            // By using a lock, we have the certainty that all threads will see the new value for 'locked'
            // immediately.
            lock (this.locker)
            {
                if (this.locked)
                {
                    throw new InvalidOperationException(StringResources.ContainerCanNotBeChangedAfterUse(
                        this.stackTraceThatLockedTheContainer));
                }
            }
        }

        internal void ThrowParameterTypeMustBeRegistered(InjectionTargetInfo target)
        {
            var type = target.TargetType;

            throw new ActivationException(
                StringResources.ParameterTypeMustBeRegistered(
                    container: this,
                    target: target,
                    forceFullQualificationOfTarget: this.GetLookalikesForMissingType(target.Member.DeclaringType).Any(),
                    numberOfConditionals: this.GetNumberOfConditionalRegistrationsFor(type),
                    hasRelatedOneToOneMapping: this.ContainsOneToOneRegistrationForCollection(type),
                    collectionRegistrationDoesNotExists: this.IsCollectionButNoOneToToOneRegistrationExists(type),
                    hasRelatedCollectionMapping: this.ContainsCollectionRegistrationFor(type),
                    skippedDecorators: this.GetNonGenericDecoratorsSkippedDuringAutoRegistration(type),
                    lookalikes: this.GetLookalikesForMissingType(type)));
        }

        internal CollectionResolver GetContainerUncontrolledResolver(Type itemType) =>
            this.GetCollectionResolver(itemType, containerControlled: false);

        internal CollectionResolver GetCollectionResolver(Type itemType, bool containerControlled)
        {
            Type key = GetRegistrationKey(itemType);

            return this.collectionResolvers.GetValueOrDefault(key)
                ?? this.CreateAndAddCollectionResolver(key, containerControlled);
        }

        /// <summary>Releases all instances that are cached by the <see cref="Container"/> object.</summary>
        /// <param name="disposing">True for a normal dispose operation; false to finalize the handle.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.disposed)
                {
                    this.stackTraceThatDisposedTheContainer = GetStackTraceOrNull();

                    try
                    {
                        this.ContainerScope.Dispose();
                    }
                    finally
                    {
                        this.disposed = true;
                        this.isVerifying.Dispose();
                    }
                }
            }
        }

        [DebuggerStepThrough]
        private static string? GetStackTraceOrNull()
        {
#if !NETSTANDARD1_0 && !NETSTANDARD1_3
            return new System.Diagnostics.StackTrace(fNeedFileInfo: true, skipFrames: 2).ToString();
#else
            return null;
#endif
        }

        private static Func<object> ApplyResolveInterceptor(
            ResolveInterceptor interceptor, InitializationContext context, Func<object> wrappedProducer)
        {
            return () => ThrowWhenResolveInterceptorReturnsNull(interceptor(context, wrappedProducer));
        }

        private CollectionResolver CreateAndAddCollectionResolver(Type openServiceType, bool controlled)
        {
            var resolver = controlled
                ? (CollectionResolver)new ContainerControlledCollectionResolver(this, openServiceType)
                : (CollectionResolver)new ContainerUncontrolledCollectionResolver(this, openServiceType);

            this.collectionResolvers.Add(openServiceType, resolver);

            this.ResolveUnregisteredType += resolver.ResolveUnregisteredType;
            this.Verifying += resolver.TriggerUnregisteredTypeResolutionOnAllClosedCollections;

            return resolver;
        }

        private void NotifyAndLock()
        {
            // By using a lock, we have the certainty that all threads will see the new value for 'locked'
            // immediately, since ThrowWhenContainerIsLocked also locks on 'locker'.
            lock (this.locker)
            {
                if (!this.locked)
                {
                    this.stackTraceThatLockedTheContainer = GetStackTraceOrNull();

                    try
                    {
                        this.Options.RaiseContainerLockingAndReset();
                    }
                    finally
                    {
                        this.locked = true;
                    }
                }
            }

            if (this.Options.EnableAutoVerification && !this.IsVerifying && !this.SuccesfullyVerified)
            {
                try
                {
                    this.Verify();
                }
                catch (InvalidOperationException ex)
                {
                    // Verify throws an invalid operation exception. Here, we instead want to throw an
                    // ActivationException, as that would allow the same exception type to be thrown in that
                    // case, which is often what the user would expect.
                    throw new ActivationException(
                        StringResources.EnableAutoVerificationIsEnabled(ex.Message), ex);
                }
                catch (DiagnosticVerificationException ex)
                {
                    // Same for DiagnosticVerificationExceptions.
                    throw new ActivationException(
                        StringResources.EnableAutoVerificationIsEnabled(ex.Message), ex);
                }
            }
        }

        private void ThrowContainerDisposedException() =>
            throw new ObjectDisposedException(
                objectName: null,
                message: StringResources.ContainerCanNotBeUsedAfterDisposal(
                    this.GetType(), this.stackTraceThatDisposedTheContainer));

        private static object ThrowWhenResolveInterceptorReturnsNull(object? instance) =>
            instance ?? throw new ActivationException(StringResources.ResolveInterceptorDelegateReturnedNull());

        private IEnumerable<ResolveInterceptor> GetResolveInterceptorsFor(InitializationContext context) =>
            from resolveInterceptor in this.resolveInterceptors
            where resolveInterceptor.Predicate(context)
            select resolveInterceptor.Interceptor;

        private Action<T>[] GetInstanceInitializersFor<T>(Type type, Registration registration)
        {
            if (this.instanceInitializers.Count == 0)
            {
                return Helpers.Array<Action<T>>.Empty;
            }

            var context = new InitializerContext(registration);

            return (
                from instanceInitializer in this.instanceInitializers
                where instanceInitializer.AppliesTo(type, context)
                select instanceInitializer.CreateAction<T>(context))
                .ToArray();
        }

        private void RegisterOpenGeneric(
            Type serviceType,
            Type implementationType,
            Lifestyle lifestyle,
            Predicate<PredicateContext>? predicate = null)
        {
            Requires.IsGenericType(serviceType, nameof(serviceType));
            Requires.IsNotPartiallyClosed(serviceType, nameof(serviceType));
            Requires.ServiceOrItsGenericTypeDefinitionIsAssignableFromImplementation(
                serviceType, implementationType, nameof(serviceType));
            Requires.OpenGenericTypeDoesNotContainUnresolvableTypeArguments(
                serviceType, implementationType, nameof(implementationType));

            this.GetOrCreateRegistrationalEntry(serviceType)
                .AddGeneric(serviceType, implementationType, lifestyle, predicate);
        }

        private void AddInstanceProducer(InstanceProducer producer)
        {
            // HACK: Conditional registrations for IEnumerable<T> are not added as uncontrolled collection,
            // because that would lose the conditional ability and would cause collections to be appended
            // together, instead of selected conditionally. (see #468).
            if (typeof(IEnumerable<>).IsGenericTypeDefinitionOf(producer.ServiceType)
                && producer.IsUnconditional)
            {
                this.AddUncontrolledCollectionInstanceProducer(producer);
            }
            else
            {
                var entry = this.GetOrCreateRegistrationalEntry(producer.ServiceType);

                entry.Add(producer);

                this.RemoveExternalProducer(producer);

                // There could already be a 'null' entry in the root-producer cache, which will happen when
                // GetRegistration(X) is called before X is registered. We need to clear the cache to prevent
                // the cached 'null' to be returned from GetRegistration instead of this producer. See #909.
                this.rootProducerCache.Remove(producer.ServiceType);
            }
        }

        private void AddUncontrolledCollectionInstanceProducer(InstanceProducer producer)
        {
            Type itemType = producer.ServiceType.GetGenericArguments()[0];

            var resolver = this.GetContainerUncontrolledResolver(itemType);

            resolver.RegisterUncontrolledCollection(itemType, producer);
        }

        private int GetNumberOfConditionalRegistrationsFor(Type serviceType) =>
            this.GetRegistrationalEntryOrNull(serviceType)
                ?.GetNumberOfConditionalRegistrationsFor(serviceType) ?? 0;

        // Instead of using the this.registrations instance, this method takes a snapshot. This allows the
        // container to be thread-safe, without using locks.
        private InstanceProducer? GetInstanceProducerForType(
            Type serviceType, InjectionConsumerInfo consumer, Func<InstanceProducer?> buildInstanceProducer) =>
            this.GetExplicitlyRegisteredInstanceProducer(serviceType, consumer)
            ?? this.TryGetInstanceProducerForRegisteredCollection(serviceType)
            ?? buildInstanceProducer();

        private InstanceProducer? GetExplicitlyRegisteredInstanceProducer(
            Type serviceType, InjectionConsumerInfo consumer) =>
            this.GetRegistrationalEntryOrNull(serviceType)?.TryGetInstanceProducer(serviceType, consumer);

        private InstanceProducer? TryGetInstanceProducerForRegisteredCollection(Type enumerableServiceType) =>
            typeof(IEnumerable<>).IsGenericTypeDefinitionOf(enumerableServiceType)
                ? this.GetInstanceProducerForRegisteredCollection(
                    enumerableServiceType.GetGenericArguments()[0])
                : null;

        private InstanceProducer? GetInstanceProducerForRegisteredCollection(Type serviceType) =>
            this.collectionResolvers.GetValueOrDefault(GetRegistrationKey(serviceType))
                ?.TryGetInstanceProducer(serviceType);

        private IRegistrationEntry GetOrCreateRegistrationalEntry(Type serviceType)
        {
            Type key = GetRegistrationKey(serviceType);

            var entry = this.explicitRegistrations.GetValueOrDefault(key);

            if (entry is null)
            {
                this.explicitRegistrations[key] = entry = RegistrationEntry.Create(serviceType, this);
            }

            return entry;
        }

        private IRegistrationEntry? GetRegistrationalEntryOrNull(Type serviceType) =>
            this.explicitRegistrations.GetValueOrDefault(GetRegistrationKey(serviceType));

        private static Type GetRegistrationKey(Type serviceType) =>
            serviceType.IsGenericType()
                ? serviceType.GetGenericTypeDefinition()
                : serviceType;

        private Type[] GetLookalikesForMissingType(Type missingServiceType) =>
            this.GetLookalikesForMissingNonGenericType(missingServiceType)
                .Concat(this.GetLookalikesForMissingGenericTypeDefinitions(missingServiceType))
                .ToArray();

        // A lookalike type is a registered type that shares the same type name (where casing is ignored
        // and the parent type name of a nested type is included) as the missing type. Nested types are
        // mostly excluded from this list, because it would be quite common for developers to have lots
        // of nested types with the same name.
        private IEnumerable<Type> GetLookalikesForMissingNonGenericType(Type missingServiceType)
        {
            string name = missingServiceType.ToFriendlyName();

            return this.GetLookalikesFromCurrentRegistrationsForMissingNonAndClosedGenericType(name)
                .Concat(this.GetLookalikesFromExplictRegistrationsForMissingNonAndClosedGenericType(name))
                .Except(new[] { missingServiceType })
                .Distinct();
        }

        private IEnumerable<Type> GetLookalikesFromCurrentRegistrationsForMissingNonAndClosedGenericType(
            string name) =>
            from registration in this.GetCurrentRegistrations(false, includeExternalProducers: false)
            let typeName = registration.ServiceType.ToFriendlyName()
            where StringComparer.OrdinalIgnoreCase.Equals(typeName, name)
            select registration.ServiceType;

        private IEnumerable<Type> GetLookalikesFromExplictRegistrationsForMissingNonAndClosedGenericType(
            string name) =>
            from type in this.explicitRegistrations.Keys
            where !type.IsGenericTypeDefinition()
            let friendlyName = type.ToFriendlyName()
            where StringComparer.OrdinalIgnoreCase.Equals(friendlyName, name)
            select type;

        private IEnumerable<Type> GetLookalikesForMissingGenericTypeDefinitions(Type missingType)
        {
            if (!missingType.IsGenericType())
            {
                return Enumerable.Empty<Type>();
            }

            Type missingTypeDef = missingType.GetGenericTypeDefinition();

            string missingTypeDefName = missingTypeDef.ToFriendlyName();

            return
                from type in this.explicitRegistrations.Keys
                where type.IsGenericTypeDefinition()
                where type != missingTypeDef
                let friendlyName = type.GetGenericTypeDefinition().ToFriendlyName()
                where StringComparer.OrdinalIgnoreCase.Equals(friendlyName, missingTypeDefName)
                select type;
        }

        private sealed class ContextualResolveInterceptor
        {
            public readonly ResolveInterceptor Interceptor;
            public readonly Predicate<InitializationContext> Predicate;

            public ContextualResolveInterceptor(
                ResolveInterceptor interceptor, Predicate<InitializationContext> predicate)
            {
                this.Interceptor = interceptor;
                this.Predicate = predicate;
            }
        }

        private sealed class TypedInstanceInitializer : IInstanceInitializer
        {
            private readonly Type serviceType;
            private readonly object instanceInitializer;

            private TypedInstanceInitializer(Type serviceType, object instanceInitializer)
            {
                this.serviceType = serviceType;
                this.instanceInitializer = instanceInitializer;
            }

            public bool AppliesTo(Type implementationType, InitializerContext context) =>
                Types.GetTypeHierarchyFor(implementationType).Contains(this.serviceType);

            public Action<T> CreateAction<T>(InitializerContext context) =>
                Helpers.CreateAction<T>(this.instanceInitializer);

            internal static IInstanceInitializer Create<TImplementation>(Action<TImplementation> initializer)
            {
                return new TypedInstanceInitializer(typeof(TImplementation), initializer);
            }
        }

        private sealed class ContextualInstanceInitializer : IInstanceInitializer
        {
            private readonly Predicate<InitializerContext> predicate;
            private readonly Action<InstanceInitializationData> instanceInitializer;

            private ContextualInstanceInitializer(
                Predicate<InitializerContext> predicate,
                Action<InstanceInitializationData> instanceInitializer)
            {
                this.predicate = predicate;
                this.instanceInitializer = instanceInitializer;
            }

            public bool AppliesTo(Type implementationType, InitializerContext context) =>
                this.predicate(context);

            public Action<T> CreateAction<T>(InitializerContext context) =>
                instance => this.instanceInitializer(new InstanceInitializationData(context, instance!));

            internal static IInstanceInitializer Create(
                Action<InstanceInitializationData> instanceInitializer,
                Predicate<InitializerContext> predicate)
            {
                return new ContextualInstanceInitializer(predicate, instanceInitializer);
            }
        }
    }
}