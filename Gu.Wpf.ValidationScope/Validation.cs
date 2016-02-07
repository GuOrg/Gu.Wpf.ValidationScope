namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

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

        private static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Errors",
            typeof(AggregateErrors),
            typeof(Validation),
            new PropertyMetadata(default(AggregateErrors), OnErrorsChanged));

        public static readonly DependencyProperty ErrorsProperty = ErrorsPropertyKey.DependencyProperty;

        private static readonly DependencyProperty ErrorCountProxyProperty = DependencyProperty.RegisterAttached(
            "ErrorCountProxy",
            typeof(int),
            typeof(Validation),
            new PropertyMetadata(default(int), OnErrorCountProxyChanged));

        private static readonly PropertyPath ErrorCountPropertyPath = new PropertyPath("(Validation.Errors).Count");

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

        private static void SetErrors(this UIElement element, AggregateErrors value)
        {
            element.SetValue(ErrorsPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static AggregateErrors GetErrors(this UIElement element)
        {
            return (AggregateErrors)element.GetValue(ErrorsProperty);
        }

        private static void OnIsValidationScopeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(d.GetType().FullName);
            if (Equals(e.NewValue, BooleanBoxes.True))
            {
                var errorCountExpression = d.Bind(ErrorCountProxyProperty)
                                            .OneWayTo(d, ErrorCountPropertyPath);
                d.SetValue(ErrorsPropertyKey, new AggregateErrors(errorCountExpression));
            }
            else
            {
                BindingOperations.ClearBinding(d, ErrorCountProxyProperty);
                d.ClearValue(ErrorsPropertyKey);
            }
        }

        private static void OnErrorCountProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var errors = (AggregateErrors)d.GetValue(ErrorsProperty);
            if (errors != null)
            {
                d.SetValue(ErrorsPropertyKey, errors);
                var hasErrors = errors.HasErrors;
                var parent = VisualTreeHelper.GetParent(d);
                while (parent != null)
                {
                    var parentErrors = (AggregateErrors)parent.GetValue(ErrorsProperty);
                    if (parentErrors == null)
                    {
                        break;
                    }

                    parentErrors.UpdateChildErrors(errors, hasErrors);
                    parent.SetValue(ErrorsPropertyKey, parentErrors);
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }

            Console.WriteLine("Error count:" + e.NewValue);
        }

        private static void OnErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var hasErrors = e.NewValue == null
                ? BooleanBoxes.False
                : BooleanBoxes.Box(((AggregateErrors)e.NewValue).HasErrors);
            d.SetValue(HasErrorsPropertyKey, hasErrors);
        }
    }
}
