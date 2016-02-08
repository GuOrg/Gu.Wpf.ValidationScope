namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;

    public interface IErrorNode : INotifyPropertyChanged
    {
        bool HasErrors { get; }

        ReadOnlyObservableCollection<IErrorNode> Children { get; }

        DependencyObject Source { get; }
    }
}