namespace Gu.Wpf.ValidationScope.Tests.Internal
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Controls;

    using NUnit.Framework;

    public partial class ErrorCollectionTests
    {
        public class UpdateWithChangeArgs
        {
            [Test]
            public void UpdatesAndNotifiesOnAdd()
            {
                var source = new ObservableCollection<ValidationError>();
                var sourceEvents = SubscribeAllEvents(source);
                var errors = new ErrorCollection();
                var errorEvents = SubscribeAllEvents(errors);
                var error = TestValidationError.Create();
                source.Add(error);
                var changes = errors.Update(sourceEvents.OfType<NotifyCollectionChangedEventArgs>().Last());
                CollectionAssert.AreEqual(sourceEvents, errorEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { new ValidationErrorChange(error, 0, ValidationErrorEventAction.Added), }, changes, ValidationErrorChangeComparer.Default);
            }

            [Test]
            public void UpdatesAndNotifiesOnRemove()
            {
                var error = TestValidationError.Create();
                var source = new ObservableCollection<ValidationError> { error };
                var sourceEvents = SubscribeAllEvents(source);
                var errors = new ErrorCollection { error };
                var errorEvents = SubscribeAllEvents(errors);
                source.Remove(error);
                var changes = errors.Update(sourceEvents.OfType<NotifyCollectionChangedEventArgs>().Last());
                CollectionAssert.AreEqual(sourceEvents, errorEvents, ObservableCollectionArgsComparer.Default);
                CollectionAssert.AreEqual(new[] { new ValidationErrorChange(error, 0, ValidationErrorEventAction.Removed), }, changes, ValidationErrorChangeComparer.Default);
            }
        }
    }
}
