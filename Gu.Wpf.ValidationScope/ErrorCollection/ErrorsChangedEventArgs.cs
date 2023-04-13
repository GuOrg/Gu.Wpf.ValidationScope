namespace Gu.Wpf.ValidationScope;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// EventArgs for Scope.ErrorsChanged event.
/// </summary>
public class ErrorsChangedEventArgs : RoutedEventArgs
{
    /// <summary>Initializes a new instance of the <see cref="ErrorsChangedEventArgs"/> class.</summary>
    /// <param name="removed">Removed validation errors.</param>
    /// <param name="added">New validation errors.</param>
    public ErrorsChangedEventArgs(IEnumerable<ValidationError> removed, IEnumerable<ValidationError> added)
    {
        this.RoutedEvent = Scope.ErrorsChangedEvent;
        this.Removed = removed ?? ErrorCollection.EmptyValidationErrors;
        this.Added = added ?? ErrorCollection.EmptyValidationErrors;
    }

    /// <summary>Gets the validation errors that were removed.</summary>
    internal IEnumerable<ValidationError> Removed { get; }

    /// <summary>Gets the validation errors that were added.</summary>
    internal IEnumerable<ValidationError> Added { get; }
}