namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    internal class ObservableBatchCollection<T> : ObservableCollection<T>
    {
        protected static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
        protected static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));

        private readonly BatchChanges batch;

        public ObservableBatchCollection()
        {
            this.batch = new BatchChanges(this);
        }

        public ObservableBatchCollection(IEnumerable<T> items)
            : base(items)
        {
            this.batch = new BatchChanges(this);
        }

        protected IReadOnlyList<BatchChangeItem<T>> Changes => this.batch;

        /// <summary>
        /// Clears current items and adds <paramref name="newItems"/> notifies once when done
        /// </summary>
        /// <param name="newItems">The values that will be in the collection after.</param>
        public void Refresh(IEnumerable<T> newItems)
        {
            newItems = newItems ?? Enumerable.Empty<T>();
            using (this.BeginChange())
            {
                using (var enumerator = newItems.GetEnumerator())
                {
                    var index = 0;
                    var addCurrent = false;

                    while (index < this.Count && enumerator.MoveNext())
                    {
                        if (Equals(this.Items[index], enumerator.Current))
                        {
                            index++;
                        }
                        else
                        {
                            addCurrent = true;
                            break;
                        }
                    }

                    for (var i = this.Count - 1; i >= index; i--)
                    {
                        this.batch.Add(BatchChangeItem.CreateRemove(this.Items[i], i));
                        this.Items.RemoveAt(i);
                    }

                    if (addCurrent)
                    {
                        this.batch.Add(BatchChangeItem.CreateAdd(enumerator.Current, index));
                        this.Items.Add(enumerator.Current);
                        index++;
                    }

                    while (enumerator.MoveNext())
                    {
                        this.batch.Add(BatchChangeItem.CreateAdd(enumerator.Current, index));
                        this.Items.Add(enumerator.Current);
                        index++;
                    }
                }
            }
        }

        private static bool Equals(T first, T other)
        {
            if (typeof(T).IsValueType)
            {
                return first.Equals(other);
            }

            return ReferenceEquals(first, other);
        }

        public void AddRange(IEnumerable<T> newItems)
        {
            if (newItems == null)
            {
                return;
            }

            if (this.batch.IsProcessing)
            {
                foreach (var error in newItems)
                {
                    this.Add(error);
                }
            }
            else
            {
                using (this.BeginChange())
                {
                    foreach (var error in newItems)
                    {
                        this.Add(error);
                    }
                }
            }
        }

        public void RemoveRange(IEnumerable<T> oldItems)
        {
            if (oldItems == null)
            {
                return;
            }

            if (this.batch.IsProcessing)
            {
                foreach (var error in oldItems)
                {
                    this.Remove(error);
                }
            }
            else
            {
                using (this.BeginChange())
                {
                    foreach (var error in oldItems)
                    {
                        this.Remove(error);
                    }
                }
            }
        }

        public IDisposable BeginChange()
        {
            return this.batch.Start();
        }

        protected override void InsertItem(int index, T item)
        {
            if (this.batch.IsProcessing)
            {
                this.CheckReentrancy();
                this.batch.Add(BatchChangeItem.CreateAdd(item, index));
                this.Items.Insert(index, item);
            }
            else
            {
                base.InsertItem(index, item);
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (this.batch.IsProcessing)
            {
                this.CheckReentrancy();
                this.batch.Add(BatchChangeItem.CreateRemove(this.Items[index], index));
                this.batch.Add(BatchChangeItem.CreateAdd(item, index));
                this.Items[index] = item;
            }
            else
            {
                base.SetItem(index, item);
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (this.batch.IsProcessing)
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
            if (this.batch.IsProcessing)
            {
                this.CheckReentrancy();
                this.batch.Add(BatchChangeItem.CreateRemove(this.Items[index], index));
                this.Items.RemoveAt(index);
            }
            else
            {
                base.RemoveItem(index);
            }
        }

        protected override void ClearItems()
        {
            if (this.batch.IsProcessing)
            {
                this.CheckReentrancy();
                for (var i = 0; i < this.Items.Count; i++)
                {
                    this.batch.Add(BatchChangeItem.CreateRemove(this.Items[i], i));
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
            var args = this.GetMergedCollectionChangedEventArgs();
            if (args == null)
            {
                return;
            }

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

        protected NotifyCollectionChangedEventArgs GetMergedCollectionChangedEventArgs()
        {
            if (this.batch.Count == 0)
            {
                return null;
            }

            if (this.batch.Count == 1)
            {
                var change = this.batch[0];
                return new NotifyCollectionChangedEventArgs(change.Action.AsCollectionChangedAction(), change.Item, change.Index);
            }

            if (this.batch.Count == 2)
            {
                var c1 = this.batch[0];
                var c2 = this.batch[1];
                if (c1.Index == c2.Index && c1.Action != c2.Action)
                {
                    if (Equals(c1.Item, c2.Item))
                    {
                        return null;
                    }

                    if (IsAddAndRemove(c1.Action, c2.Action))
                    {
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, c1.Item, c2.Item, c1.Index);
                    }

                    if (IsAddAndRemove(c2.Action, c1.Action))
                    {
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, c2.Item, c1.Item, c1.Index);
                    }
                }

                if (Equals(c1.Item, c2.Item) &&
                    (IsAddAndRemove(c1.Action, c2.Action) || IsAddAndRemove(c2.Action, c1.Action)))
                {
                    var oldIndex = c1.Action == BatchItemChangeAction.Remove ? c1.Index : c2.Index;
                    var newIndex = c1.Action == BatchItemChangeAction.Add ? c1.Index : c2.Index;
                    return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, c1.Item, newIndex, oldIndex);
                }
            }

            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        }

        protected IEnumerable<NotifyCollectionChangedEventArgs> GetCollectionChangedEventArgs()
        {
            foreach (var change in this.batch)
            {
                switch (change.Action)
                {
                    case BatchItemChangeAction.Add:
                    case BatchItemChangeAction.Remove:
                        yield return new NotifyCollectionChangedEventArgs(change.Action.AsCollectionChangedAction(), change.Item, change.Index);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static bool IsAddAndRemove(BatchItemChangeAction first, BatchItemChangeAction other)
        {
            return first == BatchItemChangeAction.Add && other == BatchItemChangeAction.Remove;
        }

        private class BatchChanges : Collection<BatchChangeItem<T>>, IDisposable
        {
            private readonly ObservableBatchCollection<T> source;
            private readonly object gate = new object();

            public BatchChanges(ObservableBatchCollection<T> source)
            {
                this.source = source;
            }

            public bool IsProcessing { get; private set; }

            void IDisposable.Dispose()
            {
                lock (this.gate)
                {
                    if (!this.IsProcessing)
                    {
                        throw new ObjectDisposedException("Cannot end a batch twice.");
                    }

                    this.source.NotifyBatch();
                    this.Clear();
                    this.IsProcessing = false;
                }
            }

            public IDisposable Start()
            {
                lock (this.gate)
                {
                    if (this.IsProcessing)
                    {
                        throw new InvalidOperationException("Cannot start a batch before the current batch is finished.");
                    }

                    this.IsProcessing = true;
                    return this;
                }
            }
        }
    }
}