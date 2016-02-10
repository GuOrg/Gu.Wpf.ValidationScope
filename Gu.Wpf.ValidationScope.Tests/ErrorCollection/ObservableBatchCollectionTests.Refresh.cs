namespace Gu.Wpf.ValidationScope.Tests.ErrorCollection
{
    using System;
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
            public void RefreshAddSingle()
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
                var errors = new ObservableBatchCollection<int> { 1 };
                var allEvents = errors.SubscribeAllEvents();
                errors.Refresh(new int[0]);
                reference.Remove(1);

                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, errors);
            }

            [Test]
            public void RefreshRemoveSingleNull()
            {
                var reference = new ObservableCollection<int> { 1 };
                var expectedEvents = reference.SubscribeAllEvents();
                var errors = new ObservableBatchCollection<int> { 1 };
                var allEvents = errors.SubscribeAllEvents();
                errors.Refresh(null);
                reference.Remove(1);

                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, errors);
            }

            [Test]
            public void RefreshReplace()
            {
                var reference = new ObservableCollection<int> { 1 };
                var expectedEvents = reference.SubscribeAllEvents();
                var errors = new ObservableBatchCollection<int> { 1 };
                var allEvents = errors.SubscribeAllEvents();
                errors.Refresh(new[] { 2 });
                reference[0] = 2;

                CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(reference, errors);
            }
        }
    }
}
