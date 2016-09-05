namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    internal abstract class Node
    {
        public abstract bool HasError { get; }

        public abstract ReadOnlyObservableCollection<ValidationError> Errors { get; }

        public abstract ReadOnlyObservableCollection<IErrorNode> Children { get; }
    }
}