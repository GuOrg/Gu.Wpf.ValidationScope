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
            using (var errors = new ErrorCollection())
            {
                var actualEvents = errors.SubscribeAllEvents();
                errors.Add(ErrorCollection.EmptyValidationErrors);
                CollectionAssert.IsEmpty(actualEvents);
                CollectionAssert.IsEmpty(errors);
            }
        }

        [Test]
        public void AddEmptyDoesNotSubscribe()
        {
            var reference = new ObservableCollection<ValidationError>();
            using (var errors = new ErrorCollection())
            {
                var actualEvents = errors.SubscribeAllEvents();
                errors.Add(new ReadOnlyObservableCollection<ValidationError>(reference));
                CollectionAssert.IsEmpty(actualEvents);
                CollectionAssert.IsEmpty(errors);

                reference.Add(TestValidationError.Create());
                CollectionAssert.IsEmpty(actualEvents);
                CollectionAssert.IsEmpty(errors);
            }
        }

        [Test]
        public void AddWithOne()
        {
            Assert.Fail();
            //var reference = new ObservableCollection<ValidationError> { TestValidationError.Create() };
            //var referenceEvents = reference.SubscribeAllEvents();
            //using (var errors = new ErrorCollection())
            //{
            //    var actualEvents = errors.SubscribeAllEvents();
            //    errors.Update(new ReadOnlyObservableCollection<ValidationError>(reference));
            //    CollectionAssert.AreEqual(reference, errors);
            //    CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);

            //    reference.Add(TestValidationError.Create());
            //    CollectionAssert.AreEqual(reference, errors);
            //    CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            //}
        }

        [Test]
        public void AddWithTwo()
        {
            Assert.Fail();
            //var reference = new ObservableCollection<ValidationError> { TestValidationError.Create() };
            //var referenceEvents = reference.SubscribeAllEvents();
            //using (var errors = new ErrorCollection())
            //{
            //    var actualEvents = errors.SubscribeAllEvents();
            //    errors.Update(new ReadOnlyObservableCollection<ValidationError>(reference));
            //    CollectionAssert.AreEqual(reference, errors);
            //    CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);

            //    reference.Add(TestValidationError.Create());
            //    CollectionAssert.AreEqual(reference, errors);
            //    CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            //}
        }

        [Test]
        public void ThenSet()
        {
            Assert.Fail();
            //var reference = new ObservableCollection<ValidationError> { TestValidationError.Create() };
            //var referenceEvents = reference.SubscribeAllEvents();
            //using (var errors = new ErrorCollection())
            //{
            //    var actualEvents = errors.SubscribeAllEvents();
            //    errors.Update(null, new ReadOnlyObservableCollection<ValidationError>(reference));
            //    CollectionAssert.AreEqual(reference, errors);
            //    CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);

            //    reference[0] = TestValidationError.Create();
            //    CollectionAssert.AreEqual(reference, errors);
            //    CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            //}
        }

        [Test]
        public void ThenRemove()
        {
            Assert.Fail();
            //var reference = new ObservableCollection<ValidationError> { TestValidationError.Create(), TestValidationError.Create() };
            //var referenceEvents = reference.SubscribeAllEvents();
            //using (var errors = new ErrorCollection())
            //{
            //    var actualEvents = errors.SubscribeAllEvents();
            //    errors.Update(null, new ReadOnlyObservableCollection<ValidationError>(reference));
            //    CollectionAssert.AreEqual(reference, errors);
            //    CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);

            //    reference.RemoveAt(0);
            //    CollectionAssert.AreEqual(reference, errors);
            //    CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            //}
        }

        [Test]
        public void WithEmpty()
        {
            Assert.Fail();
            //var reference1 = new ObservableCollection<ValidationError> { TestValidationError.Create() };
            //var referenceEvents = reference1.SubscribeAllEvents();
            //var errors = new ErrorCollection();
            //var actualEvents = errors.SubscribeAllEvents();
            //errors.Update(null, new ReadOnlyObservableCollection<ValidationError>(reference1));
            //CollectionAssert.AreEqual(reference1, errors);
            //CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);

            //reference1.Add(TestValidationError.Create());
            //CollectionAssert.AreEqual(reference1, errors);
            //CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
        }

        [Test]
        public void WithOther()
        {
            Assert.Fail();
            //var reference1 = new ObservableCollection<ValidationError> { TestValidationError.Create() };
            //var referenceEvents = reference1.SubscribeAllEvents();
            //var errors = new ErrorCollection();
            //var actualEvents = errors.SubscribeAllEvents();
            //errors.Update(null, new ReadOnlyObservableCollection<ValidationError>(reference1));
            //CollectionAssert.AreEqual(reference1, errors);
            //CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);

            //reference1.Add(TestValidationError.Create());
            //CollectionAssert.AreEqual(reference1, errors);
            //CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
        }

        [Test]
        public void ThenDispose()
        {
            Assert.Fail();
            //var reference1 = new ObservableCollection<ValidationError> { TestValidationError.Create() };
            //var referenceEvents = reference1.SubscribeAllEvents();
            //var errors = new ErrorCollection();
            //var actualEvents = errors.SubscribeAllEvents();
            //errors.Update(null, new ReadOnlyObservableCollection<ValidationError>(reference1));
            //CollectionAssert.AreEqual(reference1, errors);
            //CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);

            //reference1.Add(TestValidationError.Create());
            //CollectionAssert.AreEqual(reference1, errors);
            //CollectionAssert.AreEqual(referenceEvents, actualEvents, ObservableCollectionArgsComparer.Default);
        }

        [Test]
        public void RefreshAddSingleWhenEmpty()
        {
            Assert.Fail();
            //var reference = new ObservableCollection<ValidationError>();
            //var expectedEvents = reference.SubscribeAllEvents();
            //var errors = new ErrorCollection();
            //var allEvents = errors.SubscribeAllEvents();
            //var newErrors = Create(1);
            //errors.Refresh(newErrors);
            //reference.Add(newErrors[0]);

            //CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
            //CollectionAssert.AreEqual(reference, errors);
        }

        [Test]
        public void RefreshAddSingleWhenNotEmpty()
        {
            Assert.Fail();
            //var reference = new ObservableCollection<ValidationError>(Create(2));
            //var expectedEvents = reference.SubscribeAllEvents();
            //var errors = new ErrorCollection(reference);
            //var allEvents = errors.SubscribeAllEvents();
            //reference.Add(TestValidationError.Create());
            //errors.Refresh(reference);

            //CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
            //CollectionAssert.AreEqual(reference, errors);
        }

        [Test]
        public void RefreshAddMany()
        {
            Assert.Fail();
            //var error = TestValidationError.Create();
            //var errors = new ErrorCollection { error };
            //var actualEvents = errors.SubscribeAllEvents();
            //var newErrors = Create(3);
            //errors.Refresh(newErrors);
            //var changes = new List<BatchChangeItem<ValidationError>> { BatchChangeItem<ValidationError>.CreateRemove(error, 0) };
            //changes.AddRange(newErrors.Select(BatchChangeItem<ValidationError>.CreateAdd));
            //var expectedEvents = new List<EventArgs>
            //                       {
            //                           new PropertyChangedEventArgs("Count"),
            //                           new PropertyChangedEventArgs("Item[]"),
            //                           new ErrorCollectionResetEventArgs(changes)
            //                       };
            //CollectionAssert.AreEqual(expectedEvents, actualEvents, ObservableCollectionArgsComparer.Default);
            //CollectionAssert.AreEqual(newErrors, errors);
        }

        [Test]
        public void RefreshRemoveSingleEmpty()
        {
            Assert.Fail();
            //var error = TestValidationError.Create();
            //var reference = new ObservableCollection<ValidationError> { error };
            //var expectedEvents = reference.SubscribeAllEvents();
            //var errors = new ErrorCollection { error };
            //var allEvents = errors.SubscribeAllEvents();
            //errors.Refresh(new ValidationError[0]);
            //reference.Remove(error);

            //CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
            //CollectionAssert.AreEqual(reference, errors);
        }

        [Test]
        public void RefreshRemoveSingleNull()
        {
            Assert.Fail();
            //var error = TestValidationError.Create();
            //var reference = new ObservableCollection<ValidationError> { error };
            //var expectedEvents = reference.SubscribeAllEvents();
            //var errors = new ErrorCollection { error };
            //var allEvents = errors.SubscribeAllEvents();
            //errors.Refresh(null);
            //reference.Remove(error);

            //CollectionAssert.AreEqual(expectedEvents, allEvents, ObservableCollectionArgsComparer.Default);
            //CollectionAssert.AreEqual(reference, errors);
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
