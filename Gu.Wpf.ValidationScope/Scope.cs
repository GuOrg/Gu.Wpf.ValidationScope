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
            typeof(ErrorNode),
            typeof(Scope),
            new PropertyMetadata(default(ErrorNode), OnErrorsChanged));

        public static readonly DependencyProperty ErrorsProperty = ErrorsPropertyKey.DependencyProperty;

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

        private static void SetErrors(this DependencyObject element, ErrorNode value)
        {
            element.SetValue(ErrorsPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ErrorNode GetErrors(UIElement element)
        {
            return (ErrorNode)element.GetValue(ErrorsProperty);
        }

        private static void OnScopeForChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((InputTypeCollection)e.NewValue)?.IsInputType(d) == true)
            {
                if (BindingOperations.GetBindingExpression(d, ErrorCountProxyProperty) != null)
                {
                    return;
                }


                d.SetValue(ErrorsPropertyKey, new ErrorNode(errorCountExpression));
            }
            else
            {
                d.ClearValue(ErrorsPropertyKey);
            }
        }

        private static void OnErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var hasErrors = e.NewValue == null
                ? BooleanBoxes.False
                : BooleanBoxes.Box(((ErrorNode)e.NewValue).HasErrors);
            d.SetValue(HasErrorsPropertyKey, hasErrors);
        }
    }
}
