namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Collections.ObjectModel;

    using NUnit.Framework;

    public static class ErrorCollectionExt
    {
        internal static EventList SubscribeErrorCollectionEvents(this ErrorCollection col)
        {
            return new EventList(col);
        }

        internal class EventList : Collection<ErrorsChangedEventArgs>, IDisposable
        {
            private readonly ErrorCollection source;

            public EventList(ErrorCollection source)
            {
                this.source = source;
                source.ErrorsChanged += this.Add;
            }

            public void Dispose()
            {
                this.source.ErrorsChanged -= this.Add;
            }

            private void Add(object sender, ErrorsChangedEventArgs e)
            {
                Assert.AreSame(this.source, sender);
                this.Add(e);
            }
        }
    }
}