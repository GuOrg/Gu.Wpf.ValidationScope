namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using NUnit.Framework;

    public class ErrorsChangedEventManagerTest
    {
        [Test]
        public void AddVanillaAndHandler()
        {
            var source = new ObservableCollection<ValidationError>();
            var vanillaChanges = new List<ErrorsChangedEventArgs>();
            var subscribeChanges = new List<ErrorsChangedEventArgs>();
            using (var errors = new ErrorCollection())
            {
                errors.ErrorsChanged += (sender, args) => vanillaChanges.Add(args);
                ErrorsChangedEventManager.AddHandler(errors, (sender, args) => subscribeChanges.Add(args));
                errors.Add(new ReadOnlyObservableCollection<ValidationError>(source));
                CollectionAssert.IsEmpty(vanillaChanges);
                CollectionAssert.IsEmpty(subscribeChanges);

                var error = Factory.CreateValidationError();
                source.Add(error);
                var expected = new[] { Factory.CreateAddedEventArgs(error) };
                CollectionAssert.AreEqual(expected, vanillaChanges, ErrorsChangedEventArgsComparer.Default);
                CollectionAssert.AreEqual(expected, subscribeChanges, ErrorsChangedEventArgsComparer.Default);
                CollectionAssert.AreEqual(vanillaChanges, subscribeChanges);
            }
        }

        [Test]
        public void AddTwoHandlersLambda()
        {
            var source = new ObservableCollection<ValidationError>();
            var subscribeChanges1 = new List<ErrorsChangedEventArgs>();
            var subscribeChanges2 = new List<ErrorsChangedEventArgs>();
            using (var errors = new ErrorCollection())
            {
                ErrorsChangedEventManager.AddHandler(errors, (sender, args) => subscribeChanges1.Add(args));
                ErrorsChangedEventManager.AddHandler(errors, (sender, args) => subscribeChanges2.Add(args));
                errors.Add(new ReadOnlyObservableCollection<ValidationError>(source));
                CollectionAssert.IsEmpty(subscribeChanges1);
                CollectionAssert.IsEmpty(subscribeChanges2);

                var error = Factory.CreateValidationError();
                source.Add(error);
                var expected = new[] { Factory.CreateAddedEventArgs(error) };
                CollectionAssert.AreEqual(expected, subscribeChanges1, ErrorsChangedEventArgsComparer.Default);
                CollectionAssert.AreEqual(expected, subscribeChanges2, ErrorsChangedEventArgsComparer.Default);
            }
        }

        [Test]
        public void AddTwoHandlersSubscriber()
        {
            var source = new ObservableCollection<ValidationError>();
            var subscribeChanges1 = new Subscriber();
            var subscribeChanges2 = new Subscriber();
            using (var errors = new ErrorCollection())
            {
                ErrorsChangedEventManager.AddHandler(errors, subscribeChanges1.Add);
                ErrorsChangedEventManager.AddHandler(errors, subscribeChanges2.Add);
                errors.Add(new ReadOnlyObservableCollection<ValidationError>(source));
                CollectionAssert.IsEmpty(subscribeChanges1);
                CollectionAssert.IsEmpty(subscribeChanges2);

                var error = Factory.CreateValidationError();
                source.Add(error);
                var expected = new[] { Factory.CreateAddedEventArgs(error) };
                CollectionAssert.AreEqual(expected, subscribeChanges1, ErrorsChangedEventArgsComparer.Default);
                CollectionAssert.AreEqual(expected, subscribeChanges2, ErrorsChangedEventArgsComparer.Default);
            }
        }


        [Test]
        public void AddSubscriberTwice()
        {
            var source = new ObservableCollection<ValidationError>();
            var subscribeChanges = new Subscriber();
            using (var errors = new ErrorCollection())
            {
                ErrorsChangedEventManager.AddHandler(errors, subscribeChanges.Add);
                ErrorsChangedEventManager.AddHandler(errors, subscribeChanges.Add);
                errors.Add(new ReadOnlyObservableCollection<ValidationError>(source));
                CollectionAssert.IsEmpty(subscribeChanges);

                var error = Factory.CreateValidationError();
                source.Add(error);
                var expected = new[] { Factory.CreateAddedEventArgs(error), Factory.CreateAddedEventArgs(error) };
                CollectionAssert.AreEqual(expected, subscribeChanges, ErrorsChangedEventArgsComparer.Default);
            }
        }

        [Test]
        public void AddNested()
        {
            var source = new ObservableCollection<ValidationError>();
            var vanillaChanges = new List<ErrorsChangedEventArgs>();
            var subscribeChanges = new List<ErrorsChangedEventArgs>();
            using (var childErrors = new ErrorCollection())
            {
                using (var parentErrors = new ErrorCollection { childErrors })
                {
                    parentErrors.ErrorsChanged += (sender, args) => vanillaChanges.Add(args);
                    ErrorsChangedEventManager.AddHandler(parentErrors, (sender, args) => subscribeChanges.Add(args));
                    childErrors.Add(new ReadOnlyObservableCollection<ValidationError>(source));
                    CollectionAssert.AreEqual(vanillaChanges, subscribeChanges);

                    var error = Factory.CreateValidationError();
                    source.Add(error);
                    CollectionAssert.AreEqual(vanillaChanges, subscribeChanges);
                }
            }
        }

        private class Subscriber : IReadOnlyList<ErrorsChangedEventArgs>
        {
            private readonly List<ErrorsChangedEventArgs> args = new List<ErrorsChangedEventArgs>();

            public int Count => this.args.Count;

            public ErrorsChangedEventArgs this[int index] => this.args[index];

            public IEnumerator<ErrorsChangedEventArgs> GetEnumerator() => this.args.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.args).GetEnumerator();

            public void Add(object sender, ErrorsChangedEventArgs e)
            {
                this.args.Add(e);
            }
        }
    }
}