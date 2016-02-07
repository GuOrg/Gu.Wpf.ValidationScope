namespace Gu.Wpf.ValidationScope
{
    using System.Windows;
    using System.Windows.Data;

    public static class Validation
    {
        public static readonly DependencyProperty IsValidationScopeProperty = DependencyProperty.RegisterAttached(
            "IsValidationScope",
            typeof(bool),
            typeof(Validation),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits,
                OnIsValidationScopeChanged));

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "HasErrors",
            typeof(bool),
            typeof(Validation),
            new PropertyMetadata(BooleanBoxes.False));

        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        private static readonly DependencyProperty ErrorCountProxyProperty = DependencyProperty.RegisterAttached(
            "ErrorCountProxy",
            typeof(int),
            typeof(Validation),
            new PropertyMetadata(default(int), OnErrorCountProxyChanged));

        public static void SetIsValidationScope(this UIElement element, bool value)
        {
            element.SetValue(IsValidationScopeProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetIsValidationScope(this UIElement element)
        {
            return (bool)element.GetValue(IsValidationScopeProperty);
        }

        private static void SetHasErrors(this UIElement element, bool value)
        {
            element.SetValue(HasErrorsPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetHasErrors(this UIElement element)
        {
            return (bool)element.GetValue(HasErrorsProperty);
        }

        private static void OnIsValidationScopeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Equals(e.NewValue, BooleanBoxes.True))
            {
                d.Bind(ErrorCountProxyProperty)
                 .OneWayTo(d, new PropertyPath("Validation.Errors.Count"));
            }
            else
            {
                BindingOperations.ClearBinding(d, ErrorCountProxyProperty);
            }
        }

        private static void OnErrorCountProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
