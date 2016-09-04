namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;

    using NUnit.Framework;

    public partial class ErrorCollectionTests
    {
        public class Nested
        {
            [Test]
            public void AddEmptyValidationErrors()
            {
                using (var childErrors = new ErrorCollection())
                {
                    var childEvents = childErrors.SubscribeObservableCollectionEvents();
                    using (var parentErrors = new ErrorCollection {childErrors})
                    {
                        var parentEvents = childErrors.SubscribeObservableCollectionEvents();

                        childErrors.Add(ErrorCollection.EmptyValidationErrors);
                        CollectionAssert.IsEmpty(childEvents);
                        CollectionAssert.IsEmpty(childErrors);
                        CollectionAssert.IsEmpty(parentEvents);
                        CollectionAssert.IsEmpty(parentErrors);
                    }
                }
            }

            [Test]
            public void AddEmptyThenAddOne()
            {
                var reference = new ObservableCollection<ValidationError>();
                var referenceEvents = reference.SubscribeObservableCollectionEvents();
                using (var childErrors = new ErrorCollection())
                {
                    var childChanges = childErrors.SubscribeErrorCollectionEvents();
                    var childEvents = childErrors.SubscribeObservableCollectionEvents();
                    using (var parentErrors = new ErrorCollection { childErrors })
                    {
                        var parentChanges = childErrors.SubscribeErrorCollectionEvents();
                        var parentEvents = childErrors.SubscribeObservableCollectionEvents();

                        childErrors.Add(new ReadOnlyObservableCollection<ValidationError>(reference));
                        CollectionAssert.IsEmpty(childEvents);
                        CollectionAssert.IsEmpty(childErrors);
                        CollectionAssert.IsEmpty(parentEvents);
                        CollectionAssert.IsEmpty(parentErrors);

                        var error = Factory.CreateValidationError();
                        reference.Add(error);
                        CollectionAssert.AreEqual(reference, childErrors);
                        CollectionAssert.AreEqual(referenceEvents, childEvents, ObservableCollectionArgsComparer.Default);
                        var expectedChanges = new[] { Factory.CreateAddedEventArgs(error) };
                        CollectionAssert.AreEqual(expectedChanges, childChanges,ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(reference, parentErrors);
                        CollectionAssert.AreEqual(referenceEvents, parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, parentChanges, ErrorsChangedEventArgsComparer.Default);
                    }
                }
            }

            [Test]
            public void AddWithOne()
            {
                var reference = new ObservableCollection<ValidationError>();
                var referenceEvents = reference.SubscribeObservableCollectionEvents();
                using (var childErrors = new ErrorCollection())
                {
                    var childChanges = childErrors.SubscribeErrorCollectionEvents();
                    var childEvents = childErrors.SubscribeObservableCollectionEvents();
                    using (var parentErrors = new ErrorCollection { childErrors })
                    {
                        var parentChanges = childErrors.SubscribeErrorCollectionEvents();
                        var parentEvents = childErrors.SubscribeObservableCollectionEvents();

                        var error = Factory.CreateValidationError();
                        childErrors.Add(Factory.CreateReadOnlyObservableCollection(error));
                        reference.Add(error);
                        CollectionAssert.AreEqual(reference, childErrors);
                        CollectionAssert.AreEqual(referenceEvents, childEvents, ObservableCollectionArgsComparer.Default);
                        var expectedChanges = new[] { Factory.CreateAddedEventArgs(error) };
                        CollectionAssert.AreEqual(expectedChanges, childChanges, ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(reference, parentErrors);
                        CollectionAssert.AreEqual(referenceEvents, parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, parentChanges, ErrorsChangedEventArgsComparer.Default);
                    }
                }
            }

            [Test]
            public void AddWithOneThenRemoveIt()
            {
                var reference = new ObservableCollection<ValidationError>();
                var referenceEvents = reference.SubscribeObservableCollectionEvents();
                using (var childErrors = new ErrorCollection())
                {
                    var childChanges = childErrors.SubscribeErrorCollectionEvents();
                    var childEvents = childErrors.SubscribeObservableCollectionEvents();

                    using (var parentErrors = new ErrorCollection { childErrors })
                    {
                        var parentChanges = childErrors.SubscribeErrorCollectionEvents();
                        var parentEvents = childErrors.SubscribeObservableCollectionEvents();
                        var error = Factory.CreateValidationError();
                        var readOnlyObservableCollection = Factory.CreateReadOnlyObservableCollection(error);
                        childErrors.Add(readOnlyObservableCollection);
                        reference.Add(error);
                        CollectionAssert.AreEqual(reference, childErrors);
                        CollectionAssert.AreEqual(referenceEvents, childEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error) }, childChanges, ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(reference, parentErrors);
                        CollectionAssert.AreEqual(referenceEvents, parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(new[] { Factory.CreateAddedEventArgs(error) }, parentChanges, ErrorsChangedEventArgsComparer.Default);

                        childErrors.Remove(readOnlyObservableCollection);
                        reference.RemoveAt(0);
                        var expectedErrors = reference.ToArray();
                        CollectionAssert.AreEqual(expectedErrors, childErrors);
                        CollectionAssert.AreEqual(referenceEvents, childEvents, ObservableCollectionArgsComparer.Default);
                        var expectedChanges = new[]
                                                  {
                                                      Factory.CreateAddedEventArgs(error),
                                                      Factory.CreateRemovedEventArgs(error)
                                                  };
                        CollectionAssert.AreEqual(expectedChanges, childChanges, ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(expectedErrors, parentErrors);
                        CollectionAssert.AreEqual(referenceEvents, parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, parentChanges, ErrorsChangedEventArgsComparer.Default);
                    }
                }
            }

            [Test]
            public void AddWithTwo()
            {
                using (var childErrors = new ErrorCollection())
                {
                    var childChanges = childErrors.SubscribeErrorCollectionEvents();
                    var childEvents = childErrors.SubscribeObservableCollectionEvents();

                    using (var parentErrors = new ErrorCollection { childErrors })
                    {
                        var parentChanges = childErrors.SubscribeErrorCollectionEvents();
                        var parentEvents = childErrors.SubscribeObservableCollectionEvents();
                        var error1 = Factory.CreateValidationError();
                        var error2 = Factory.CreateValidationError();
                        childErrors.Add(Factory.CreateReadOnlyObservableCollection(error1, error2));

                        CollectionAssert.AreEqual(new[] { error1, error2 }, childErrors);
                        CollectionAssert.AreEqual(Factory.ResetArgs(), childEvents, ObservableCollectionArgsComparer.Default);
                        var expectedChanges = new[] { Factory.CreateAddedEventArgs(error1, error2) };
                        CollectionAssert.AreEqual( expectedChanges, childChanges, ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(new[] { error1, error2 }, parentErrors);
                        CollectionAssert.AreEqual(Factory.ResetArgs(), parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, parentChanges, ErrorsChangedEventArgsComparer.Default);
                    }
                }
            }

            [Test]
            public void AddTwoWithOne()
            {
                var reference = new ObservableCollection<ValidationError>();
                var referenceEvents = reference.SubscribeObservableCollectionEvents();
                using (var childErrors = new ErrorCollection())
                {
                    var childChanges = childErrors.SubscribeErrorCollectionEvents();
                    var childEvents = childErrors.SubscribeObservableCollectionEvents();

                    using (var parentErrors = new ErrorCollection { childErrors })
                    {
                        var parentChanges = childErrors.SubscribeErrorCollectionEvents();
                        var parentEvents = childErrors.SubscribeObservableCollectionEvents();

                        var error1 = Factory.CreateValidationError();
                        childErrors.Add(Factory.CreateReadOnlyObservableCollection(error1));
                        reference.Add(error1);
                        CollectionAssert.AreEqual(reference, childErrors);
                        CollectionAssert.AreEqual(referenceEvents, childEvents, ObservableCollectionArgsComparer.Default);
                        var expectedChanges = new[] { Factory.CreateAddedEventArgs(error1) };
                        CollectionAssert.AreEqual(expectedChanges, childChanges, ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(reference, parentErrors);
                        CollectionAssert.AreEqual(referenceEvents, parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, parentChanges, ErrorsChangedEventArgsComparer.Default);

                        var error2 = Factory.CreateValidationError();
                        childErrors.Add(Factory.CreateReadOnlyObservableCollection(error2));
                        reference.Add(error2);
                        CollectionAssert.AreEqual(reference, childErrors);
                        CollectionAssert.AreEqual(referenceEvents, childEvents, ObservableCollectionArgsComparer.Default);
                        expectedChanges = new[] { Factory.CreateAddedEventArgs(error1), Factory.CreateAddedEventArgs(error2) };
                        CollectionAssert.AreEqual(expectedChanges, childChanges, ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(reference, parentErrors);
                        CollectionAssert.AreEqual(referenceEvents, parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, parentChanges, ErrorsChangedEventArgsComparer.Default);
                    }
                }
            }

            [Test]
            public void RemoveStopsSubscribing()
            {
                var reference = new ObservableCollection<ValidationError>();
                var referenceEvents = reference.SubscribeObservableCollectionEvents();
                using (var childErrors = new ErrorCollection())
                {
                    var childChanges = childErrors.SubscribeErrorCollectionEvents();
                    var childEvents = childErrors.SubscribeObservableCollectionEvents();

                    using (var parentErrors = new ErrorCollection { childErrors })
                    {
                        var parentChanges = childErrors.SubscribeErrorCollectionEvents();
                        var parentEvents = childErrors.SubscribeObservableCollectionEvents();

                        var error = Factory.CreateValidationError();
                        var inner = new ObservableCollection<ValidationError> { error };
                        var readOnlyObservableCollection = new ReadOnlyObservableCollection<ValidationError>(inner);
                        childErrors.Add(readOnlyObservableCollection);
                        reference.Add(error);
                        CollectionAssert.AreEqual(reference, childErrors);
                        CollectionAssert.AreEqual(referenceEvents, childEvents, ObservableCollectionArgsComparer.Default);
                        var expectedChanges = new[] { Factory.CreateAddedEventArgs(error) };
                        CollectionAssert.AreEqual(expectedChanges, childChanges, ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(reference, parentErrors);
                        CollectionAssert.AreEqual(referenceEvents, parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, parentChanges, ErrorsChangedEventArgsComparer.Default);

                        childErrors.Remove(readOnlyObservableCollection);
                        reference.RemoveAt(0);
                        CollectionAssert.AreEqual(reference, childErrors);
                        CollectionAssert.AreEqual(referenceEvents, childEvents, ObservableCollectionArgsComparer.Default);
                        expectedChanges = new[] { Factory.CreateAddedEventArgs(inner), Factory.CreateRemovedEventArgs(inner) };
                        CollectionAssert.AreEqual(expectedChanges, childChanges, ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(reference, parentErrors);
                        CollectionAssert.AreEqual(referenceEvents, parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, parentChanges, ErrorsChangedEventArgsComparer.Default);

                        inner.Add(error);
                        CollectionAssert.AreEqual(reference, childErrors);
                        CollectionAssert.AreEqual(referenceEvents, childEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, childChanges, ErrorsChangedEventArgsComparer.Default);

                        CollectionAssert.AreEqual(reference, parentErrors);
                        CollectionAssert.AreEqual(referenceEvents, parentEvents, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedChanges, parentChanges, ErrorsChangedEventArgsComparer.Default);
                    }
                }
            }
        }
    }
}