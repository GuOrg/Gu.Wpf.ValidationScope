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

        public bool IsInputType(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                return false;
            }

            return this.Any(x => x.IsInstanceOfType(dependencyObject));
        }

        public void AddRange(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                this.Add(type);
            }
        }

        protected override void InsertItem(int index, Type item)
        {
            this.VerifyCompatible(item);
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Type item)
        {
            this.VerifyCompatible(item);
            base.SetItem(index, item);
        }

        protected virtual bool IsCompatible(Type type)
        {
            return IsCompatibleType(type);
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