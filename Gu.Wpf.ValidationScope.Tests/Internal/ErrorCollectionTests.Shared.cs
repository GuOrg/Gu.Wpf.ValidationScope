namespace Gu.Wpf.ValidationScope.Tests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Controls;

    public partial class ErrorCollectionTests
    {
        private static ReadOnlyObservableCollection<ValidationError> Create(int n)
        {
            var errors = new ObservableCollection<ValidationError>();
            for (int i = 0; i < n; i++)
            {
                errors.Add(TestValidationError.Create());
            }

            return new ReadOnlyObservableCollection<ValidationError>(errors);
        }

        private static List<EventArgs> SubscribeAllEvents<T>(T col)
            where T : IEnumerable<ValidationError>, INotifyCollectionChanged, INotifyPropertyChanged
        {
            var args = new List<EventArgs>();
            col.PropertyChanged += (_, e) => args.Add(e);
            col.CollectionChanged += (_, e) => args.Add(e);
            return args;
        }
    }
}
