namespace Gu.Wpf.ValidationScope;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// EventArgs for Scope.ValidationError event.
/// Wrote this class as <see cref="ValidationErrorEventArgs"/> as internal ctor.
/// </summary>
public class ScopeValidationErrorEventArgs : RoutedEventArgs
{
    internal ScopeValidationErrorEventArgs(ValidationError validationError, ValidationErrorEventAction action)
    {
        this.RoutedEvent = Scope.ValidationErrorEvent;
        this.Error = validationError;
        this.Action = action;
    }

    /// <summary>Gets the ValidationError that caused this ValidationErrorEvent to be raised.</summary>
    public ValidationError Error { get; }

    /// <summary>Gets the action that indicates whether the <see cref="Error"/> is a new error or a previous error that has now been cleared.</summary>
    public ValidationErrorEventAction Action { get; }
}