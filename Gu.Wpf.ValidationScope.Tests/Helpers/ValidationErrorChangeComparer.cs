namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Windows.Controls;

    public class ValidationErrorChangeComparer : GenericComparer<BatchChangeItem>
    {
        public static readonly ValidationErrorChangeComparer Default = new ValidationErrorChangeComparer();

        public override int Compare(BatchChangeItem x, BatchChangeItem y)
        {
            if (x.Action != y.Action)
            {
                return -1;
            }

            if (!ReferenceEquals(x.Item, y.Item))
            {
                return -1;
            }

            return 0;
        }
    }
}