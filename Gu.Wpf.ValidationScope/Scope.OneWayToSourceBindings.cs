namespace Gu.Wpf.ValidationScope
{
    using System.Windows;
    using System.Windows.Data;

    public static partial class Scope
    {
        public static readonly DependencyProperty HasErrorsOneWayToSourceBindingProperty = DependencyProperty.RegisterAttached(
            "HasErrorsOneWayToSourceBinding",
            typeof(bool),
            typeof(Scope),
            new PropertyMetadata(BooleanBoxes.False));

        public static readonly DependencyProperty ErrorsOneWayToSourceBindingProperty = DependencyProperty.RegisterAttached(
            "ErrorsOneWayToSourceBinding",
            typeof(IErrorNode),
            typeof(Scope),
            new PropertyMetadata(default(IErrorNode)));

        public static void SetHasErrorsOneWayToSourceBinding(this UIElement element, Binding value) => element.SetValue(HasErrorsOneWayToSourceBindingProperty, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Binding GetHasErrorsOneWayToSourceBinding(this UIElement element) => (Binding)element.GetValue(HasErrorsOneWayToSourceBindingProperty);

        public static void SetErrorsOneWayToSourceBinding(this UIElement element, IErrorNode value) => element.SetValue(ErrorsOneWayToSourceBindingProperty, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IErrorNode GetErrorsOneWayToSourceBinding(this UIElement element) => (IErrorNode)element.GetValue(ErrorsOneWayToSourceBindingProperty);
    }
}
