// ReSharper disable PossibleMultipleEnumeration
namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    internal sealed class ErrorCollection : ReadOnlyObservableCollection<ValidationError>, INotifyErrorsChanged
    {
        internal static readonly ReadOnlyObservableCollection<ValidationError> EmptyValidationErrors = Validation.GetErrors(new DependencyObject());

        private readonly ObservableBatchCollection<ValidationError> errors;

        internal ErrorCollection()
            : this(new ObservableBatchCollection<ValidationError>())
        {
        }

        private ErrorCollection(ObservableBatchCollection<ValidationError> errors)
            : base(errors)
        {
            this.errors = errors;
        }

        public event EventHandler<ErrorsChangedEventArgs>? ErrorsChanged;

        internal void Add(IEnumerable<ValidationError> newErrors)
        {
            this.errors.AddRange(newErrors);
            this.OnAddedErrors(newErrors);
        }

        internal void Remove(IEnumerable<ValidationError> oldErrors)
        {
            this.errors.RemoveRange(oldErrors);
            this.OnRemovedErrors(oldErrors);
        }

        internal void Add(ErrorNode errorNode)
        {
            this.Add(errorNode.ErrorCollection);
        }

        internal void Remove(ErrorNode errorNode)
        {
            this.Remove(errorNode.ErrorCollection);
        }

        private void OnAddedErrors(IEnumerable<ValidationError> added)
        {
            if (added.Any())
            {
                this.ErrorsChanged?.Invoke(this, new ErrorsChangedEventArgs(EmptyValidationErrors, added));
            }
        }

        private void OnRemovedErrors(IEnumerable<ValidationError> removed)
        {
            if (removed.Any())
            {
                this.ErrorsChanged?.Invoke(this, new ErrorsChangedEventArgs(removed, EmptyValidationErrors));
            }
        }
    }
}
