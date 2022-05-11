namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Linq;

    internal sealed class NotifyCollectionChangedEventArgsComparer : GenericComparer<NotifyCollectionChangedEventArgs>
    {
        internal static readonly NotifyCollectionChangedEventArgsComparer Default = new();

        protected override int Compare(NotifyCollectionChangedEventArgs x, NotifyCollectionChangedEventArgs y)
        {
            if (x.Action != y.Action)
            {
                return -1;
            }

            if (x.NewStartingIndex != y.NewStartingIndex)
            {
                return -1;
            }

            if (!AreItemsEqual(x.NewItems, y.NewItems))
            {
                return -1;
            }

            if (x.OldStartingIndex != y.OldStartingIndex)
            {
                return -1;
            }

            if (!AreItemsEqual(x.OldItems, y.OldItems))
            {
                return -1;
            }

            return 0;
        }

        private static bool AreItemsEqual(IList x, IList y)
        {
            if (x is null && y is null)
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return Enumerable.SequenceEqual(x.Cast<object>(), y.Cast<object>());
        }
    }
}
