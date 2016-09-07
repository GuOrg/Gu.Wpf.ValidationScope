namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using JetBrains.Annotations;

    internal class ChildCollection : ReadOnlyObservableCollection<ErrorNode>
    {
        public static readonly ChildCollection Empty = new ChildCollection();
        private readonly ObservableCollection<ErrorNode> children;

        public ChildCollection()
            : this(new ObservableCollection<ErrorNode>())
        {
        }

        private ChildCollection([NotNull] ObservableCollection<ErrorNode> children)
            : base(children)
        {
            this.children = children;
        }

        internal bool TryAdd(ErrorNode child)
        {
            if (this.children.Contains(child))
            {
                return false;
            }

            this.children.Add(child);
            return true;
        }

        internal bool Remove(ErrorNode child)
        {
            return this.children.Remove(child);
        }

        internal void RemoveAt(int index)
        {
            this.children.RemoveAt(index);
        }
    }
}
