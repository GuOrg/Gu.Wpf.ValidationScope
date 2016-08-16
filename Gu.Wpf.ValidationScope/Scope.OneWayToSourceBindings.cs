namespace Gu.Wpf.ValidationScope
{
    using System.Windows;
    using System.Windows.Data;

    public static partial class Scope
    {
        public static readonly DependencyProperty HasErrorOneWayToSourceBindingProperty = DependencyProperty.RegisterAttached(
            "HasErrorOneWayToSourceBinding",
            typeof(bool),
            typeof(Scope),
            new PropertyMetadata(BooleanBoxes.False));

        public static readonly DependencyProperty ErrorsOneWayToSourceBindingProperty = DependencyProperty.RegisterAttached(
            "ErrorsOneWayToSourceBinding",
            typeof(IErrorNode),
            typeof(Scope),
            new PropertyMetadata(default(IErrorNode)));

        public static void SetHasErrorOneWayToSourceBinding(this UIElement element, Binding value) => element.SetValue(HasErrorOneWayToSourceBindingProperty, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Binding GetHasErrorOneWayToSourceBinding(this UIElement element) => (Binding)element.GetValue(HasErrorOneWayToSourceBindingProperty);

        public static void SetErrorsOneWayToSourceBinding(this UIElement element, IErrorNode value) => element.SetValue(ErrorsOneWayToSourceBindingProperty, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IErrorNode GetErrorsOneWayToSourceBinding(this UIElement element) => (IErrorNode)element.GetValue(ErrorsOneWayToSourceBindingProperty);
    }
}
