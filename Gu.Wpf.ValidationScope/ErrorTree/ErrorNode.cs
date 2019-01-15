// ReSharper disable ArrangeThisQualifier
namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>Base class for a node that has validation errors.</summary>
    public abstract class ErrorNode : Node, INotifyErrorsChanged, INotifyPropertyChanged, IDisposable
    {
        private static readonly PropertyChangedEventArgs HasErrorsPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(HasError));
        private readonly Lazy<ChildCollection> children = new Lazy<ChildCollection>(() => new ChildCollection());
        private bool disposed;

        /// <summary>Initializes a new instance of the <see cref="ErrorNode"/> class.</summary>
        protected ErrorNode()
        {
            this.ErrorCollection.ErrorsChanged += this.OnErrorsChanged;
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Notifies when <see cref="Errors"/> changes.</summary>
        public event EventHandler<ErrorsChangedEventArgs> ErrorsChanged;

        public override bool HasError => this.ErrorCollection.Any();

        public override ReadOnlyObservableCollection<ValidationError> Errors => this.ErrorCollection;

        public override ReadOnlyObservableCollection<ErrorNode> Children => this.children.IsValueCreated ? this.children.Value : ChildCollection.Empty;

        /// <summary>Gets the source element for this node.</summary>
        public abstract DependencyObject Source { get; }

        internal ErrorCollection ErrorCollection { get; } = new ErrorCollection();

        internal ChildCollection ChildCollection => this.children.Value;

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            if (disposing)
            {
                this.ErrorCollection.ErrorsChanged -= this.OnErrorsChanged;
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            this.PropertyChanged?.Invoke(this, args);
        }

        private void OnErrorsChanged(object sender, ErrorsChangedEventArgs e)
        {
            if ((this.Errors.Count == 0 && e.Removed.Any()) ||
                Enumerable.SequenceEqual(this.Errors, e.Added))
            {
                this.OnPropertyChanged(HasErrorsPropertyChangedEventArgs);
            }

            this.ErrorsChanged?.Invoke(this, e);
        }
    }
}
