namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Globalization;
    using System.Windows.Controls;

    public class TestValidationRule : ValidationRule
    {
        public static readonly TestValidationRule Default = new TestValidationRule();

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}