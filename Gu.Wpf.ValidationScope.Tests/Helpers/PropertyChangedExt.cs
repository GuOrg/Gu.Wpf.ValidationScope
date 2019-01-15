namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using NUnit.Framework;

    public static class PropertyChangedExt
    {
        public static EventList SubscribePropertyChangedEvents(this INotifyPropertyChanged source)
        {
            return new EventList(source);
        }

        public sealed class EventList : Collection<PropertyChangedEventArgs>, IDisposable
        {
            private readonly INotifyPropertyChanged source;

            public EventList(INotifyPropertyChanged source)
            {
                this.source = source;
                source.PropertyChanged += this.Add;
            }

            public void Dispose()
            {
                this.source.PropertyChanged -= this.Add;
            }

            private void Add(object sender, PropertyChangedEventArgs e)
            {
                Assert.AreSame(this.source, sender);
                this.Add(e);
            }
        }
    }
}
