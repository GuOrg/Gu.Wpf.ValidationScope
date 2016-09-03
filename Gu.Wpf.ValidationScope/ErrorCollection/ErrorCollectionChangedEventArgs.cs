namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    public class ErrorCollectionChangedEventArgs : EventArgs
    {
        public ErrorCollectionChangedEventArgs(IReadOnlyList<ValidationError> removedItems, IReadOnlyList<ValidationError> addedItems)
        {
            this.RemovedItems = removedItems;
            this.AddedItems = addedItems;
        }

        public IReadOnlyList<ValidationError> RemovedItems { get; }

        public IReadOnlyList<ValidationError> AddedItems { get; }
    }
}