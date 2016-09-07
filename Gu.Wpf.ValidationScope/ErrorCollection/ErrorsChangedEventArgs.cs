namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    public class ErrorsChangedEventArgs : RoutedEventArgs
    {
        public ErrorsChangedEventArgs(IEnumerable<ValidationError> removed, IEnumerable<ValidationError> added)
        {
            this.RoutedEvent = Scope.ErrorsChangedEvent;
            this.Removed = removed ?? ErrorCollection.EmptyValidationErrors;
            this.Added = added ?? ErrorCollection.EmptyValidationErrors;
        }

        internal IEnumerable<ValidationError> Removed { get; }

        internal IEnumerable<ValidationError> Added { get; }
    }
}