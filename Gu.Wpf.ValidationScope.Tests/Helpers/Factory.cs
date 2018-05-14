namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    internal static class Factory
    {
        public static ReadOnlyObservableCollection<T> CreateReadOnlyObservableCollection<T>(params T[] items)
        {
            return new ReadOnlyObservableCollection<T>(new ObservableCollection<T>(items));
        }

        public static ValidationError CreateValidationError()
        {
            return TestValidationError.Create();
        }

        public static object CreateAddedEventArgs(params ValidationError[] errors)
        {
            return CreateAddedEventArgs((IReadOnlyList<ValidationError>)errors);
        }

        public static object CreateAddedEventArgs(IReadOnlyList<ValidationError> errors)
        {
            return new ErrorsChangedEventArgs(ErrorCollection.EmptyValidationErrors, errors);
        }

        public static object CreateRemovedEventArgs(params ValidationError[] errors)
        {
            return CreateRemovedEventArgs((IReadOnlyList<ValidationError>)errors);
        }

        public static object CreateRemovedEventArgs(IReadOnlyList<ValidationError> errors)
        {
            return new ErrorsChangedEventArgs(errors, ErrorCollection.EmptyValidationErrors);
        }

        public static IReadOnlyList<EventArgs> ResetArgs()
        {
            var reference = new ObservableCollection<int> { 1 };
            using (var events = reference.SubscribeObservableCollectionEvents())
            {
                reference.Clear();
#pragma warning disable IDISP011 // Don't return disposed instance.
                return events;
#pragma warning restore IDISP011 // Don't return disposed instance.
            }
        }
    }
}
