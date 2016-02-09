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

    [RequiresSTA]
    public partial class ErrorCollectionTests
    {
        public class UpdateWithOldAndNew
        {
            [Test]
            public void UpdateWithNulls()
            {
                var collection = new ErrorCollection();
                var actual = collection.SubscribeAllEvents();
                var changes = collection.Update((ReadOnlyObservableCollection<ValidationError>)null, (ReadOnlyObservableCollection<ValidationError>)null);
                CollectionAssert.IsEmpty(actual);
                CollectionAssert.IsEmpty(changes);
            }

            [Test]
            public void UpdateWithEmpties()
            {
                var collection = new ErrorCollection();
                var actual = collection.SubscribeAllEvents();
                var changes = collection.Update(Create(0), Create(0));
                CollectionAssert.IsEmpty(actual);
                CollectionAssert.IsEmpty(changes);
            }

            [Test]
            public void UpdateWithNewWhenEmpty()
            {
                var collection = new ErrorCollection();
                var actual = collection.SubscribeAllEvents();
                var newCollection = Create(1);
                var changes = collection.Update(null, newCollection);
                var expectedArgs = new List<EventArgs>
                                   {
                                       new PropertyChangedEventArgs("Count"),
                                       new PropertyChangedEventArgs("Item[]"),
                                       new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newCollection[0], 0)
                                   };
                CollectionAssert.AreEqual(expectedArgs, actual, ObservableCollectionArgsComparer.Default);

                var expectedChanges = new List<BatchChangeItem<ValidationError>>
                                   {
                                       BatchChangeItem.CreateAdd(newCollection[0], 0)
                                   };
                CollectionAssert.AreEqual(expectedChanges, changes, BatchChangeItemComparer<ValidationError>.Default);
            }

            [Test]
            public void AddNewValuesAlreadyInCollection()
            {
                Assert.Inconclusive("");
                var newCollection = Create(1);
                var collection = new ErrorCollection { newCollection[0] };
                var actual = collection.SubscribeAllEvents();
                var changes = collection.Update(null, newCollection);
                CollectionAssert.IsEmpty(actual);
                CollectionAssert.IsEmpty(changes);
            }

            [Test]
            public void AddNewValuesAlreadyInOldValues()
            {
                Assert.Inconclusive("not sure if this complexity is needed");
                var errors = Create(1);
                var collection = new ErrorCollection { errors[0] };
                var actual = collection.SubscribeAllEvents();
                var changes = collection.Update(errors, errors);
                CollectionAssert.IsEmpty(actual);
                CollectionAssert.IsEmpty(changes);
            }
        }
    }
}
