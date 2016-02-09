namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Specialized;

    internal struct BatchChangeItem<T>
    {
        public readonly T Item;
        public readonly int Index;
        public readonly NotifyCollectionChangedAction Action;

        public BatchChangeItem(T item, int index, NotifyCollectionChangedAction action)
        {
            this.Item = item;
            this.Index = index;
            this.Action = action;
        }
    }
}