﻿// Copyright (c) Simple Injector Contributors. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace SimpleInjector.Lifestyles
{
    using System;

    internal sealed class HybridLifestyle : Lifestyle, IHybridLifestyle
    {
        private readonly Predicate<Container> lifestyleSelector;
        private readonly Lifestyle trueLifestyle;
        private readonly Lifestyle falseLifestyle;

        internal HybridLifestyle(
            Predicate<Container> lifestyleSelector, Lifestyle trueLifestyle, Lifestyle falseLifestyle)
            : base("Hybrid " + GetHybridName(trueLifestyle) + " / " + GetHybridName(falseLifestyle))
        {
            this.lifestyleSelector = lifestyleSelector;
            this.trueLifestyle = trueLifestyle;
            this.falseLifestyle = falseLifestyle;
        }

        public override int Length =>
            throw new NotSupportedException("The length property is not supported for this lifestyle.");

        string IHybridLifestyle.GetHybridName() =>
            GetHybridName(this.trueLifestyle) + " / " + GetHybridName(this.falseLifestyle);

        internal override int ComponentLength(Container container) =>
            Math.Max(
                this.trueLifestyle.ComponentLength(container),
                this.falseLifestyle.ComponentLength(container));

        internal override int DependencyLength(Container container) =>
            Math.Min(
                this.trueLifestyle.DependencyLength(container),
                this.falseLifestyle.DependencyLength(container));

        internal static string GetHybridName(Lifestyle lifestyle) =>
            (lifestyle as IHybridLifestyle)?.GetHybridName() ?? lifestyle.Name;

        protected internal override Registration CreateRegistrationCore(Type concreteType, Container container) =>
            new HybridRegistration(
                implementationType: concreteType,
                test: () => this.lifestyleSelector(container),
                trueRegistration: this.trueLifestyle.CreateRegistration(concreteType, container),
                falseRegistration: this.falseLifestyle.CreateRegistration(concreteType, container),
                lifestyle: this,
                container: container);

        protected internal override Registration CreateRegistrationCore<TService>(
            Func<TService> instanceCreator, Container container) =>
            new HybridRegistration(
                implementationType: typeof(TService),
                test: () => this.lifestyleSelector(container),
                trueRegistration: this.trueLifestyle.CreateRegistration(instanceCreator, container),
                falseRegistration: this.falseLifestyle.CreateRegistration(instanceCreator, container),
                lifestyle: this,
                container: container);
    }
}