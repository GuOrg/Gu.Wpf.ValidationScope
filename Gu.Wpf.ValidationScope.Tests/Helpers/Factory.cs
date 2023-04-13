namespace Gu.Wpf.ValidationScope.Tests;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

internal static class Factory
{
    internal static ReadOnlyObservableCollection<T> CreateReadOnlyObservableCollection<T>(params T[] items)
    {
        return new ReadOnlyObservableCollection<T>(new ObservableCollection<T>(items));
    }

    internal static ValidationError CreateValidationError()
    {
        return TestValidationError.Create();
    }

    internal static object CreateAddedEventArgs(params ValidationError[] errors)
    {
        return CreateAddedEventArgs((IReadOnlyList<ValidationError>)errors);
    }

    internal static object CreateAddedEventArgs(IReadOnlyList<ValidationError> errors)
    {
        return new ErrorsChangedEventArgs(ErrorCollection.EmptyValidationErrors, errors);
    }

    internal static object CreateRemovedEventArgs(params ValidationError[] errors)
    {
        return CreateRemovedEventArgs((IReadOnlyList<ValidationError>)errors);
    }

    internal static object CreateRemovedEventArgs(IReadOnlyList<ValidationError> errors)
    {
        return new ErrorsChangedEventArgs(errors, ErrorCollection.EmptyValidationErrors);
    }

    internal static IReadOnlyList<EventArgs> ResetArgs()
    {
        var reference = new ObservableCollection<int> { 1 };
        var result = new List<EventArgs>();
        using var events = reference.SubscribeObservableCollectionEvents();
        reference.Clear();
        result.AddRange(events);
        return result;
    }
}
