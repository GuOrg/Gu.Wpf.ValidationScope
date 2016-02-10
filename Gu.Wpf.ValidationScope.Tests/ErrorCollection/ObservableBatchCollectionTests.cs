namespace Gu.Wpf.ValidationScope.Tests.ErrorCollection
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using Gu.Wpf.ValidationScope.Tests.Helpers;
    using NUnit.Framework;

    public partial class ObservableBatchCollectionTests
    {
        [Test]
        public void UpdatesAndNotifiesOnAdd()
        {
            var reference = new ObservableCollection<int>();
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int>();
            var actualEvents = batchCol.SubscribeAllEvents();
            reference.Add(1);
            batchCol.Add(1);
            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnBatchAdd()
        {
            var reference = new ObservableCollection<int>();
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int>();
            var actualEvents = batchCol.SubscribeAllEvents();
            reference.Add(1);
            using (batchCol.BeginChange())
            {
                batchCol.Add(1);
            }

            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnAddRange()
        {
            var batchCol = new ObservableBatchCollection<int>();
            var actualEvents = batchCol.SubscribeAllEvents();
            batchCol.AddRange(new[] { 1, 2, 3 });
            var expectedEvents = new EventArgs[]
            {
                new PropertyChangedEventArgs("Count"),
                new PropertyChangedEventArgs("Item[]"),
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
            };
            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnRemove()
        {
            var reference = new ObservableCollection<int> { 1, 2 };
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int> { 1, 2 };
            var actualEvents = batchCol.SubscribeAllEvents();
            reference.Remove(1);
            batchCol.Remove(1);
            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnBatchRemove()
        {
            var reference = new ObservableCollection<int> { 1, 2 };
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int> { 1, 2 };
            var actualEvents = batchCol.SubscribeAllEvents();
            reference.Remove(1);
            using (batchCol.BeginChange())
            {
                batchCol.Remove(1);
            }

            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnRemoveRange()
        {
            var batchCol = new ObservableBatchCollection<int> { 1, 2, 3 };
            var actualEvents = batchCol.SubscribeAllEvents();
            batchCol.RemoveRange(new[] { 1, 3 });
            var expectedEvents = new EventArgs[]
            {
                new PropertyChangedEventArgs("Count"),
                new PropertyChangedEventArgs("Item[]"),
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
            };
            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(new[] { 2 }, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnClear()
        {
            var reference = new ObservableCollection<int> { 1, 2 };
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int> { 1, 2 };
            var actualEvents = batchCol.SubscribeAllEvents();
            reference.Clear();
            batchCol.Clear();
            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnBatchClear()
        {
            var reference = new ObservableCollection<int> { 1, 2 };
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int> { 1, 2 };
            var actualEvents = batchCol.SubscribeAllEvents();
            reference.Clear();
            using (batchCol.BeginChange())
            {
                batchCol.Clear();
            }

            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnMove()
        {
            var reference = new ObservableCollection<int> { 1, 2 };
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int> { 1, 2 };
            var actualEvents = batchCol.SubscribeAllEvents();
            reference.Move(1, 0);
            batchCol.Move(1, 0);
            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnBatchMove()
        {
            var reference = new ObservableCollection<int> { 1, 2 };
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int> { 1, 2 };
            var actualEvents = batchCol.SubscribeAllEvents();
            reference.Move(1, 0);
            using (batchCol.BeginChange())
            {
                batchCol.Move(1, 0);
            }

            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnReplace()
        {
            var reference = new ObservableCollection<int> { 1, 2 };
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int> { 1, 2 };
            var actualEvents = batchCol.SubscribeAllEvents();
            reference[1] = 3;
            batchCol[1] = 3;
            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }

        [Test]
        public void UpdatesAndNotifiesOnBatchReplace()
        {
            var reference = new ObservableCollection<int> { 1, 2 };
            var expectedEvents = reference.SubscribeAllEvents();
            var batchCol = new ObservableBatchCollection<int> { 1, 2 };
            var actualEvents = batchCol.SubscribeAllEvents();
            reference[1] = 3;
            using (batchCol.BeginChange())
            {
                batchCol[1] = 3;
            }

            CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, batchCol);
        }
    }
}