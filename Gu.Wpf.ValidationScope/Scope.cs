namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows;

    public static partial class Scope
    {
#pragma warning disable SA1202 // Elements must be ordered by access

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
            new PropertyMetadata(BooleanBoxes.False, OnHasErrorsChanged));

        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Errors",
            typeof(IErrorNode),
            typeof(Scope),
            new PropertyMetadata(default(IErrorNode), OnErrorsChanged));

        public static readonly DependencyProperty ErrorsProperty = ErrorsPropertyKey.DependencyProperty;

#pragma warning restore SA1202 // Elements must be ordered by access

        public static void SetForInputTypes(this UIElement element, InputTypeCollection value) => element.SetValue(ForInputTypesProperty, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static InputTypeCollection GetForInputTypes(DependencyObject element) => (InputTypeCollection)element.GetValue(ForInputTypesProperty);

        internal static void SetHasErrors(DependencyObject element, bool value) => element.SetValue(HasErrorsPropertyKey, BooleanBoxes.Box(value));

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetHasErrors(UIElement element) => (bool)element.GetValue(HasErrorsProperty);

        internal static void SetErrors(DependencyObject element, IErrorNode value) => element.SetValue(ErrorsPropertyKey, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IErrorNode GetErrors(DependencyObject element) => (IErrorNode)element.GetValue(ErrorsProperty);

        private static void OnScopeForChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((InputTypeCollection)e.NewValue)?.IsInputType(d) == true)
            {
                if (GetErrors(d) == null)
                {
                    SetErrors(d, ErrorNode.CreateFor(d));
                }
            }
            else if (((InputTypeCollection)e.OldValue)?.IsInputType(d) == true)
            {
                d.ClearValue(ErrorsPropertyKey);
            }
        }

        private static void OnErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (e.OldValue as IDisposable)?.Dispose();
            SetHasErrors(d, ((IErrorNode)e.NewValue)?.HasErrors == true);
            (e.NewValue as ErrorNode)?.BindToErrors();
            d.SetCurrentValue(ErrorsOneWayToSourceBindingProperty, e.NewValue);
        }

        private static void OnHasErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(HasErrorsOneWayToSourceBindingProperty, e.NewValue);
        }
    }
}
