namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Controls;

    public interface IErrorNode : IReadOnlyList<ValidationError>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
    {
        bool HasErrors { get; }

        ReadOnlyObservableCollection<ValidationError> Errors { get; }

        ReadOnlyObservableCollection<IErrorNode> Children { get; }
    }
}