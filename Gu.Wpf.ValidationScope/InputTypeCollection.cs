using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    public class InputTypeCollection : Collection<Type>
    {
        public static readonly InputTypeCollection Defaut = new InputTypeCollection
        {
            typeof (TextBoxBase),
            typeof (Selector),
            typeof (ToggleButton),
            typeof (Slider)
        };

        public bool IsInputType(DependencyObject dependencyObject)
        {
            return this.Any(x => x.IsInstanceOfType(dependencyObject));
        }

        public void AddRange(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                Add(type);
            }
        }

        protected override void InsertItem(int index, Type item)
        {
            VerifyCompatible(item);
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Type item)
        {
            VerifyCompatible(item);
            base.SetItem(index, item);
        }

        protected virtual bool IsCompatibleType(Type type)
        {
            return typeof(UIElement).IsAssignableFrom(type) ||
                   typeof(ContentElement).IsAssignableFrom(type);
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