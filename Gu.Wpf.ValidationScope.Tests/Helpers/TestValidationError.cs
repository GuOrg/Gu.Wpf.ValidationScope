namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class TestValidationError : ValidationError
    {
        private static readonly BindingExpressionBase DummyBindingExpression = BindingOperations.SetBinding(new DependencyObject(), FrameworkElement.DataContextProperty, new Binding());

        private TestValidationError(BindingExpressionBase bindingInError)
            : base(TestValidationRule.Default, bindingInError)
        {
        }

        public static TestValidationError GetFor(BindingExpressionBase expression)
        {
            return new TestValidationError(expression);
        }

        public static TestValidationError Create()
        {
            return new TestValidationError(DummyBindingExpression);
        }

        public static TestValidationError GetFor(UIElement element, DependencyProperty property)
        {
            return TestValidationError.GetFor(BindingOperations.SetBinding(element, property, new Binding("Foo")));
        }
    }
}