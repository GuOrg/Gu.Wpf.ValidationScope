namespace Gu.Wpf.ValidationScope
{
    using System;

    /// <summary>
    /// For signaling changes in errors.
    /// </summary>
    internal interface INotifyErrorsChanged
    {
        /// <summary>
        /// Signals that errors changed.
        /// </summary>
        event EventHandler<ErrorsChangedEventArgs> ErrorsChanged;
    }
}
