﻿// Copyright (c) Simple Injector Contributors. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace SimpleInjector
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq.Expressions;
    using SimpleInjector.Advanced;

    /// <summary>
    /// Provides data for and interaction with the
    /// <see cref="Container.ExpressionBuilding">ExpressionBuilding</see> event of
    /// the <see cref="Container"/>. An observer can change the
    /// <see cref="Expression"/> property to change the component that is
    /// currently being built.
    /// </summary>
    [DebuggerDisplay(nameof(ExpressionBuildingEventArgs) +
        " ({" + nameof(ExpressionBuildingEventArgs.DebuggerDisplay) + "), nq})")]
    public class ExpressionBuildingEventArgs : EventArgs
    {
        private Expression expression;

        internal ExpressionBuildingEventArgs(
            Type knownImplementationType,
            Expression expression,
            Lifestyle lifestyle,
            Collection<KnownRelationship> knownRelationships)
        {
            this.KnownImplementationType = knownImplementationType;
            this.Lifestyle = lifestyle;
            this.KnownRelationships = knownRelationships;
            this.expression = expression;
        }

        /// <summary>
        /// Gets the type that is known to be returned by the
        /// <see cref="Expression">Expression</see> (most often the implementation
        /// type used in the <b>Register</b> call). If the <b>Expression</b> is changed, the new expression
        /// must also return an instance of type <b>KnownImplementationType</b> or a sub type.
        /// This information must be described in the new Expression.
        /// </summary>
        /// <value>A <see cref="Type"/>.</value>
        public Type KnownImplementationType { get; }

        /// <summary>Gets the lifestyle for the component that is currently being built.</summary>
        /// <value>The <see cref="Lifestyle"/>.</value>
        public Lifestyle Lifestyle { get; }

        /// <summary>Gets or sets the currently registered
        /// <see cref="System.Linq.Expressions.Expression">Expression</see>.</summary>
        /// <value>The current registration.</value>
        /// <exception cref="ArgumentNullException">Thrown when the supplied value is a null reference.</exception>
        public Expression Expression
        {
            get
            {
                return this.expression;
            }

            set
            {
                Requires.IsNotNull(value, nameof(value));

                if (!this.KnownImplementationType.IsAssignableFrom(value.Type))
                {
                    throw new ArgumentException(
                        StringResources.KnownImplementationTypeShouldBeAssignableFromExpressionType(
                            this.KnownImplementationType,
                            value.Type));
                }

                this.expression = value;
            }
        }

        /// <summary>
        /// Gets the collection of currently known relationships. This information is used by the Diagnostics
        /// Debug View. Change the contents of this collection to represent the changes made to the
        /// <see cref="Expression">Expression</see> property (if any). This allows
        /// the Diagnostics Debug View to analyze those new relationships as well.
        /// </summary>
        /// <value>The collection of <see cref="KnownRelationship"/> instances.</value>
        public Collection<KnownRelationship> KnownRelationships { get; internal set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string DebuggerDisplay => string.Format(
            CultureInfo.InvariantCulture,
            "{0}: {1}, {2}: {3}",
            nameof(this.KnownImplementationType),
            this.KnownImplementationType.ToFriendlyName(),
            nameof(this.KnownImplementationType),
            this.KnownImplementationType.ToFriendlyName());
    }
}