namespace Gu.Wpf.ValidationScope.Tests;

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using NUnit.Framework;

public static class ObservableCollectionExt
{
    public static EventCollection<T> SubscribeObservableCollectionEvents<T>(this T col)
        where T : IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
    {
        return new EventCollection<T>(col);
    }

    public sealed class EventCollection<T> : Collection<EventArgs>, IDisposable
        where T : IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly T source;

        public EventCollection(T source)
        {
            this.source = source;
            source.PropertyChanged += this.Add;
            source.CollectionChanged += this.Add;
        }

        public void Dispose()
        {
            this.source.PropertyChanged -= this.Add;
            this.source.CollectionChanged -= this.Add;
        }

        private void Add(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Assert.AreSame(this.source, sender);
            this.Add(e);
        }

        private void Add(object? sender, PropertyChangedEventArgs e)
        {
            Assert.AreSame(this.source, sender);
            this.Add(e);
        }
    }
}