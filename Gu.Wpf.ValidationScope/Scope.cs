namespace Gu.Wpf.ValidationScope
{
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public static class Scope
    {
        public static readonly DependencyProperty ForInputTypesProperty = DependencyProperty.RegisterAttached(
            "ForInputTypes",
            typeof(InputTypeCollection),
            typeof(Scope),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnScopeForChanged));

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "HasErrors",
            typeof(bool),
            typeof(Scope),
            new PropertyMetadata(BooleanBoxes.False));

        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Errors",
            typeof(AggregateErrors),
            typeof(Scope),
            new PropertyMetadata(default(AggregateErrors), OnErrorsChanged));

        public static readonly DependencyProperty ErrorsProperty = ErrorsPropertyKey.DependencyProperty;

        private static readonly DependencyProperty ErrorCountProxyProperty = DependencyProperty.RegisterAttached(
            "ErrorCountProxy",
            typeof(int),
            typeof(Scope),
            new PropertyMetadata(
                default(int),
                OnErrorCountProxyChanged));

        private static readonly PropertyPath ErrorCountPropertyPath = new PropertyPath("(Validation.Errors).Count");

        public static void SetForInputTypes(this UIElement element, InputTypeCollection value)
        {
            element.SetValue(ForInputTypesProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static InputTypeCollection GetForInputTypes(this UIElement element)
        {
            return (InputTypeCollection)element.GetValue(ForInputTypesProperty);
        }

        private static void SetHasErrors(this DependencyObject element, bool value)
        {
            element.SetValue(HasErrorsPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetHasErrors(UIElement element)
        {
            return (bool)element.GetValue(HasErrorsProperty);
        }

        private static void SetErrors(this DependencyObject element, AggregateErrors value)
        {
            element.SetValue(ErrorsPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static AggregateErrors GetErrors(UIElement element)
        {
            return (AggregateErrors)element.GetValue(ErrorsProperty);
        }

        private static void OnScopeForChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((InputTypeCollection)e.NewValue)?.IsInputType(d) == true)
            {
                if (BindingOperations.GetBindingExpression(d, ErrorCountProxyProperty) != null)
                {
                    return;
                }

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
                var hasErrors = errors.HasErrors;
                d.SetValue(HasErrorsPropertyKey, BooleanBoxes.Box(hasErrors));
                var parent = VisualTreeHelper.GetParent(d);
                while (parent != null)
                {
                    if (parent.GetValue(ForInputTypesProperty) != null)
                    {
                        break;
                    }

                    var parentErrors = (AggregateErrors)parent.GetValue(ErrorsProperty);
                    if (parentErrors == null)
                    {
                        if (hasErrors)
                        {
                            parentErrors = new AggregateErrors(errors);
                            parent.SetValue(ErrorsProperty, parentErrors);
                        }
                    }
                    else
                    {
                        parentErrors.UpdateChildErrors(errors, hasErrors);
                    }

                    parent.SetValue(HasErrorsPropertyKey, BooleanBoxes.Box(parentErrors?.HasErrors == true));
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
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
