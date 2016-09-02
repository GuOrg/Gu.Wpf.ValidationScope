namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Controls;

    public class ErrorCollectionResetEventArgs : NotifyCollectionChangedEventArgs
    {
        internal ErrorCollectionResetEventArgs(IReadOnlyList<BatchChangeItem<ValidationError>> changes)
            : base(NotifyCollectionChangedAction.Reset)
        {
            this.RemovedItems = GetChanges(changes, BatchItemChangeAction.Remove);
            this.AddedItems = GetChanges(changes, BatchItemChangeAction.Add);
        }

        public IReadOnlyList<ValidationError> RemovedItems { get; }

        public IReadOnlyList<ValidationError> AddedItems { get; }

        private static IReadOnlyList<ValidationError> GetChanges(IReadOnlyList<BatchChangeItem<ValidationError>> changes, BatchItemChangeAction action)
        {
            // not efficient here but not expecting many validation errors.
            return changes.Where(c => c.Action == action)
                          .Select(x => x.Item)
                          .Where(item => !changes.Any(c => c.Action != action && ReferenceEquals(c.Item, item)))
                          .ToArray();
        }
    }
}