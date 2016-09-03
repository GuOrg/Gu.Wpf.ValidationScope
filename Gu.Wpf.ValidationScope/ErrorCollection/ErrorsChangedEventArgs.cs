namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    internal class ErrorsChangedEventArgs : EventArgs
    {
        public ErrorsChangedEventArgs(IReadOnlyList<ValidationError> removed, IReadOnlyList<ValidationError> added)
        {
            this.Removed = removed;
            this.Added = added;
        }

        internal IReadOnlyList<ValidationError> Removed { get; }

        internal IReadOnlyList<ValidationError> Added { get; }
    }
}