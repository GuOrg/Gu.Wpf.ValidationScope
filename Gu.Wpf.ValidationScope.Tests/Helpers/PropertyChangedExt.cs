namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using NUnit.Framework;

    public static class PropertyChangedExt
    {
        public static EventCollection SubscribePropertyChangedEvents(this INotifyPropertyChanged source)
        {
            return new EventCollection(source);
        }

        public sealed class EventCollection : Collection<PropertyChangedEventArgs>, IDisposable
        {
            private readonly INotifyPropertyChanged source;

            public EventCollection(INotifyPropertyChanged source)
            {
                this.source = source ?? throw new ArgumentNullException(nameof(source));
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
