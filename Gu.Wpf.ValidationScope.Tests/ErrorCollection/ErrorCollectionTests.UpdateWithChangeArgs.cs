namespace Gu.Wpf.ValidationScope.Tests.ErrorCollection
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Controls;
    using Gu.Wpf.ValidationScope.Tests.Helpers;
    using NUnit.Framework;
    using ErrorCollection = Gu.Wpf.ValidationScope.ErrorCollection;

    public partial class ErrorCollectionTests
    {
        public class UpdateWithChangeArgs
        {
            [Test]
            public void UpdatesAndNotifiesOnAdd()
            {
                var source = new ObservableCollection<ValidationError>();
                var sourceEvents = source.SubscribeAllEvents();
                var errors = new ErrorCollection();
                var errorEvents = errors.SubscribeAllEvents();
                var error = TestValidationError.Create();
                source.Add(error);
                var e = sourceEvents.OfType<NotifyCollectionChangedEventArgs>().Last();
                var changes = errors.Update(e);
                CollectionAssert.AreEqual(sourceEvents, errorEvents, ObservableCollectionArgsComparer.Default);
                var expectedChanges = new[] { BatchChangeItem.CreateAdd<ValidationError>(error, 0), };
                CollectionAssert.AreEqual(expectedChanges, changes, BatchChangeItemComparer<ValidationError>.Default);
            }

            [Test]
            public void UpdatesAndNotifiesOnRemove()
            {
                var error = TestValidationError.Create();
                var source = new ObservableCollection<ValidationError> { error };
                var sourceEvents = source.SubscribeAllEvents();
                var errors = new ErrorCollection { error };
                var errorEvents = errors.SubscribeAllEvents();
                source.Remove(error);
                var e = sourceEvents.OfType<NotifyCollectionChangedEventArgs>().Last();
                var changes = errors.Update(e);
                CollectionAssert.AreEqual(sourceEvents, errorEvents, ObservableCollectionArgsComparer.Default);
                var expectedChanges = new[] { BatchChangeItem.CreateRemove<ValidationError>(error, 0), };
                CollectionAssert.AreEqual(expectedChanges, changes, BatchChangeItemComparer<ValidationError>.Default);
            }
        }
    }
}
