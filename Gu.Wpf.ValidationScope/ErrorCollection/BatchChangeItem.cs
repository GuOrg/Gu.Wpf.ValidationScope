namespace Gu.Wpf.ValidationScope
{
    internal static class BatchChangeItem
    {
        internal static BatchChangeItem<T> CreateAdd<T>(T item, int index)
        {
            return BatchChangeItem<T>.CreateAdd(item, index);
        }

        internal static BatchChangeItem<T> CreateRemove<T>(T item, int index)
        {
            return BatchChangeItem<T>.CreateRemove(item, index);
        }
    }
}