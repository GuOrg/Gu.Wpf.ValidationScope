namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Windows.Controls;

    public class ValidationErrorChangeComparer : GenericComparer<ValidationErrorChange>
    {
        public static readonly ValidationErrorChangeComparer Default = new ValidationErrorChangeComparer();

        public override int Compare(ValidationErrorChange x, ValidationErrorChange y)
        {
            if (x.Action != y.Action)
            {
                return -1;
            }

            if (!ReferenceEquals(x.Error, y.Error))
            {
                return -1;
            }

            return 0;
        }
    }
}