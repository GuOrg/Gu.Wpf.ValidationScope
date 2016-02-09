namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Specialized;

    internal static class BatchChangeItem
    {
        public static BatchChangeItem<T> CreateAdd<T>(T item, int index)
        {
            return new BatchChangeItem<T>(item, index, NotifyCollectionChangedAction.Add);
        }

        public static BatchChangeItem<T> CreateRemove<T>(T item, int index)
        {
            return new BatchChangeItem<T>(item, index, NotifyCollectionChangedAction.Remove);
        }
    }
}