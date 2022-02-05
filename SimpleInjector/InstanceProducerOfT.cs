﻿// Copyright (c) Simple Injector Contributors. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace SimpleInjector
{
    // NOTE: In v5, InstanceProducer<TService>'s type constraint has been removed, making it possible to
    // create instance producers for value types as well. There were some use cases that require the creation
    // of InstanceProducers on value types, but that required the creation of non-generic InstanceProducer
    // instances, which is now not possible any longer.
    using System;

    /// <summary>
    /// Produces instances for a given registration. Instances of this type are generally created by the
    /// container when calling one of the <b>Register</b> overloads. Instances can be retrieved by calling
    /// <see cref="Container.GetCurrentRegistrations()"/> or <see cref="Container.GetRegistration(Type, bool)"/>.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    public class InstanceProducer<TService> : InstanceProducer
    {
        /// <summary>Initializes a new instance of the <see cref="InstanceProducer{TService}"/> class.</summary>
        /// <param name="registration">The <see cref="Registration"/>.</param>
        public InstanceProducer(Registration registration)
            : base(typeof(TService), registration)
        {
        }

        /// <summary>Produces an instance.</summary>
        /// <returns>An instance. Will never return null.</returns>
        /// <exception cref="ActivationException">When the instance could not be retrieved or is null.</exception>
        public new TService GetInstance() => (TService)base.GetInstance();
    }
}