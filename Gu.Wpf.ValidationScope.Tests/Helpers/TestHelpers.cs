namespace Gu.Wpf.ValidationScope.Tests;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using NUnit.Framework;

public static class TestHelpers
{
    public static void SetValidationError(this DependencyObject textBox, ValidationError error)
    {
        var expression = (BindingExpressionBase)error.BindingInError;
        Assert.AreSame(textBox, expression.Target);
        Validation.MarkInvalid(expression, error);
    }

    public static void ClearValidationError(this TextBox textBox, ValidationError error)
    {
        var expression = (BindingExpressionBase)error.BindingInError;
        Assert.AreSame(textBox, expression.Target);
        Validation.ClearInvalid(expression);
    }
}
