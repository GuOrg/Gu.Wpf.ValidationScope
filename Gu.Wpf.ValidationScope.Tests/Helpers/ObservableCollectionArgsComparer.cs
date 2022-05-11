namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections;

    public sealed class ObservableCollectionArgsComparer : IComparer
    {
        public static readonly ObservableCollectionArgsComparer Default = new();

        public int Compare(object? x, object? y)
        {
            if (((IComparer)NotifyCollectionChangedEventArgsComparer.Default).Compare(x, y) == 0 ||
                ((IComparer)PropertyChangedEventArgsComparer.Default).Compare(x, y) == 0)
            {
                return 0;
            }

            return -1;
        }
    }
}
