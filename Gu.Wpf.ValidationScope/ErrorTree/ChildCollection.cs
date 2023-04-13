namespace Gu.Wpf.ValidationScope;

using System.Collections.ObjectModel;

internal sealed class ChildCollection : ReadOnlyObservableCollection<ErrorNode>
{
    internal static readonly ChildCollection Empty = new();
    private readonly ObservableCollection<ErrorNode> children;

    internal ChildCollection()
        : this(new ObservableCollection<ErrorNode>())
    {
    }

    private ChildCollection(ObservableCollection<ErrorNode> children)
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
