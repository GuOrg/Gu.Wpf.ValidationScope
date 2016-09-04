namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using JetBrains.Annotations;

    internal class ChildCollection : ReadOnlyObservableCollection<IErrorNode>
    {
        public static readonly ChildCollection Empty = new ChildCollection();
        private readonly ObservableCollection<IErrorNode> children;

        public ChildCollection()
            : this(new ObservableCollection<IErrorNode>())
        {
        }

        private ChildCollection([NotNull] ObservableCollection<IErrorNode> children)
            : base(children)
        {
            this.children = children;
        }

        internal bool TryAdd(IErrorNode child)
        {
            if (this.children.Contains(child))
            {
                return false;
            }

            this.children.Add(child);
            return true;
        }

        internal bool Remove(IErrorNode child)
        {
            return this.children.Remove(child);
        }

        internal void RemoveAt(int index)
        {
            this.children.RemoveAt(index);
        }
    }
}
