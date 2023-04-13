namespace Gu.Wpf.ValidationScope.Tests;

using System;
using System.Collections.ObjectModel;
using System.Windows;

using NUnit.Framework;

public static class ScopeEventsExt
{
    internal static EventList SubscribeScopeEvents(this FrameworkElement source)
    {
        return new EventList(source);
    }

    internal sealed class EventList : Collection<ScopeValidationErrorEventArgs>, IDisposable
    {
        private readonly FrameworkElement source;

        internal EventList(FrameworkElement source)
        {
            this.source = source;
            Scope.AddErrorHandler(source, this.Add);
        }

        public void Dispose()
        {
            Scope.AddErrorHandler(this.source, this.Add);
        }

        private void Add(object? sender, ScopeValidationErrorEventArgs e)
        {
            Assert.AreSame(this.source, sender);
            this.Add(e);
        }
    }
}
