namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections.ObjectModel;
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
            var errors = new ErrorCollection();
            var actualEvents = errors.SubscribeObservableCollectionEvents();
            errors.Add(ErrorCollection.EmptyValidationErrors);
            CollectionAssert.IsEmpty(actualEvents);
            CollectionAssert.IsEmpty(errors);
        }

        [Test]
        public void AddWithOne()
        {
            var reference = new ObservableCollection<ValidationError>();
            var referenceEvents = reference.SubscribeObservableCollectionEvents();
            var errors = new ErrorCollection();
            var changes = errors.SubscribeErrorCollectionEvents();
            var actualEvents = errors.SubscribeObservableCollectionEvents();
            var error = Factory.CreateValidationError();
            errors.Add(Factory.CreateReadOnlyObservableCollection(error));
            reference.Add(error);
            CollectionAssert.AreEqual(reference, errors);
            CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error) }, changes, ErrorsChangedEventArgsComparer.Default);
        }

        [Test]
        public void AddWithTwo()
        {
            var errors = new ErrorCollection();
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
}