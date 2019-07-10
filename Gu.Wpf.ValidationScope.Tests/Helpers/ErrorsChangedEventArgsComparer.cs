namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Linq;

    internal class ErrorsChangedEventArgsComparer : GenericComparer<ErrorsChangedEventArgs>
    {
        internal static readonly ErrorsChangedEventArgsComparer Default = new ErrorsChangedEventArgsComparer();

        protected override int Compare(ErrorsChangedEventArgs x, ErrorsChangedEventArgs y)
        {
            if (!Enumerable.SequenceEqual(x.Added, y.Added))
            {
                return -1;
            }

            if (!Enumerable.SequenceEqual(x.Removed, y.Removed))
            {
                return -1;
            }

            return 0;
        }
    }
}
