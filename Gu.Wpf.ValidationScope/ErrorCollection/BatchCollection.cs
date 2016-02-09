namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Controls;

    public class BatchCollection<T> : ObservableCollection<T>
    {
        private static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
        private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));

        private BatchChanges batchChanges;

        public IReadOnlyList<BatchChangeItem<T>> CurrentBatch { get; private set; }

        public void AddRange(IEnumerable<T> newItems)
        {
            if (newItems == null)
            {
                return;
            }

            if (this.batchChanges == null)
            {
                using (this.BeginChange())
                {
                    foreach (var error in newItems)
                    {
                        this.Add(error);
                    }
                }
            }
            else
            {
                foreach (var error in newItems)
                {
                    this.Add(error);
                }
            }
        }

        public void RemoveRange(IEnumerable<T> oldItems)
        {
            if (oldItems == null)
            {
                return;
            }

            if (this.batchChanges == null)
            {
                using (this.BeginChange())
                {
                    foreach (var error in oldItems)
                    {
                        this.Remove(error);
                    }
                }
            }
            else
            {
                foreach (var error in oldItems)
                {
                    this.Remove(error);
                }
            }
        }

        public IDisposable BeginChange()
        {
            return new BatchChange(this);
        }

        protected override void InsertItem(int index, T item)
        {
            if (this.batchChanges != null)
            {
                this.CheckReentrancy();
                this.batchChanges.Add(new BatchChangeItem<T>(item, index, NotifyCollectionChangedAction.Add));
                this.Items.Insert(index, item);
            }
            else
            {
                base.InsertItem(index, item);
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (this.batchChanges != null)
            {
                this.CheckReentrancy();
                this.batchChanges.Add(new BatchChangeItem<T>(this.Items[index], index, NotifyCollectionChangedAction.Remove));
                this.batchChanges.Add(new BatchChangeItem<T>(item, index, NotifyCollectionChangedAction.Add));
                this.Items[index] = item;
            }
            else
            {
                base.SetItem(index, item);
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (this.batchChanges != null)
            {
                this.CheckReentrancy();
                var item = this.Items[oldIndex];
                this.RemoveItem(oldIndex);
                base.InsertItem(newIndex, item);
            }
            else
            {
                base.MoveItem(oldIndex, newIndex);
            }
        }

        protected override void RemoveItem(int index)
        {
            if (this.batchChanges != null)
            {
                this.CheckReentrancy();
                this.batchChanges.Add(new BatchChangeItem<T>(this.Items[index], index, NotifyCollectionChangedAction.Remove));
                this.Items.RemoveAt(index);
            }
            else
            {
                base.RemoveItem(index);
            }
        }

        protected override void ClearItems()
        {
            if (this.batchChanges != null)
            {
                this.CheckReentrancy();
                for (int i = 0; i < this.Items.Count; i++)
                {
                    this.batchChanges.Add(new BatchChangeItem<T>(this.Items[i], i, NotifyCollectionChangedAction.Remove));
                }

                this.Items.Clear();
            }
            else
            {
                base.ClearItems();
            }
        }

        protected virtual void NotifyBatch()
        {
            var changes = this.batchChanges;
            this.batchChanges = null;
            if (changes == null)
            {
                return;
            }

            var args = GetCollectionChangedEventArgs(changes);
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
            }
        }

        private static NotifyCollectionChangedEventArgs GetCollectionChangedEventArgs(IReadOnlyList<BatchChangeItem<T>> changes)
        {
            if (changes.Count == 0)
            {
                return null;
            }

            if (changes.Count == 1)
            {
                var change = changes[0];
                switch (change.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    case NotifyCollectionChangedAction.Remove:
                        return new NotifyCollectionChangedEventArgs(change.Action, change.Item, change.Index);
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                        throw new InvalidOperationException($"Should be two changes recorded for a {change.Action}");
                    case NotifyCollectionChangedAction.Reset:
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (changes.Count == 2)
            {
                var c1 = changes[0];
                var c2 = changes[1];
                if (c1.Index == c2.Index && c1.Action != c2.Action)
                {
                    if (ReferenceEquals(c1.Item, c2.Item))
                    {
                        return null;
                    }

                    if (c1.Action == NotifyCollectionChangedAction.Remove)
                    {
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, c1.Item, c2.Item, c1.Index);
                    }
                    else
                    {
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, c2.Item, c1.Item, c1.Index);
                    }
                }
            }

            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        }

        private class BatchChanges : Collection<BatchChangeItem<T>>
        {
        }

        private class BatchChange : IDisposable
        {
            private readonly BatchCollection<T> source;

            public BatchChange(BatchCollection<T> source)
            {
                Ensure.IsNull(source.batchChanges, $"{nameof(source)}.{nameof(source.CurrentBatch)}");
                this.source = source;
                source.batchChanges = new BatchChanges();
            }

            public void Dispose()
            {
                this.source.NotifyBatch();
            }
        }
    }
}