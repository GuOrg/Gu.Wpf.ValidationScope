namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Specialized;

    internal static class BatchChangeItem
    {
        internal static BatchChangeItem<T> CreateAdd<T>(T item, int index)
        {
            return new BatchChangeItem<T>(item, index, NotifyCollectionChangedAction.Add);
        }

        internal static BatchChangeItem<T> CreateRemove<T>(T item, int index)
        {
            return new BatchChangeItem<T>(item, index, NotifyCollectionChangedAction.Remove);
        }
    }
}