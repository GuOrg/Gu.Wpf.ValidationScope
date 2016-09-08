namespace Gu.Wpf.ValidationScope
{
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
            this.RoutedEvent = Scope.ErrorEvent;
            this.Error = validationError;
            this.Action = action;
        }

        /// <summary>The ValidationError that caused this ValidationErrorEvent to be raised.</summary>
        public ValidationError Error { get; }

        /// <summary>Action indicates whether the <seealso cref="Error"/> is a new error or a previous error that has now been cleared.</summary>
        public ValidationErrorEventAction Action { get; }
    }
}