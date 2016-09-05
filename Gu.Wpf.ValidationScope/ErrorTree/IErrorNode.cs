namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Controls;

    public interface IErrorNode : INotifyPropertyChanged, IDisposable
    {
        bool HasErrors { get; }

        ReadOnlyObservableCollection<ValidationError> Errors { get; }

        ReadOnlyObservableCollection<IErrorNode> Children { get; }
    }
}