namespace Gu.Wpf.ValidationScope
{
    internal struct BatchChangeItem<T>
    {
        public readonly T Item;
        public readonly int Index;
        public readonly BatchItemChangeAction Action;

        private BatchChangeItem(T item, int index, BatchItemChangeAction action)
        {
            this.Item = item;
            this.Index = index;
            this.Action = action;
        }

        internal static BatchChangeItem<T> CreateAdd(T item, int index)
        {
            return new BatchChangeItem<T>(item, index, BatchItemChangeAction.Add);
        }

        internal static BatchChangeItem<T> CreateRemove(T item, int index)
        {
            return new BatchChangeItem<T>(item, index, BatchItemChangeAction.Remove);
        }
    }
}