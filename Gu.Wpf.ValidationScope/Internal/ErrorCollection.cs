namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Controls;

    public class ErrorCollection : ObservableCollection<ValidationError>
    {
        private static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
        private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
        private static readonly IReadOnlyList<ValidationErrorChange> EmptyValidationErrorEventArgses = new ValidationErrorChange[0];

        private ChangeList batchChange;

        public IReadOnlyList<ValidationErrorChange> Update(
            ReadOnlyObservableCollection<ValidationError> oldValue,
            ReadOnlyObservableCollection<ValidationError> newValue)
        {
            return this.UpdateInternal(oldValue, newValue);
        }

        public bool CanUpdate(NotifyCollectionChangedEventArgs changes)
        {
            return changes.Action != NotifyCollectionChangedAction.Reset;
        }

        public IReadOnlyList<ValidationErrorChange> Update(NotifyCollectionChangedEventArgs changes)
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
                                   new ValidationErrorChange(old, index, ValidationErrorEventAction.Removed),
                                   new ValidationErrorChange(this[index], index, ValidationErrorEventAction.Added)
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

        private static ValidationError Single(IList col)
        {
            Ensure.Equal(col.Count, 1, nameof(col));
            return (ValidationError)col[0];
        }

        public IReadOnlyList<ValidationErrorChange> Refresh(ICollection<ValidationError> newValues)
        {
            return this.UpdateInternal(this.ToList(), newValues);
        }

        protected override void InsertItem(int index, ValidationError item)
        {
            if (this.batchChange != null)
            {
                this.batchChange.Add(new ValidationErrorChange(item, index, ValidationErrorEventAction.Added));
                this.Items.Insert(index, item);
            }
            else
            {
                base.InsertItem(index, item);
            }
        }

        protected override void SetItem(int index, ValidationError item)
        {
            if (this.batchChange != null)
            {
                this.batchChange.Add(new ValidationErrorChange(this.Items[index], index, ValidationErrorEventAction.Removed));
                this.batchChange.Add(new ValidationErrorChange(item, index, ValidationErrorEventAction.Added));
                this.Items[index] = item;
            }
            else
            {
                base.SetItem(index, item);
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (this.batchChange != null)
            {
                throw new NotSupportedException();
            }
            else
            {
                base.MoveItem(oldIndex, newIndex);
            }
        }

        protected override void RemoveItem(int index)
        {
            if (this.batchChange != null)
            {
                this.batchChange.Add(new ValidationErrorChange(this.Items[index], index, ValidationErrorEventAction.Removed));
                this.Items.RemoveAt(index);
            }
            else
            {
                base.RemoveItem(index);
            }
        }

        protected override void ClearItems()
        {
            if (this.batchChange != null)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    this.batchChange.Add(new ValidationErrorChange(this.Items[i], i, ValidationErrorEventAction.Removed));
                }

                this.Items.Clear();
            }
            else
            {
                base.ClearItems();
            }
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

        private IReadOnlyList<ValidationErrorChange> UpdateInternal(IEnumerable oldValue, IEnumerable newValue)
        {
            if (IsNullOrEmpty(oldValue) && IsNullOrEmpty(newValue))
            {
                return EmptyValidationErrorEventArgses;
            }

            var changes = new ChangeList();
            this.batchChange = changes;
            this.RemoveRange(oldValue);
            this.AddRange(newValue);

            this.batchChange = null;
            var args = changes.AsCollectionChangedEventArgs();
            if (args != null)
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Reset:
                        this.OnPropertyChanged(CountPropertyChangedEventArgs);
                        this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                        this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                this.OnCollectionChanged(args);
                return changes;
            }

            return EmptyValidationErrorEventArgses;
        }

        private void AddRange(IEnumerable newErrors)
        {
            if (newErrors == null)
            {
                return;
            }

            foreach (var error in newErrors)
            {
                this.Add((ValidationError)error);
            }
        }

        private void RemoveRange(IEnumerable oldErrors)
        {
            if (oldErrors == null)
            {
                return;
            }

            foreach (var error in oldErrors)
            {
                this.Remove((ValidationError)error);
            }
        }

        private class ChangeList : Collection<ValidationErrorChange>
        {
            public NotifyCollectionChangedEventArgs AsCollectionChangedEventArgs()
            {
                if (this.Count == 0)
                {
                    return null;
                }

                if (this.Count == 1)
                {
                    var change = this[0];
                    var action = change.Action == ValidationErrorEventAction.Added ? NotifyCollectionChangedAction.Add : NotifyCollectionChangedAction.Remove;
                    return new NotifyCollectionChangedEventArgs(action, change.Error, change.index);
                }

                if (this.Count == 2)
                {
                    var c1 = this[0];
                    var c2 = this[1];
                    if (c1.index == c2.index && c1.Action != c2.Action)
                    {
                        if (ReferenceEquals(c1.Error, c2.Error))
                        {
                            return null;
                        }

                        if (c1.Action == ValidationErrorEventAction.Removed)
                        {
                            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, c1.Error, c2.Error, c1.index);
                        }
                        else
                        {
                            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, c2.Error, c1.Error, c1.index);
                        }
                    }
                }

                return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            }
        }
    }
}
