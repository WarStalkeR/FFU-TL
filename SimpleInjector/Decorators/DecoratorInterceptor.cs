﻿// Copyright (c) Simple Injector Contributors. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace SimpleInjector.Decorators
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using SimpleInjector.Internals;

    internal class DecoratorInterceptor
    {
        // Cache for decorators when the decorator is registered as singleton. Since all decoration requests
        // for the registration of that decorator will go through the same instance, we can (or must)
        // define this dictionary as instance field (not as static or thread-static). When a decorator is
        // registered.
        // So the Type is an closed generic version of the open generic service that is wrapped, the
        // registration is the registration for the closed generic decorator.
        private readonly Dictionary<InstanceProducer, Registration> registrationsCache;
        private readonly Dictionary<InstanceProducer, IEnumerable> singletonDecoratedCollectionsCache;

        private readonly DecoratorExpressionInterceptorData data;

        public DecoratorInterceptor(DecoratorExpressionInterceptorData data)
        {
            this.registrationsCache = new Dictionary<InstanceProducer, Registration>();
            this.singletonDecoratedCollectionsCache = new Dictionary<InstanceProducer, IEnumerable>();

            this.data = data;
        }

        // The service type definition (possibly open generic).
        protected Type ServiceTypeDefinition => this.data.ServiceType;

        // The decorator type definition (possibly open generic).
        protected Type? DecoratorTypeDefinition => this.data.DecoratorType;

        internal void ExpressionBuilt(object sender, ExpressionBuiltEventArgs e)
        {
            this.TryToApplyDecorator(e);
            this.TryToApplyDecoratorOnContainerUncontrolledCollections(e);
        }

        private void TryToApplyDecorator(ExpressionBuiltEventArgs e)
        {
            if (this.MustDecorate(e.RegisteredServiceType, out Type? closedDecoratorType))
            {
                var decoratorInterceptor =
                    new ServiceDecoratorExpressionInterceptor(this.data, this.registrationsCache, e);

                if (decoratorInterceptor.SatisfiesPredicate())
                {
                    if (this.data.DecoratorTypeFactory != null)
                    {
                        // Context gets set by SatisfiesPredicate
                        var context = decoratorInterceptor.Context!;

                        closedDecoratorType = this.GetDecoratorTypeFromDecoratorFactory(
                            e.RegisteredServiceType, context);
                    }

                    if (closedDecoratorType != null)
                    {
                        decoratorInterceptor.ApplyDecorator(closedDecoratorType);
                    }
                }
            }
        }

        private void TryToApplyDecoratorOnContainerUncontrolledCollections(ExpressionBuiltEventArgs e)
        {
            if (!IsCollectionType(e.RegisteredServiceType) ||
                ControlledCollectionHelper.IsContainerControlledCollectionExpression(e.Expression))
            {
                // NOTE: Decorators on controlled collections will be applied by the normal mechanism.
                return;
            }

            var serviceType = e.RegisteredServiceType.GetGenericArguments()[0];

            if (this.MustDecorate(serviceType, out Type? decoratorType))
            {
                this.ApplyDecoratorOnContainerUncontrolledCollection(e, decoratorType);
            }
        }

        private void ApplyDecoratorOnContainerUncontrolledCollection(
            ExpressionBuiltEventArgs e, Type? decoratorType)
        {
            var serviceType = e.RegisteredServiceType.GetGenericArguments()[0];

            var uncontrolledInterceptor = new ContainerUncontrolledServicesDecoratorInterceptor(
                this.data, this.singletonDecoratedCollectionsCache, e, serviceType);

            if (uncontrolledInterceptor.SatisfiesPredicate())
            {
                if (this.data.DecoratorTypeFactory != null)
                {
                    // Context gets set by SatisfiesPredicate
                    var context = uncontrolledInterceptor.Context!;

                    decoratorType = this.GetDecoratorTypeFromDecoratorFactory(
                        serviceType, context);
                }

                if (decoratorType != null)
                {
                    uncontrolledInterceptor.SetDecorator(decoratorType);
                    uncontrolledInterceptor.ApplyDecorator();
                }
            }
        }

        private static bool IsCollectionType(Type serviceType) =>
            typeof(IEnumerable<>).IsGenericTypeDefinitionOf(serviceType);

        private bool MustDecorate(Type serviceType, out Type? decoratorType)
        {
            decoratorType = null;

            if (this.ServiceTypeDefinition == serviceType)
            {
                decoratorType = this.DecoratorTypeDefinition;

                return true;
            }

            if (!this.ServiceTypeDefinition.IsGenericTypeDefinitionOf(serviceType))
            {
                return false;
            }

            if (this.data.DecoratorTypeFactory != null)
            {
                // Since a decorator type factory delegate has been registered, we must assume at this point
                // that the decorator must be applied, because we can't call the factory at this point. The
                // predicate must always be called before calling the factory.
                return true;
            }

            var results = this.BuildClosedGenericImplementation(serviceType, this.DecoratorTypeDefinition!);

            if (!results.ClosedServiceTypeSatisfiesAllTypeConstraints)
            {
                return false;
            }

            decoratorType = results.ClosedGenericImplementation;

            return true;
        }

        private Type? GetDecoratorTypeFromDecoratorFactory(
            Type requestedServiceType, DecoratorPredicateContext context)
        {
            Type decoratorType = this.data.DecoratorTypeFactory!(context);

            if (decoratorType.ContainsGenericParameters())
            {
                if (!requestedServiceType.IsGenericType())
                {
                    throw new ActivationException(
                        StringResources.TheDecoratorReturnedFromTheFactoryShouldNotBeOpenGeneric(
                            requestedServiceType, decoratorType));
                }

                Requires.TypeFactoryReturnedTypeThatDoesNotContainUnresolvableTypeArguments(
                    requestedServiceType, decoratorType);

                var builder = new GenericTypeBuilder(requestedServiceType, decoratorType);

                var results = builder.BuildClosedGenericImplementation();

                // decoratorType is null when type constraints don't match.
                if (results.ClosedServiceTypeSatisfiesAllTypeConstraints)
                {
                    Requires.HasFactoryCreatedDecorator(
                        this.data.Container, requestedServiceType, results.ClosedGenericImplementation!);

                    return results.ClosedGenericImplementation;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Requires.FactoryReturnsATypeThatIsAssignableFromServiceType(
                    requestedServiceType, decoratorType);

                return decoratorType;
            }
        }

        private GenericTypeBuilder.BuildResult BuildClosedGenericImplementation(
            Type serviceType, Type decoratorTypeDefinition)
        {
            var builder = new GenericTypeBuilder(serviceType, decoratorTypeDefinition);

            return builder.BuildClosedGenericImplementation();
        }
    }
}