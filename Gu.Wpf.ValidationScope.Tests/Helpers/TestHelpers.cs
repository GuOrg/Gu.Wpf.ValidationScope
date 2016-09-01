namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public static class TestHelpers
    {
        public static readonly DependencyProperty ProxyProperty = DependencyProperty.RegisterAttached(
            "Proxy",
            typeof(object),
            typeof(TestHelpers),
            new PropertyMetadata(default(object)));

        public static void SetValidationError(this TextBox textBox)
        {
            var expression = BindingOperations.GetBindingExpression(textBox, ProxyProperty)
                             ?? textBox.Bind(ProxyProperty).OneWayTo(textBox, TextBox.TextProperty);

            Validation.MarkInvalid(expression, TestValidationError.GetFor(expression));
        }

        public static void ClearValidationError(this TextBox textBox)
        {
            var expression = BindingOperations.GetBindingExpression(textBox, ProxyProperty);
            if (expression == null)
            {
                throw new InvalidOperationException();
            }

            Validation.ClearInvalid(expression);
        }
    }
}