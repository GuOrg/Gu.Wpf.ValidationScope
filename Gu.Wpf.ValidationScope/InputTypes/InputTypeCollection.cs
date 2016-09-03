namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>A collection of controls that will be subscribed to for validation errors.</summary>
    [TypeConverter(typeof(InputTypeCollectionConverter))]
    public class InputTypeCollection : Collection<Type>
    {
        public static readonly InputTypeCollection Default = new InputTypeCollection
        {
            typeof(Scope),
            typeof(TextBoxBase),
            typeof(ComboBox),
            typeof(ToggleButton),
            typeof(Slider)
        };

        public InputTypeCollection()
        {
        }

        public InputTypeCollection(IEnumerable<Type> types)
        {
            this.AddRange(types);
        }

        /// <summary>
        /// Check if <paramref name="type"/> can be an input type.
        /// Valid input types are { Scope, UIElement, ContentElement }
        /// </summary>
        /// <returns>True if <paramref name="type"/> is a possible input type.</returns>
        public static bool IsCompatibleType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            return type == typeof(Scope) ||
                   typeof(UIElement).IsAssignableFrom(type) ||
                   typeof(ContentElement).IsAssignableFrom(type);
        }

        /// <summary>
        /// Check if the collection contains <paramref name="dependencyObject"/> or a type that is a subclass of <paramref name="dependencyObject"/>
        /// </summary>
        /// <returns>True if <paramref name="dependencyObject"/> is an input type registered for the collection.</returns>
        public bool IsInputType(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                return false;
            }

            return this.Any(x => x.IsInstanceOfType(dependencyObject));
        }

        /// <summary>See <see cref="List{T}.AddRange"/>.</summary>
        public void AddRange(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                this.Add(type);
            }
        }

        /// <inheritdoc />
        protected override void InsertItem(int index, Type item)
        {
            this.VerifyCompatible(item);
            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        protected override void SetItem(int index, Type item)
        {
            this.VerifyCompatible(item);
            base.SetItem(index, item);
        }

        private void VerifyCompatible(Type type)
        {
            if (!IsCompatibleType(type))
            {
                throw new ArgumentException($"Type {type} is not compatible. Must be a type that works with Validation.");
            }
        }
    }
}