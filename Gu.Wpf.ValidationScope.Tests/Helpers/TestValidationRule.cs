namespace Gu.Wpf.ValidationScope.Tests;

using System.Globalization;
using System.Windows.Controls;

public sealed class TestValidationRule : ValidationRule
{
    public static readonly TestValidationRule Default = new();

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
#pragma warning disable GU0090 // Don't throw NotImplementedException.
        throw new System.NotImplementedException();
#pragma warning restore GU0090 // Don't throw NotImplementedException.
    }
}
