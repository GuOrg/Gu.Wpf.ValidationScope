namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    internal sealed class ErrorCollection : ReadOnlyObservableCollection<ValidationError>, IDisposable, INotifyErrorsChanged
    {
        internal static readonly ReadOnlyObservableCollection<ValidationError> EmptyValidationErrors = Validation.GetErrors(new DependencyObject());

        private readonly MergedErrorsCollection errors;

        public ErrorCollection()
            : this(new MergedErrorsCollection())
        {
        }

        private ErrorCollection(MergedErrorsCollection errors)
            : base(errors)
        {
            this.errors = errors;
            this.errors.ErrorCollection = this;
        }

        public event EventHandler<ErrorsChangedEventArgs> ErrorsChanged;

        public void Dispose()
        {
            this.errors.Dispose();
        }

        internal void Add(ReadOnlyObservableCollection<ValidationError> newErrors)
        {
            this.errors.Add(newErrors);
        }

        internal void Remove(ReadOnlyObservableCollection<ValidationError> oldErrors)
        {
            this.errors.Remove(oldErrors);
        }

        internal void Add(IErrorNode errorNode)
        {
            this.Add(errorNode?.Errors);
        }

        internal void Remove(IErrorNode errorNode)
        {
            this.Remove(errorNode?.Errors);
        }

        private void OnAddedErrors(IReadOnlyList<ValidationError> added)
        {
            this.ErrorsChanged?.Invoke(this, new ErrorsChangedEventArgs(EmptyValidationErrors, added));
        }

        private void OnRemovedErrors(IReadOnlyList<ValidationError> removed)
        {
            this.ErrorsChanged?.Invoke(this, new ErrorsChangedEventArgs(removed, EmptyValidationErrors));
        }

        private class MergedErrorsCollection : ObservableCollection<ValidationError>, IDisposable
        {
            private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
            private static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
            private static readonly NotifyCollectionChangedEventArgs NotifyCollectionResetEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

            private readonly List<ReadOnlyObservableCollection<ValidationError>> collections = new List<ReadOnlyObservableCollection<ValidationError>>();

            internal ErrorCollection ErrorCollection { get; set; }

            public void Dispose()
            {
                for (var i = this.collections.Count - 1; i >= 0; i--)
                {
                    this.Remove(this.collections[i]);
                }
            }

            internal void Add(ReadOnlyObservableCollection<ValidationError> errors)
            {
                if (errors == null)
                {
                    return;
                }

                Debug.Assert(!this.collections.Contains(errors), "!this.collections.Contains(errors)");
                if (errors == EmptyValidationErrors)
                {
                    return;
                }

                this.AddRange(errors);
                var errorCollection = errors as ErrorCollection;
                if (errorCollection != null)
                {
                    errorCollection.ErrorsChanged += this.OnErrorsChanged;
                }
                else
                {
                    CollectionChangedEventManager.AddHandler(errors, this.OnErrorsCollectionChanged);
                }

                this.collections.Add(errors);
            }

            internal void Remove(ReadOnlyObservableCollection<ValidationError> errors)
            {
                if (errors == null)
                {
                    return;
                }

                if (errors == EmptyValidationErrors)
                {
                    return;
                }

                Debug.Assert(this.collections.Contains(errors), "this.collections.Contains(errors)");
                this.RemoveRange(errors);
                var errorCollection = errors as ErrorCollection;
                if (errorCollection != null)
                {
                    errorCollection.ErrorsChanged -= this.OnErrorsChanged;
                }
                else
                {
                    CollectionChangedEventManager.RemoveHandler(errors, this.OnErrorsCollectionChanged);
                }

                this.collections.Remove(errors);
            }

            protected override void MoveItem(int oldIndex, int newIndex) => ThrowNotSupported();

            protected override void SetItem(int index, ValidationError item) => ThrowNotSupported();

            protected override void ClearItems() => ThrowNotSupported();

            private static void ThrowNotSupported([CallerMemberName] string caller = null)
            {
                throw new NotSupportedException($"{nameof(MergedErrorsCollection)} does not support {caller}");
            }

            private void AddRange(IReadOnlyList<ValidationError> errors)
            {
                if (errors.Count == 0)
                {
                    return;
                }

                if (errors.Count == 1)
                {
                    this.Add(errors[0]);
                    this.ErrorCollection.OnAddedErrors(errors);
                    return;
                }

                foreach (var error in errors)
                {
                    this.Items.Add(error);
                }

                this.RaiseReset();
                this.ErrorCollection.OnAddedErrors(errors);
            }

            private void RemoveRange(IReadOnlyList<ValidationError> errors)
            {
                if (errors.Count == 0)
                {
                    return;
                }

                if (errors.Count == 1)
                {
                    this.Remove(errors[0]);
                    this.ErrorCollection.OnRemovedErrors(errors);
                    return;
                }

                foreach (var error in errors)
                {
                    this.Items.Remove(error);
                }

                this.RaiseReset();
                this.ErrorCollection.OnRemovedErrors(errors);
            }

            private void OnErrorsChanged(object sender, ErrorsChangedEventArgs e)
            {
                this.RemoveRange(e.Removed);
                this.AddRange(e.Added);
            }

            private void OnErrorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        var newError = (ValidationError)e.NewItems[0];
                        this.Add(newError);
                        this.ErrorCollection.OnAddedErrors(new[] { newError });
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        var oldError = (ValidationError)e.OldItems[0];
                        this.Remove(oldError);
                        this.ErrorCollection.OnRemovedErrors(new[] { oldError });
                        break;
                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                        // http://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Validation.cs,507
                        throw new NotSupportedException("Only add or remove should ever happen.");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private void RaiseReset()
            {
                this.OnPropertyChanged(CountPropertyChangedEventArgs);
                this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
                this.OnCollectionChanged(NotifyCollectionResetEventArgs);
            }
        }
    }
}
