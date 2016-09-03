namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public static class ObservableCollectionExt
    {
        public static EventList<T> SubscribeAllEvents<T>(this T col)
             where T : IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
        {
            return new EventList<T>(col);
 }

        public class EventList<T> : Collection<EventArgs>, IDisposable
                        where T : IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
        {
            private readonly T source;

            public EventList(T source)
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

            private void Add(object sender, NotifyCollectionChangedEventArgs e)
            {
                this.Add(e);
            }

            private void Add(object sender, PropertyChangedEventArgs e)
            {
                this.Add(e);
            }
        }
    }
}
