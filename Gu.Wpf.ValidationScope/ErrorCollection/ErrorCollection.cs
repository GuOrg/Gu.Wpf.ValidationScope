namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Controls;

    internal class ErrorCollection : ObservableBatchCollection<ValidationError>
    {
        private static readonly IReadOnlyList<BatchChangeItem<ValidationError>> EmptyValidationErrorEventArgses = new BatchChangeItem<ValidationError>[0];

        public IReadOnlyList<BatchChangeItem<ValidationError>> Update(
            ReadOnlyObservableCollection<ValidationError> oldValue,
            ReadOnlyObservableCollection<ValidationError> newValue)
        {
            return this.UpdateInternal(oldValue, newValue);
        }

        public void Update(IReadOnlyList<BatchChangeItem<ValidationError>> updates)
        {
            if (updates == null || updates.Count == 0)
            {
                return;
            }

            using (this.BeginChange())
            {
                foreach (var change in updates)
                {
                    switch (change.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            this.Add(change.Item);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            this.Remove(change.Item);
                            break;
                        case NotifyCollectionChangedAction.Replace:
                        case NotifyCollectionChangedAction.Move:
                        case NotifyCollectionChangedAction.Reset:
                            throw new InvalidOperationException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public bool CanUpdate(NotifyCollectionChangedEventArgs changes)
        {
            return changes.Action != NotifyCollectionChangedAction.Reset;
        }

        public IReadOnlyList<BatchChangeItem<ValidationError>> Update(NotifyCollectionChangedEventArgs changes)
        {
            switch (changes.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    return this.UpdateInternal(null, changes.NewItems);
                case NotifyCollectionChangedAction.Remove:
                    return this.UpdateInternal(changes.OldItems, null);
                case NotifyCollectionChangedAction.Replace:
                    var index = changes.NewStartingIndex;
                    var old = this[index];
                    this[index] = Single(changes.NewItems);
                    return new[]
                    {
                        BatchChangeItem.CreateRemove(old, index),
                        BatchChangeItem.CreateAdd(this[index], index)
                    };
                case NotifyCollectionChangedAction.Move:
                    this.MoveItem(changes.OldStartingIndex, changes.NewStartingIndex);
                    return EmptyValidationErrorEventArgses;
                case NotifyCollectionChangedAction.Reset:
                    throw new NotSupportedException("Cannot update for Reset action. Use Refresh()");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IReadOnlyList<BatchChangeItem<ValidationError>> Refresh(IReadOnlyList<ValidationError> newValues)
        {
            return this.UpdateInternal(this.ToList(), newValues);
        }

        private static ValidationError Single(IList col)
        {
            Ensure.Equal(col.Count, 1, nameof(col));
            return (ValidationError)col[0];
        }

        private static bool IsNullOrEmpty(IEnumerable collection)
        {
            if (collection == null)
            {
                return true;
            }

            var col = collection as ICollection;
            if (col != null)
            {
                return col.Count == 0;
            }

            return !collection.Cast<object>()
                              .Any();
        }

        private IReadOnlyList<BatchChangeItem<ValidationError>> UpdateInternal(
            IEnumerable toRemove,
            IEnumerable toAdd)
        {
            if (IsNullOrEmpty(toRemove) && IsNullOrEmpty(toAdd))
            {
                return EmptyValidationErrorEventArgses;
            }

            using (this.BeginChange())
            {
                var changes = this.CurrentBatch;
                this.RemoveRange(toRemove);
                this.AddRange(toAdd);
                return changes;
            }
        }

        private void AddRange(IEnumerable newItems)
        {
            if (newItems == null)
            {
                return;
            }

            foreach (var newItem in newItems)
            {
                this.Add((ValidationError) newItem);
            }
        }

        private void RemoveRange(IEnumerable toRemove)
        {
            if (toRemove == null)
            {
                return;
            }

            foreach (var oldItem in toRemove)
            {
                this.Remove((ValidationError) oldItem);
            }
        }
    }
}
