namespace Gu.Wpf.ValidationScope.Tests.ErrorCollection
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Controls;

    using Gu.Wpf.ValidationScope.Tests.Helpers;

    using NUnit.Framework;
    using ErrorCollection = Gu.Wpf.ValidationScope.ErrorCollection;

    public class ErrorCollectionTests
    {
        [Test]
        public void RefreshAddSingleWhenEmpty()
        {
            var reference = new ObservableCollection<ValidationError>();
            var expectedEvents = reference.SubscribeAllEvents();
            var errors = new ErrorCollection();
            var allEvents = errors.SubscribeAllEvents();
            var newErrors = Create(1);
            errors.Refresh(newErrors);
            reference.Add(newErrors[0]);

            CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, errors);
        }

        [Test]
        public void RefreshAddSingleWhenNotEmpty()
        {
            var reference = new ObservableCollection<ValidationError>(Create(2));
            var expectedEvents = reference.SubscribeAllEvents();
            var errors = new ErrorCollection(reference);
            var allEvents = errors.SubscribeAllEvents();
            reference.Add(TestValidationError.Create());
            errors.Refresh(reference);

            CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, errors);
        }

        [Test]
        public void RefreshAddMany()
        {
            var error = TestValidationError.Create();
            var errors = new ErrorCollection { error };
            var allEvents = errors.SubscribeAllEvents();
            var newErrors = Create(3);
            errors.Refresh(newErrors);

            var expectedEvents = new List<EventArgs>
                                   {
                                       new PropertyChangedEventArgs("Count"),
                                       new PropertyChangedEventArgs("Item[]"),
                                       new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
                                   };
            CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(newErrors, errors);
        }

        [Test]
        public void RefreshRemoveSingleEmpty()
        {
            var error = TestValidationError.Create();
            var reference = new ObservableCollection<ValidationError> { error };
            var expectedEvents = reference.SubscribeAllEvents();
            var errors = new ErrorCollection { error };
            var allEvents = errors.SubscribeAllEvents();
            errors.Refresh(new ValidationError[0]);
            reference.Remove(error);

            CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, errors);
        }

        [Test]
        public void RefreshRemoveSingleNull()
        {
            var error = TestValidationError.Create();
            var reference = new ObservableCollection<ValidationError> { error };
            var expectedEvents = reference.SubscribeAllEvents();
            var errors = new ErrorCollection { error };
            var allEvents = errors.SubscribeAllEvents();
            errors.Refresh(null);
            reference.Remove(error);

            CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(reference, errors);
        }

        private static ReadOnlyObservableCollection<ValidationError> Create(int n)
        {
            var errors = new ObservableCollection<ValidationError>();
            for (int i = 0; i < n; i++)
            {
                errors.Add(TestValidationError.Create());
            }

            return new ReadOnlyObservableCollection<ValidationError>(errors);
        }
    }
}
