﻿// Copyright (c) Simple Injector Contributors. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace SimpleInjector.ProducerBuilders
{
    using System;

    internal interface IInstanceProducerBuilder
    {
        InstanceProducer? TryBuild(Type serviceType);
    }
}