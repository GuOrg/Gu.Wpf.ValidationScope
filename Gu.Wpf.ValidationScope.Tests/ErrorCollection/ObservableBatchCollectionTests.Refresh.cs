namespace Gu.Wpf.ValidationScope.Tests.ErrorCollection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;

    using Gu.Wpf.ValidationScope.Tests.Helpers;

    using NUnit.Framework;

    public partial class ObservableBatchCollectionTests
    {
        public class Refresh
        {
            [Test]
            public void RefreshAddSingleWhenEmpty()
            {
                var reference = new ObservableCollection<int>();
                var expectedEvents = reference.SubscribeAllEvents();
                var errors = new ObservableBatchCollection<int>();
                var allEvents = errors.SubscribeAllEvents();
                errors.Refresh(new[] { 1 });
                reference.Add(1);

                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, errors);
            }

            [Test]
            public void RefreshAddsOneWhenNotEmpty()
            {
                var reference = new ObservableCollection<int> { 1, 2 };
                var expectedEvents = reference.SubscribeAllEvents();
                var batchCol = new ObservableBatchCollection<int>(reference);
                var actualEvents = batchCol.SubscribeAllEvents();
                reference.Add(3);
                batchCol.Refresh(reference);
                CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, batchCol);
            }

            [TestCase(0)]
            [TestCase(1)]
            [TestCase(2)]
            public void RefreshInsertOne(int index)
            {
                var reference = new ObservableCollection<int> { 1, 2, 3, 4 };
                var expectedEvents = reference.SubscribeAllEvents();
                var batchCol = new ObservableBatchCollection<int>(reference);
                var actualEvents = batchCol.SubscribeAllEvents();
                reference.Insert(index, 3);
                batchCol.Refresh(reference);
                CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, batchCol);
            }

            [Test]
            public void RefreshAddMany()
            {
                var errors = new ObservableBatchCollection<int> { 1 };
                var allEvents = errors.SubscribeAllEvents();
                errors.Refresh(new[] { 2, 3, 4 });

                var expectedEvents = new List<EventArgs>
                                   {
                                       new PropertyChangedEventArgs("Count"),
                                       new PropertyChangedEventArgs("Item[]"),
                                       new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
                                   };
                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { 2, 3, 4 }, errors);
            }

            [Test]
            public void RefreshRemoveSingleEmpty()
            {
                var reference = new ObservableCollection<int> { 1 };
                var expectedEvents = reference.SubscribeAllEvents();
                var errors = new ObservableBatchCollection<int>(reference);
                var allEvents = errors.SubscribeAllEvents();
                reference.Remove(1);
                errors.Refresh(reference);

                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, errors);
            }

            [Test]
            public void RefreshRemoveSingleNull()
            {
                var reference = new ObservableCollection<int> { 1 };
                var expectedEvents = reference.SubscribeAllEvents();
                var errors = new ObservableBatchCollection<int>(reference);
                var allEvents = errors.SubscribeAllEvents();
                errors.Refresh(null);
                reference.Remove(1);

                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, errors);
            }

            [TestCase(0)]
            [TestCase(1)]
            [TestCase(2)]
            [TestCase(3)]
            public void RefreshRemoveOne(int index)
            {
                var reference = new ObservableCollection<int> { 1, 2, 3, 4 };
                var expectedEvents = reference.SubscribeAllEvents();
                var errors = new ObservableBatchCollection<int>(reference);
                var allEvents = errors.SubscribeAllEvents();
                reference.RemoveAt(index);
                errors.Refresh(reference);

                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, errors);
            }

            [TestCase(0, 0)]
            [TestCase(0, 2)]
            [TestCase(3, 2)]
            [TestCase(3, 0)]
            [TestCase(1, 1)]
            public void RefreshRemoveTwo(int removeIndex1, int removeIndex2)
            {
                var reference = new List<int> { 1, 2, 3, 4 };
                var errors = new ObservableBatchCollection<int>(reference);
                var allEvents = errors.SubscribeAllEvents();
                reference.RemoveAt(removeIndex1);
                reference.RemoveAt(removeIndex2);
                errors.Refresh(reference);

                var expectedEvents = new List<EventArgs>
                                   {
                                       new PropertyChangedEventArgs("Count"),
                                       new PropertyChangedEventArgs("Item[]"),
                                       new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
                                   };
                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, errors);
            }

            [TestCase(0)]
            [TestCase(1)]
            [TestCase(2)]
            [TestCase(3)]
            public void RefreshReplace(int index)
            {
                var reference = new ObservableCollection<int> { 1, 2, 3, 4 };
                var expectedEvents = reference.SubscribeAllEvents();
                var errors = new ObservableBatchCollection<int>(reference);
                var allEvents = errors.SubscribeAllEvents();
                reference[index] = 5;
                errors.Refresh(reference);

                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, errors);
            }

            [TestCase(0)]
            [TestCase(1)]
            [TestCase(2)]
            [TestCase(3)]
            public void RefreshRemovesOneInt(int index)
            {
                var reference = new ObservableCollection<int> { 1, 2, 3, 4 };
                var expectedEvents = reference.SubscribeAllEvents();
                var batchCol = new ObservableBatchCollection<int>(reference);
                var actualEvents = batchCol.SubscribeAllEvents();
                reference.RemoveAt(index);
                batchCol.Refresh(reference);
                CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, batchCol);
            }
        }
    }
}
