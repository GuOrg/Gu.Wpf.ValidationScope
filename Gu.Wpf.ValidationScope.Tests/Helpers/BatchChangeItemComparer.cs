namespace Gu.Wpf.ValidationScope.Tests
{
    public class BatchChangeItemComparer<T> : GenericComparer<BatchChangeItem<T>>
    {
        public static readonly BatchChangeItemComparer<T> Default = new BatchChangeItemComparer<T>();

        public override int Compare(BatchChangeItem<T> x, BatchChangeItem<T> y)
        {
            if (x.Action != y.Action)
            {
                return -1;
            }

            if (x.Index != y.Index)
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