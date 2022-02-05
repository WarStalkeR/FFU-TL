﻿// Copyright (c) Simple Injector Contributors. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace SimpleInjector
{
    using System;
    using System.Collections.Generic;
    using SimpleInjector.Internals;
    using SimpleInjector.ProducerBuilders;

#if !PUBLISH
    /// <summary>Methods for resolving instances.</summary>
#endif
    public partial class Container : IServiceProvider
    {
        // Cache for producers that are resolved as root type
        // PERF: The rootProducerCache uses a special equality comparer that does a quicker lookup of types.
        // PERF: This collection is updated by replacing the complete collection.
        private Dictionary<Type, InstanceProducer?> rootProducerCache =
            new(ReferenceEqualityComparer<Type>.Instance);

        private enum MutableCollectionType
        {
            Array,
            List,
        }

        /// <summary>Gets an instance of the given <typeparamref name="TService"/>.</summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="ActivationException">Thrown when there are errors resolving the service instance.</exception>
        public TService GetInstance<TService>()
            where TService : class
        {
            this.ThrowWhenDisposed();
            this.LockContainer();

            // Performance optimization: This if check is a duplicate to save a call to GetInstanceForType.
            if (!this.rootProducerCache.TryGetValue(typeof(TService), out InstanceProducer? instanceProducer))
            {
                return (TService)this.GetInstanceForRootType(typeof(TService));
            }

            if (instanceProducer is null)
            {
                this.ThrowMissingInstanceProducerException(typeof(TService));
            }

            return (TService)instanceProducer!.GetInstance();
        }

        /// <summary>Gets an instance of the given <paramref name="serviceType"/>.</summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="ActivationException">Thrown when there are errors resolving the service instance.</exception>
        public object GetInstance(Type serviceType)
        {
            this.ThrowWhenDisposed();
            this.LockContainer();

            if (!this.rootProducerCache.TryGetValue(serviceType, out InstanceProducer? instanceProducer))
            {
                return this.GetInstanceForRootType(serviceType);
            }

            if (instanceProducer is null)
            {
                this.ThrowMissingInstanceProducerException(serviceType);
            }

            return instanceProducer!.GetInstance();
        }

        /// <summary>
        /// Gets all instances of the given <typeparamref name="TService"/> currently registered in the container.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <returns>A sequence of instances of the requested TService.</returns>
        /// <exception cref="ActivationException">Thrown when there are errors resolving the service instance.</exception>
        public IEnumerable<TService> GetAllInstances<TService>()
            where TService : class
        {
            return this.GetInstance<IEnumerable<TService>>();
        }

        /// <summary>
        /// Gets all instances of the given <paramref name="serviceType"/> currently registered in the container.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <returns>A sequence of instances of the requested serviceType.</returns>
        /// <exception cref="ActivationException">Thrown when there are errors resolving the service instance.</exception>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            Requires.IsNotNull(serviceType, nameof(serviceType));

            Type collectionType = typeof(IEnumerable<>).MakeGenericType(serviceType);

            return (IEnumerable<object>)this.GetInstance(collectionType);
        }

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType.  -or- null if there is no service object of type
        /// <paramref name="serviceType"/>.</returns>
        object? IServiceProvider.GetService(Type serviceType)
        {
            Requires.IsNotNull(serviceType, nameof(serviceType));
            this.ThrowWhenDisposed();
            this.LockContainer();

            if (!this.rootProducerCache.TryGetValue(serviceType, out InstanceProducer? instanceProducer))
            {
                instanceProducer = this.GetRegistration(serviceType);
            }

            return instanceProducer?.IsValid == true
                ? instanceProducer.GetInstance()
                : null;
        }

        /// <summary>
        /// Gets the <see cref="InstanceProducer"/> for the given <paramref name="serviceType"/>. When no
        /// registration exists, the container will try creating a new producer. A producer can be created
        /// when the type is a concrete reference type, there is an <see cref="ResolveUnregisteredType"/>
        /// event registered that acts on that type, or when the service type is an <see cref="IEnumerable{T}"/>.
        /// Otherwise <b>null</b> is returned.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A call to this method locks the container. New registrations can't be made after a call to this
        /// method.
        /// </para>
        /// <para>
        /// <b>Note:</b> This method is <i>not</i> guaranteed to always return the same
        /// <see cref="InstanceProducer"/> instance for a given <see cref="System.Type"/>. It will however either
        /// always return <b>null</b> or always return a producer that is able to return the expected instance.
        /// </para>
        /// </remarks>
        /// <param name="serviceType">The <see cref="System.Type"/> that the returned instance producer should produce.</param>
        /// <returns>An <see cref="InstanceProducer"/> or <b>null</b>.</returns>
        public InstanceProducer? GetRegistration(Type serviceType)
        {
            return this.GetRegistration(serviceType, throwOnFailure: false);
        }

        /// <summary>
        /// Gets the <see cref="InstanceProducer"/> for the given <typeparamref name="TService"/>. When no
        /// registration exists, the container will try creating a new producer. A producer can be created
        /// when the type is a concrete reference type, there is an <see cref="ResolveUnregisteredType"/>
        /// event registered that acts on that type, or when the service type is an <see cref="IEnumerable{T}"/>.
        /// Otherwise <b>null</b> is returned.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A call to this method locks the container. New registrations can't be made after a call to this
        /// method.
        /// </para>
        /// <para>
        /// <b>Note:</b> This method is <i>not</i> guaranteed to always return the same
        /// <see cref="InstanceProducer"/> instance for a given <see cref="System.Type"/>. It will however either
        /// always return <b>null</b> or always return a producer that is able to return the expected instance.
        /// </para>
        /// </remarks>
        /// <returns>An <see cref="InstanceProducer"/> or <b>null</b>.</returns>
        public InstanceProducer? GetRegistration<TService>()
            where TService : class
        {
            return this.GetRegistration<TService>(throwOnFailure: false);
        }

        /// <summary>
        /// Gets the <see cref="InstanceProducer{TService}"/> for the given <typeparamref name="TService"/>.When no
        /// registration exists, the container will try creating a new producer. A producer can be created
        /// when the type is a concrete reference type, there is an <see cref="ResolveUnregisteredType"/>
        /// event registered that acts on that type, or when the service type is an <see cref="IEnumerable{T}"/>.
        /// Otherwise <b>null</b> is returned, or an exception is throw when
        /// <paramref name="throwOnFailure"/> is set to <b>true</b>.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <remarks>
        /// <para>
        /// A call to this method might lock the container.
        /// </para>
        /// <para>
        /// <b>Note:</b> This method is <i>not</i> guaranteed to always return the same
        /// <see cref="InstanceProducer"/> instance for a given <see cref="Type"/>. It will however either
        /// always return <b>null</b> or always return a producer that is able to return the expected instance.
        /// </para>
        /// </remarks>
        /// <param name="throwOnFailure">The indication whether the method should return null or throw
        /// an exception when the type is not registered.</param>
        /// <returns>An <see cref="InstanceProducer"/> or <b>null</b>.</returns>
        public InstanceProducer? GetRegistration<TService>(bool throwOnFailure)
        {
            return this.GetRegistration(typeof(TService), throwOnFailure);
        }

        /// <summary>
        /// Gets the <see cref="InstanceProducer"/> for the given <paramref name="serviceType"/>. When no
        /// registration exists, the container will try creating a new producer. A producer can be created
        /// when the type is a concrete reference type, there is an <see cref="ResolveUnregisteredType"/>
        /// event registered that acts on that type, or when the service type is an <see cref="IEnumerable{T}"/>.
        /// Otherwise <b>null</b> is returned, or an exception is throw when
        /// <paramref name="throwOnFailure"/> is set to <b>true</b>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A call to this method might lock the container.
        /// </para>
        /// <para>
        /// <b>Note:</b> This method is <i>not</i> guaranteed to always return the same
        /// <see cref="InstanceProducer"/> instance for a given <see cref="System.Type"/>. It will however either
        /// always return <b>null</b> or always return a producer that is able to return the expected instance.
        /// </para>
        /// </remarks>
        /// <param name="serviceType">The <see cref="Type"/> that the returned instance producer should produce.</param>
        /// <param name="throwOnFailure">The indication whether the method should return null or throw
        /// an exception when the type is not registered.</param>
        /// <returns>An <see cref="InstanceProducer"/> or <b>null</b>.</returns>
        //// Yippie, we broke a framework design guideline rule here :-).
        //// 7.1 DO NOT have public members that can either throw or not based on some option.
        public InstanceProducer? GetRegistration(Type serviceType, bool throwOnFailure)
        {
            Requires.IsNotNull(serviceType, nameof(serviceType));

            // GetRegistration might lock the container, but only when not-explicitly made registrations are
            // requested.
            this.ThrowWhenDisposed();

            if (!this.rootProducerCache.TryGetValue(serviceType, out InstanceProducer? producer))
            {
                producer = this.GetExplicitlyRegisteredInstanceProducer(serviceType, InjectionConsumerInfo.Root);

                if (producer is null)
                {
                    producer = this.GetRegistrationEvenIfInvalid(
                        serviceType, InjectionConsumerInfo.Root, autoCreateConcreteTypes: true);

                    if (producer != null)
                    {
                        // The producer is created implicitly. This forces us to lock the container. Such
                        // implicit registration could be done through in numerous ways (e.g. through
                        // unregistered type resolution, or because the type is concrete). Being able to make
                        // registrations after such call, could lead to unexpected behavior, which is why
                        // locking the container is important.
                        // We don't lock when the producer is null, even though unregistered type resolution
                        // events may have been invoked. The assumption is that events that don't add the
                        // registration have no effect on the system.
                        // NOTE: Lock should be called *after* getting the producer, because it would
                        // otherwise cause the configuration to be verified without that just-added producer.
                        this.LockContainer();
                    }
                }

                // PERF: Add the producer, even when it's null to prevent this if-block from re-executing when
                // the method is called more often for the same service type. Performance of this if-block is
                // slow and the users might call it many times in the happy path of their application. Also
                // note that GetService is calling back into this method
                this.AppendRootInstanceProducer(serviceType, producer);
            }

            bool producerIsValid = producer?.IsValid == true;

            if (!producerIsValid && throwOnFailure)
            {
                this.ThrowInvalidRegistrationException(serviceType, producer);
            }

            // Prevent returning invalid producers
            return producerIsValid ? producer : null;
        }

        internal Action<object>? GetInitializer(Type implementationType, Registration context)
        {
            return this.GetInitializer<object>(implementationType, context);
        }

        internal InstanceProducer? GetRegistrationEvenIfInvalid(
            Type serviceType, InjectionConsumerInfo consumer, bool autoCreateConcreteTypes = true)
        {
            if (serviceType.ContainsGenericParameters())
            {
                throw new ArgumentException(
                    StringResources.OpenGenericTypesCanNotBeResolved(serviceType), nameof(serviceType));
            }

            // This local function is a bit ugly, but does save us a lot of duplicate code.
            InstanceProducer? BuildProducer() =>
                this.BuildInstanceProducerForType(serviceType, autoCreateConcreteTypes);

            return this.GetInstanceProducerForType(serviceType, consumer, BuildProducer);
        }

        internal bool IsConcreteConstructableType(Type concreteType) =>
            this.Options.IsConstructableType(concreteType, out _);

        internal InstanceProducer? GetInstanceProducerForType(Type serviceType, InjectionConsumerInfo context)
        {
            return this.GetInstanceProducerForType(
                serviceType, context, () => this.BuildInstanceProducerForType(serviceType, true));
        }

        private Action<T>? GetInitializer<T>(Type implementationType, Registration context)
        {
            Action<T>[] initializersForType = this.GetInstanceInitializersFor<T>(implementationType, context);

            if (initializersForType.Length == 0)
            {
                return null;
            }
            else if (initializersForType.Length == 1)
            {
                return initializersForType[0];
            }
            else
            {
                return obj =>
                {
                    for (int index = 0; index < initializersForType.Length; index++)
                    {
                        initializersForType[index](obj);
                    }
                };
            }
        }

        private object GetInstanceForRootType(Type serviceType)
        {
            if (serviceType.ContainsGenericParameters())
            {
                throw new ArgumentException(
                    StringResources.OpenGenericTypesCanNotBeResolved(serviceType), nameof(serviceType));
            }

            InstanceProducer? producer = this.GetInstanceProducerForType(serviceType, InjectionConsumerInfo.Root);
            this.AppendRootInstanceProducer(serviceType, producer);
            return this.GetInstanceFromProducer(producer, serviceType);
        }

        private object GetInstanceFromProducer(InstanceProducer? instanceProducer, Type serviceType)
        {
            if (instanceProducer is null)
            {
                this.ThrowMissingInstanceProducerException(serviceType);
            }

            // We create the instance AFTER registering the instance producer. Registering the producer after
            // creating an instance, could make us loose all registrations that are done by GetInstance. This
            // will not have any functional effects, but can result in a performance penalty.
            return instanceProducer!.GetInstance();
        }

        private InstanceProducer? BuildInstanceProducerForType(
            Type serviceType, bool autoCreateConcreteTypes = true)
        {
            foreach (IInstanceProducerBuilder builder in this.producerBuilders)
            {
                var producer = builder.TryBuild(serviceType);

                if (producer != null)
                {
                    return producer;
                }
            }

            return autoCreateConcreteTypes
                ? this.unregisteredConcreteTypeProducerBuilder.TryBuild(serviceType)
                : null;
        }

        // We're registering a service type after 'locking down' the container here and that means that the
        // type is added to a copy of the registrations dictionary and the original replaced with a new one.
        // This 'reference swapping' is thread-safe, but can result in types disappearing again from the
        // registrations when multiple threads simultaneously add different types. This however, does not
        // result in a consistency problem, because the missing type will be again added later. This type of
        // swapping safes us from using locks.
        private void AppendRootInstanceProducer(Type serviceType, InstanceProducer? rootProducer)
        {
            Helpers.InterlockedAddAndReplace(ref this.rootProducerCache, serviceType, rootProducer);

            if (rootProducer != null)
            {
                this.RemoveExternalProducer(rootProducer);
            }
        }

        private void ThrowInvalidRegistrationException(Type serviceType, InstanceProducer? producer)
        {
            if (producer != null)
            {
                // Exception is never null in this context.
                throw producer.Exception!;
            }
            else
            {
                this.ThrowMissingInstanceProducerException(serviceType);
            }
        }

        internal void ThrowMissingInstanceProducerException(Type type)
        {
            if (Types.IsConcreteConstructableType(type))
            {
                this.ThrowNotConstructableException(type);
            }

            throw new ActivationException(StringResources.NoRegistrationForTypeFound(
                type,
                numberOfConditionals: this.GetNumberOfConditionalRegistrationsFor(type),
                containerHasRegistrations: this.HasRegistrations,
                collectionRegistrationDoesNotExists: this.IsCollectionButNoOneToToOneRegistrationExists(type),
                containerHasRelatedOneToOneMapping: this.ContainsOneToOneRegistrationForCollection(type),
                containerHasRelatedCollectionMapping: this.ContainsCollectionRegistrationFor(type),
                skippedDecorators: this.GetNonGenericDecoratorsSkippedDuringAutoRegistration(type),
                lookalikes: this.GetLookalikesForMissingType(type)));
        }

        private bool IsCollectionButNoOneToToOneRegistrationExists(Type collectionServiceType) =>
            Types.IsGenericCollectionType(collectionServiceType)
            && !this.ContainsOneToOneRegistrationForCollection(collectionServiceType);

        private bool ContainsOneToOneRegistrationForCollection(Type collectionServiceType) =>
            Types.IsGenericCollectionType(collectionServiceType)
            && this.ContainsExplicitRegistrationFor(collectionServiceType.GetGenericArguments()[0]);

        // NOTE: MakeGenericType will fail for IEnumerable<T> when T is a pointer.
        private bool ContainsCollectionRegistrationFor(Type serviceType) =>
            !Types.IsGenericCollectionType(serviceType)
            && !serviceType.IsPointer
            && this.ContainsExplicitRegistrationFor(typeof(IEnumerable<>).MakeGenericType(serviceType));

        private bool ContainsExplicitRegistrationFor(Type serviceType) =>
            this.GetRegistrationEvenIfInvalid(serviceType, InjectionConsumerInfo.Root, false) != null;

        private void ThrowNotConstructableException(Type concreteType)
        {
            // At this point we know the concreteType is either NOT constructable or
            // Options.ResolveUnregisteredConcreteTypes is configured to not return the type.
            this.Options.IsConstructableType(concreteType, out string? exceptionMessage);

            throw new ActivationException(
                StringResources.ImplicitRegistrationCouldNotBeMadeForType(
                    this, concreteType, this.HasRegistrations) +
                    " " + exceptionMessage);
        }
    }
}