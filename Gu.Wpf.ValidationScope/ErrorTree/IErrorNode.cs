namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    public interface IErrorNode : INotifyPropertyChanged
    {
        bool HasErrors { get; }

        ReadOnlyObservableCollection<ValidationError> Errors { get; }

        ReadOnlyObservableCollection<IErrorNode> Children { get; }

        DependencyObject Source { get; }
    }
}