namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    internal sealed class ErrorCollection : ReadOnlyObservableCollection<ValidationError>, IDisposable
    {
        internal static readonly ErrorCollection Empty = new ErrorCollection(null);

        internal static readonly ReadOnlyObservableCollection<ValidationError> EmptyValidationErrors =
            Validation.GetErrors(new DependencyObject());

        private readonly MergedErrorsCollection errors;

        public ErrorCollection()
            : this(new MergedErrorsCollection(this))
        {
        }

        private ErrorCollection(MergedErrorsCollection errors)
            : base(errors ?? new ObservableCollection<ValidationError>())
        {
            this.errors = errors;
        }

        public event EventHandler<ErrorCollectionChangedEventArgs> ErrorsChanged;

        public void Dispose()
        {
            throw new NotImplementedException("remove all subscriptions");
            throw new NotImplementedException("clear");
        }

        internal void Add(ReadOnlyObservableCollection<ValidationError> newErrors)
        {
            this.errors.Add(newErrors);
            this.ErrorsChanged?.Invoke(this, new ErrorCollectionChangedEventArgs(EmptyValidationErrors, newErrors));
        }

        internal void Remove(ReadOnlyObservableCollection<ValidationError> oldErrors)
        {
            this.errors.Remove(oldErrors);
            this.ErrorsChanged?.Invoke(this, new ErrorCollectionChangedEventArgs(oldErrors, EmptyValidationErrors));
        }

        internal void Add(IErrorNode errorNode)
        {
            this.Add(errorNode.Errors);
        }

        internal void Remove(IErrorNode errorNode)
        {
            this.Remove(errorNode.Errors);
        }

        private class MergedErrorsCollection : ObservableCollection<ValidationError>
        {
            private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
            private static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
            private static readonly NotifyCollectionChangedEventArgs NotifyCollectionResetEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

            private readonly List<PositionedCollection> collections = new List<PositionedCollection>();
            private readonly ErrorCollection errorCollection;

            public MergedErrorsCollection(ErrorCollection errorCollection)
            {
                this.errorCollection = errorCollection;
            }

            internal void Add(ReadOnlyObservableCollection<ValidationError> newErrors)
            {
                if (newErrors == EmptyValidationErrors)
                {
                    return;
                }

                this.collections.Add(new PositionedCollection(newErrors, this.Count));
                this.AddRange(newErrors);
                CollectionChangedEventManager.AddHandler(newErrors, this.OnErrorsChanged);
            }

            internal void Remove(ReadOnlyObservableCollection<ValidationError> oldErrors)
            {
                if (oldErrors == EmptyValidationErrors)
                {
                    return;
                }

                CollectionChangedEventManager.RemoveHandler(oldErrors, this.OnErrorsChanged);
                var index = this.IndexOf(oldErrors);
                var collection = this.collections[index];
                this.RemoveRange(collection);
                for (int i = index; i < this.collections.Count; i++)
                {
                    this.collections[i].Shift(-collection.Errors.Count);
                }
            }

            private void AddRange(ReadOnlyObservableCollection<ValidationError> errors)
            {
                if (errors.Count == 0)
                {
                    return;
                }

                if (errors.Count == 1)
                {
                    this.Add(errors[0]);
                    return;
                }

                foreach (var error in errors)
                {
                    this.Items.Add(error);
                }

                this.RaiseReset();
            }

            private void RemoveRange(PositionedCollection errors)
            {
                if (errors.Errors.Count == 0)
                {
                    return;
                }

                if (errors.Errors.Count == 1)
                {
                    this.RemoveAt(errors.StartIndex);
                    return;
                }

                for (int i = 0; i < errors.EndIndex; i++)
                {
                    this.Items.RemoveAt(errors.StartIndex);
                }

                this.RaiseReset();
            }

            private int IndexOf(ReadOnlyObservableCollection<ValidationError> errors)
            {
                for (int i = 0; i < this.collections.Count; i++)
                {
                    if (ReferenceEquals(this.collections[i].Errors, errors))
                    {
                        return i;
                    }
                }

                throw new ArgumentOutOfRangeException(nameof(errors), "Could not find a match for errors");
            }

            private void OnErrorsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                var index = this.IndexOf((ReadOnlyObservableCollection<ValidationError>)sender);
                var collection = this.collections[index];

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        var newError = collection.Errors[collection.Errors.Count - 1];
                        this.InsertItem(collection.EndIndex + 1, newError);
                        this.errorCollection.ErrorsChanged.Invoke(this.errorCollection, new ErrorCollectionChangedEventArgs(EmptyValidationErrors, new[] { newError }));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        var oldError = this[collection.EndIndex];
                        this.RemoveAt(collection.EndIndex);
                        this.errorCollection.ErrorsChanged.Invoke(this.errorCollection, new ErrorCollectionChangedEventArgs(new[] { oldError }, EmptyValidationErrors));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                    case NotifyCollectionChangedAction.Reset:
                        // http://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Validation.cs,507
                        throw new NotSupportedException("Only add or remove should ever happen.");
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var shift = collection.UpdateEnd();
                if (shift != 0)
                {
                    for (int i = index; i < this.collections.Count; i++)
                    {
                        this.collections[i].Shift(shift);
                    }
                }
            }

            private void RaiseReset()
            {
                this.OnPropertyChanged(CountPropertyChangedEventArgs);
                this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
                this.OnCollectionChanged(NotifyCollectionResetEventArgs);
            }

            private class PositionedCollection
            {
                public PositionedCollection(ReadOnlyObservableCollection<ValidationError> errors, int startIndex)
                {
                    this.Errors = errors;
                    this.StartIndex = startIndex;
                    this.UpdateEnd();
                }

                internal ReadOnlyObservableCollection<ValidationError> Errors { get; }

                internal int StartIndex { get; private set; }

                internal int EndIndex { get; private set; }

                internal void Shift(int n)
                {
                    this.StartIndex += n;
                    this.EndIndex += n;
                }

                internal int UpdateEnd()
                {
                    var before = this.EndIndex;
                    this.EndIndex = this.StartIndex + this.Errors.Count - 1;
                    return before - this.EndIndex;
                }
            }
        }
    }
}
