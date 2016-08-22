namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static partial class Scope
    {
        private static readonly Binding BindingNotSet = new Binding { Mode = BindingMode.OneWayToSource };

        public static readonly DependencyProperty HasErrorBindingProperty = DependencyProperty.RegisterAttached(
            "HasErrorBinding",
            typeof(Binding),
            typeof(Scope),
            new PropertyMetadata(BindingNotSet, OnHasErrorBindingChanged, OnHasErrorBindingCoerce),
            OnValidateOneWayToSourceBinding);

        public static readonly DependencyProperty ErrorsBindingProperty = DependencyProperty.RegisterAttached(
            "ErrorsBinding",
            typeof(Binding),
            typeof(Scope),
            new PropertyMetadata(BindingNotSet, OnErrorsBindingChanged, OnErrorsBindingCoerce),
            OnValidateOneWayToSourceBinding);

        public static readonly DependencyProperty NodeBindingProperty = DependencyProperty.RegisterAttached(
            "NodeBinding",
            typeof(Binding),
            typeof(Scope),
            new PropertyMetadata(BindingNotSet, OnNodeBindingChanged, OnNodeBindingCoerce),
            OnValidateOneWayToSourceBinding);

        private static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.RegisterAttached(
            "HasErrorProxy",
            typeof(bool),
            typeof(Scope),
            new PropertyMetadata(default(bool)));

        private static readonly DependencyProperty ErrorsProxyProperty = DependencyProperty.RegisterAttached(
            "ErrorsProxy",
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(Scope),
            new PropertyMetadata(default(ReadOnlyObservableCollection<ValidationError>)));

        private static readonly DependencyProperty NodeProxyProperty = DependencyProperty.RegisterAttached(
            "NodeProxy",
            typeof(IErrorNode),
            typeof(Scope),
            new PropertyMetadata(default(IErrorNode)));

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Binding GetHasErrorBinding(this UIElement element) => (Binding)element.GetValue(HasErrorBindingProperty);

        public static void SetHasErrorBinding(this UIElement element, Binding value) => element.SetValue(HasErrorBindingProperty, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Binding GetErrorsBinding(this UIElement element) => (Binding)element.GetValue(ErrorsBindingProperty);

        public static void SetErrorsBinding(this UIElement element, Binding value) => element.SetValue(ErrorsBindingProperty, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Binding GetNodeBinding(this UIElement element) => (Binding)element.GetValue(NodeBindingProperty);

        public static void SetNodeBinding(this UIElement element, Binding value) => element.SetValue(NodeBindingProperty, value);

        private static void OnHasErrorBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindingOperations.ClearBinding(d, HasErrorProxyProperty);
            if (e.NewValue != null && e.NewValue != BindingNotSet)
            {
                BindingOperations.SetBinding(d, HasErrorProxyProperty, (Binding)e.NewValue);
            }
        }

        private static void OnErrorsBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindingOperations.ClearBinding(d, ErrorsProxyProperty);
            if (e.NewValue != null && e.NewValue != BindingNotSet)
            {
                BindingOperations.SetBinding(d, ErrorsProxyProperty, (Binding)e.NewValue);
            }
        }

        private static void OnNodeBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindingOperations.ClearBinding(d, NodeProxyProperty);
            if (e.NewValue != null && e.NewValue != BindingNotSet)
            {
                BindingOperations.SetBinding(d, NodeProxyProperty, (Binding)e.NewValue);
            }
        }

        private static object OnHasErrorBindingCoerce(DependencyObject d, object basevalue)
        {
            if (basevalue != BindingNotSet && basevalue is Binding)
            {
                return basevalue;
            }

            var binding = BindingOperations.GetBinding(d, HasErrorBindingProperty);
            if (binding == null)
            {
                return null;
            }

            if (binding.Mode == BindingMode.Default)
            {
                binding = binding.Clone(BindingMode.OneWayToSource);
            }

            if (binding.Mode != BindingMode.OneWayToSource)
            {
                if (Is.DesignMode)
                {
                    throw new ArgumentException("Binding.Mode for HasErrorBinding must be BindingMode.OneWayToSource");
                }
            }

            d.SetValue(HasErrorBindingProperty, binding);
            return binding;
        }

        private static object OnErrorsBindingCoerce(DependencyObject d, object basevalue)
        {
            if (basevalue != BindingNotSet && basevalue is Binding)
            {
                return basevalue;
            }

            var binding = BindingOperations.GetBinding(d, ErrorsBindingProperty);
            if (binding == null)
            {
                return null;
            }

            if (binding.Mode == BindingMode.Default)
            {
                binding = binding.Clone(BindingMode.OneWayToSource);
            }

            if (binding.Mode != BindingMode.OneWayToSource)
            {
                if (Is.DesignMode)
                {
                    throw new ArgumentException("Binding.Mode for ErrorsBinding must be BindingMode.OneWayToSource");
                }
            }

            d.SetValue(ErrorsBindingProperty, binding);
            return binding;
        }

        private static object OnNodeBindingCoerce(DependencyObject d, object basevalue)
        {
            if (basevalue != BindingNotSet && basevalue is Binding)
            {
                return basevalue;
            }

            var binding = BindingOperations.GetBinding(d, NodeBindingProperty);
            if (binding == null)
            {
                return null;
            }

            if (binding.Mode == BindingMode.Default)
            {
                binding = binding.Clone(BindingMode.OneWayToSource);
            }

            if (binding.Mode != BindingMode.OneWayToSource)
            {
                if (Is.DesignMode)
                {
                    throw new ArgumentException("Binding.Mode for NodeBinding must be BindingMode.OneWayToSource");
                }
            }

            d.SetValue(NodeBindingProperty, binding);
            return binding;
        }

        private static bool OnValidateOneWayToSourceBinding(object value)
        {
            if (value == null)
            {
                return true;
            }

            if ((value as Binding)?.Mode != BindingMode.OneWayToSource)
            {
                return false;
            }

            return true;
        }
    }
}
