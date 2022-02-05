﻿// Copyright (c) Simple Injector Contributors. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace SimpleInjector
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using SimpleInjector.Advanced;
    using SimpleInjector.Internals;

    /// <summary>
    /// An instance of this type will be supplied to the <see cref="Predicate{T}"/>
    /// delegate that is that is supplied to the
    /// <see cref="Container.RegisterConditional(Type, Type, Lifestyle, Predicate{PredicateContext})">RegisterConditional</see>
    /// overload that takes this delegate. This type contains information about the service that
    /// is about to be created and it allows the user to examine the given instance to decide whether this
    /// implementation should be created or not.
    /// </summary>
    /// <remarks>
    /// Please see the
    /// <see cref="Container.RegisterConditional(Type, Type, Lifestyle, Predicate{PredicateContext})">Register</see>
    /// method for more information.
    /// </remarks>
    [DebuggerDisplay(nameof(PredicateContext) + " ({" + nameof(DebuggerDisplay) + ", nq})")]
    public sealed class PredicateContext : ApiObject
    {
        private readonly InjectionConsumerInfo consumer;
        private readonly LazyEx<Type> implementationType;

        internal PredicateContext(InstanceProducer producer, InjectionConsumerInfo consumer, bool handled)
            : this(producer.ServiceType, producer.Registration.ImplementationType, consumer, handled)
        {
        }

        internal PredicateContext(
            Type serviceType, Type implementationType, InjectionConsumerInfo consumer, bool handled)
        {
            Requires.IsNotNull(serviceType, nameof(serviceType));
            Requires.IsNotNull(implementationType, nameof(implementationType));
            Requires.IsNotNull(consumer, nameof(consumer));

            this.ServiceType = serviceType;
            this.implementationType = new LazyEx<Type>(implementationType);
            this.consumer = consumer;
            this.Handled = handled;
        }

        internal PredicateContext(
            Type serviceType,
            Func<Type?> implementationTypeProvider,
            InjectionConsumerInfo consumer,
            bool handled)
        {
            Requires.IsNotNull(serviceType, nameof(serviceType));
            Requires.IsNotNull(implementationTypeProvider, nameof(implementationTypeProvider));
            Requires.IsNotNull(consumer, nameof(consumer));

            this.ServiceType = serviceType;

            // HACK: LazyEx does not support null (as a simplification and memory optimization). This is why
            // the dummy type is returned when the provider returns null.
            this.implementationType =
                new LazyEx<Type>(() => implementationTypeProvider() ?? typeof(NullMarkerDummy));
            this.consumer = consumer;
            this.Handled = handled;
        }

        /// <summary>Gets the closed generic service type that is to be created.</summary>
        /// <value>The closed generic service type.</value>
        public Type ServiceType { get; }

        /// <summary>
        /// Gets the closed generic implementation type that will be created by the container.
        /// </summary>
        /// <value>The implementation type.</value>
        public Type? ImplementationType
        {
            get
            {
                Type type = this.implementationType.Value;

                return type == typeof(NullMarkerDummy) ? null : type;
            }
        }

        /// <summary>Gets a value indicating whether a previous <b>Register</b> registration has already
        /// been applied for the given <see cref="ServiceType"/>.</summary>
        /// <value>The indication whether the event has been handled.</value>
        public bool Handled { get; }

        /// <summary>
        /// Gets the contextual information of the consuming component that directly depends on the registered
        /// service. This property will never return null, but instead throw an exception when the service is
        /// requested directly from the container.
        /// </summary>
        /// <value>The <see cref="InjectionConsumerInfo"/>.</value>
        /// <exception cref="InvalidOperationException">Thrown when the service described by this instance
        /// is requested directly from the container, opposed to being injected into a consumer.</exception>
        public InjectionConsumerInfo Consumer
        {
            get
            {
                if (!this.HasConsumer)
                {
                    throw new InvalidOperationException(
                        StringResources.CallingPredicateContextConsumerOnDirectResolveIsNotSupported(this));
                }

                return this.consumer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the resolved service is injected into a consumer or is requested
        /// directly from the container.
        /// </summary>
        /// <value>True when the service is injected into a consumer; false when it is requested directly
        /// from the container</value>
        public bool HasConsumer => this.consumer != InjectionConsumerInfo.Root;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string DebuggerDisplay => string.Format(
            CultureInfo.InvariantCulture,
            "{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}",
            nameof(this.ServiceType),
            this.ServiceType.ToFriendlyName(),
            nameof(this.ImplementationType),
            this.ImplementationType?.ToFriendlyName(),
            nameof(this.Handled),
            this.Handled,
            nameof(this.Consumer),
            this.Consumer);

        private sealed class NullMarkerDummy
        {
        }
    }
}