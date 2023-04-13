// ReSharper disable StaticMemberInGenericType
namespace Gu.Wpf.ValidationScope;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

internal class ObservableBatchCollection<T> : ObservableCollection<T>
{
    private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new(nameof(Count));
    private static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new("Item[]");
    private static readonly NotifyCollectionChangedEventArgs NotifyCollectionResetEventArgs = new(NotifyCollectionChangedAction.Reset);

    internal void AddRange(IEnumerable<T> items)
    {
        this.CheckReentrancy();
        var before = this.Items.Count;
        using var e = items.GetEnumerator();
        while (e.MoveNext())
        {
            this.Items.Add(e.Current);
        }

        if (this.Items.Count == before + 1)
        {
            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this.Items[this.Items.Count - 1], this.Items.Count - 1));
        }
        else if (this.Items.Count != before)
        {
            this.RaiseReset();
        }
    }

    internal void RemoveRange(IEnumerable<T> items)
    {
        this.CheckReentrancy();
        var before = this.Items.Count;
        //// using KeyValuePair here, not dragging in reference to value tuple just for this.
        KeyValuePair<int, T>? first = null;
        using var e = items.GetEnumerator();
        while (e.MoveNext())
        {
            if (first is null &&
                this.Items.IndexOf(e.Current) is { } i &&
                i >= 0)
            {
                first = new KeyValuePair<int, T>(i, e.Current);
            }

            while (this.Items.Remove(e.Current))
            {
            }
        }

        if (this.Items.Count == before - 1 &&
            first is { Key: { } index, Value: var removed })
        {
            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed, index));
        }
        else if (this.Items.Count != before)
        {
            this.RaiseReset();
        }
    }

    private void RaiseReset()
    {
        this.OnPropertyChanged(CountPropertyChangedEventArgs);
        this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
        this.OnCollectionChanged(NotifyCollectionResetEventArgs);
    }
}
