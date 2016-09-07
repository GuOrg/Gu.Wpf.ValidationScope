namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    public abstract class Node
    {
        public abstract bool HasError { get; }

        public abstract ReadOnlyObservableCollection<ValidationError> Errors { get; }

        public abstract ReadOnlyObservableCollection<ErrorNode> Children { get; }
    }
}