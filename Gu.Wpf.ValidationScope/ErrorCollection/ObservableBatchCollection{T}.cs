namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;

    internal class ObservableBatchCollection<T> : ObservableCollection<T>
    {
        private static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
        private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
        private BatchChange batch;

        /// <summary>
        /// Clears current items and adds <paramref name="newItems"/> notifies once when done
        /// </summary>
        /// <param name="newItems">The values that will be in the collection after</param>
        public void Refresh(IEnumerable<T> newItems)
        {
            using (this.BeginChange())
            {
                this.Clear();
                this.AddRange(newItems);
            }
        }

        public void AddRange(IEnumerable<T> newItems)
        {
            if (newItems == null)
            {
                return;
            }

            if (this.batch == null)
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

            if (this.batch == null)
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
            if (this.batch != null)
            {
                this.CheckReentrancy();
                this.batch.Add(new BatchChangeItem<T>(item, index, NotifyCollectionChangedAction.Add));
                this.Items.Insert(index, item);
            }
            else
            {
                base.InsertItem(index, item);
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (this.batch != null)
            {
                this.CheckReentrancy();
                this.batch.Add(new BatchChangeItem<T>(this.Items[index], index, NotifyCollectionChangedAction.Remove));
                this.batch.Add(new BatchChangeItem<T>(item, index, NotifyCollectionChangedAction.Add));
                this.Items[index] = item;
            }
            else
            {
                base.SetItem(index, item);
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (this.batch != null)
            {
                this.CheckReentrancy();
                var item = this.Items[oldIndex];
                this.RemoveItem(oldIndex);
                this.InsertItem(newIndex, item);
            }
            else
            {
                base.MoveItem(oldIndex, newIndex);
            }
        }

        protected override void RemoveItem(int index)
        {
            if (this.batch != null)
            {
                this.CheckReentrancy();
                this.batch.Add(new BatchChangeItem<T>(this.Items[index], index, NotifyCollectionChangedAction.Remove));
                this.Items.RemoveAt(index);
            }
            else
            {
                base.RemoveItem(index);
            }
        }

        protected override void ClearItems()
        {
            if (this.batch != null)
            {
                this.CheckReentrancy();
                for (int i = 0; i < this.Items.Count; i++)
                {
                    this.batch.Add(new BatchChangeItem<T>(this.Items[i], i, NotifyCollectionChangedAction.Remove));
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
            var changes = this.batch;
            this.batch = null;
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
                    if (Equals(c1.Item, c2.Item))
                    {
                        return null;
                    }

                    if (c1.Action == NotifyCollectionChangedAction.Remove &&
                        c2.Action == NotifyCollectionChangedAction.Add)
                    {
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, c2.Item, c1.Item, c1.Index);
                    }
                    else if (c1.Action == NotifyCollectionChangedAction.Add &&
                             c2.Action == NotifyCollectionChangedAction.Remove)
                    {
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, c1.Item, c2.Item, c1.Index);
                    }
                }

                if (Equals(c1.Item, c2.Item) &&
                    (c1.Action == NotifyCollectionChangedAction.Add &&
                     c2.Action == NotifyCollectionChangedAction.Remove) ||
                    (c1.Action == NotifyCollectionChangedAction.Remove &&
                     c2.Action == NotifyCollectionChangedAction.Add))
                {
                    var oldIndex = c1.Action == NotifyCollectionChangedAction.Remove ? c1.Index : c2.Index;
                    var newIndex = c1.Action == NotifyCollectionChangedAction.Add ? c1.Index : c2.Index;
                    return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, c1.Item, newIndex, oldIndex);
                }
            }

            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        }

        private class BatchChange : Collection<BatchChangeItem<T>>, IDisposable
        {
            private readonly ObservableBatchCollection<T> source;

            public BatchChange(ObservableBatchCollection<T> source)
            {
                if (source.batch != null)
                {
                    throw new InvalidOperationException("Cannot start a batch before the current batch is finished.");
                }

                this.source = source;
                source.batch = this;
            }

            public void Dispose()
            {
                this.source.NotifyBatch();
            }
        }
    }
}