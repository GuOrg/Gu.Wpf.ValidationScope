namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;

    using NUnit.Framework;

    public class ErrorCollectionTests
    {
        [Test]
        public void EmptyValidationErrors()
        {
            Assert.NotNull(ErrorCollection.EmptyValidationErrors);
            CollectionAssert.IsEmpty(ErrorCollection.EmptyValidationErrors);
        }

        [Test]
        public void AddEmptyValidationErrors()
        {
            using (var errors = new ErrorCollection())
            {
                var actualEvents = errors.SubscribeObservableCollectionEvents();
                errors.Add(ErrorCollection.EmptyValidationErrors);
                CollectionAssert.IsEmpty(actualEvents);
                CollectionAssert.IsEmpty(errors);
            }
        }

        [Test]
        public void AddEmptyThenAddOne()
        {
            var reference = new ObservableCollection<ValidationError>();
            var referenceEvents = reference.SubscribeObservableCollectionEvents();
            using (var errors = new ErrorCollection())
            {
                var changes = errors.SubscribeErrorCollectionEvents();
                var actualEvents = errors.SubscribeObservableCollectionEvents();
                errors.Add(new ReadOnlyObservableCollection<ValidationError>(reference));
                CollectionAssert.IsEmpty(actualEvents);
                CollectionAssert.IsEmpty(errors);

                var error = Factory.CreateValidationError();
                reference.Add(error);
                CollectionAssert.AreEqual(reference, errors);
                CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error) }, changes, ErrorsChangedEventArgsComparer.Default);
            }
        }

        [Test]
        public void AddWithOne()
        {
            var reference = new ObservableCollection<ValidationError>();
            var referenceEvents = reference.SubscribeObservableCollectionEvents();
            using (var errors = new ErrorCollection())
            {
                var changes = errors.SubscribeErrorCollectionEvents();
                var actualEvents = errors.SubscribeObservableCollectionEvents();
                var error = Factory.CreateValidationError();
                errors.Add(Factory.CreateReadOnlyObservableCollection(error));
                reference.Add(error);
                CollectionAssert.AreEqual(reference, errors);
                CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error) }, changes, ErrorsChangedEventArgsComparer.Default);
            }
        }

        [Test]
        public void AddWithOneThenRemoveIt()
        {
            var reference = new ObservableCollection<ValidationError>();
            var referenceEvents = reference.SubscribeObservableCollectionEvents();
            using (var errors = new ErrorCollection())
            {
                var changes = errors.SubscribeErrorCollectionEvents();
                var actualEvents = errors.SubscribeObservableCollectionEvents();
                var error = Factory.CreateValidationError();
                var readOnlyObservableCollection = Factory.CreateReadOnlyObservableCollection(error);
                errors.Add(readOnlyObservableCollection);
                reference.Add(error);
                CollectionAssert.AreEqual(reference, errors);
                CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error) }, changes, ErrorsChangedEventArgsComparer.Default);

                errors.Remove(readOnlyObservableCollection);
                reference.RemoveAt(0);
                var expectedErrors = reference.ToArray();
                CollectionAssert.AreEqual(expectedErrors, errors);
                var expectedEvents = referenceEvents.ToArray();
                CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                var expectedChanges = new[] { Factory.CreateAddedEventArgs(error), Factory.CreateRemovedEventArgs(error) };
                CollectionAssert.AreEqual(expectedChanges, changes, ErrorsChangedEventArgsComparer.Default);
            }
        }

        [Test]
        public void AddWithTwo()
        {
            using (var errors = new ErrorCollection())
            {
                var changes = errors.SubscribeErrorCollectionEvents();
                var actualEvents = errors.SubscribeObservableCollectionEvents();
                var error1 = Factory.CreateValidationError();
                var error2 = Factory.CreateValidationError();
                errors.Add(Factory.CreateReadOnlyObservableCollection(error1, error2));
                CollectionAssert.AreEqual(new[] { error1, error2 }, errors);
                CollectionAssert.AreEqual(Factory.ResetArgs(), actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error1, error2) }, changes, ErrorsChangedEventArgsComparer.Default);
            }
        }

        [Test]
        public void AddTwoWithOne()
        {
            var reference = new ObservableCollection<ValidationError>();
            var referenceEvents = reference.SubscribeObservableCollectionEvents();
            using (var errors = new ErrorCollection())
            {
                var changes = errors.SubscribeErrorCollectionEvents();
                var actualEvents = errors.SubscribeObservableCollectionEvents();
                var error1 = Factory.CreateValidationError();
                errors.Add(Factory.CreateReadOnlyObservableCollection(error1));
                reference.Add(error1);
                CollectionAssert.AreEqual(reference, errors);
                CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error1) }, changes, ErrorsChangedEventArgsComparer.Default);

                var error2 = Factory.CreateValidationError();
                errors.Add(Factory.CreateReadOnlyObservableCollection(error2));
                reference.Add(error2);
                CollectionAssert.AreEqual(reference, errors);
                CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error1), Factory.CreateAddedEventArgs(error2) }, changes, ErrorsChangedEventArgsComparer.Default);
            }
        }

        [Test]
        public void RemoveStopsSubscribing()
        {
            var reference = new ObservableCollection<ValidationError>();
            var referenceEvents = reference.SubscribeObservableCollectionEvents();
            using (var errors = new ErrorCollection())
            {
                var changes = errors.SubscribeErrorCollectionEvents();
                var actualEvents = errors.SubscribeObservableCollectionEvents();
                var error = Factory.CreateValidationError();
                var inner = new ObservableCollection<ValidationError> {error};
                var readOnlyObservableCollection = new ReadOnlyObservableCollection<ValidationError>(inner);
                errors.Add(readOnlyObservableCollection);
                reference.Add(error);
                CollectionAssert.AreEqual(reference, errors);
                CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error) }, changes, ErrorsChangedEventArgsComparer.Default);

                errors.Remove(readOnlyObservableCollection);
                reference.RemoveAt(0);
                CollectionAssert.AreEqual(reference, errors);
                CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                var expectedChanges = new[] { Factory.CreateAddedEventArgs(inner), Factory.CreateRemovedEventArgs(inner) };
                CollectionAssert.AreEqual(expectedChanges, changes, ErrorsChangedEventArgsComparer.Default);

                inner.Add(error);
                CollectionAssert.AreEqual(reference, errors);
                CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(expectedChanges, changes, ErrorsChangedEventArgsComparer.Default);
            }
        }
    }
}
