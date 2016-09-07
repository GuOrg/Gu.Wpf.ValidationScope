// ReSharper disable ArrangeThisQualifier
namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public abstract class ErrorNode : Node, INotifyErrorsChanged, INotifyPropertyChanged
    {
        private static readonly PropertyChangedEventArgs HasErrorsPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(HasError));
        private readonly Lazy<ChildCollection> children = new Lazy<ChildCollection>(() => new ChildCollection());

        protected ErrorNode()
        {
            this.ErrorCollection.ErrorsChanged += this.OnErrorsChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ErrorsChangedEventArgs> ErrorsChanged;

        public override bool HasError => this.ErrorCollection.Any();

        public override ReadOnlyObservableCollection<ValidationError> Errors => this.ErrorCollection;

        public override ReadOnlyObservableCollection<ErrorNode> Children => this.children.IsValueCreated ? this.children.Value : ChildCollection.Empty;

        public abstract DependencyObject Source { get; }

        internal ErrorCollection ErrorCollection { get; } = new ErrorCollection();

        internal ChildCollection ChildCollection => this.children.Value;

        protected bool Disposed { get; private set; }

        public void Dispose()
        {
            if (this.Disposed)
            {
                return;
            }

            this.Disposed = true;
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.ErrorCollection.ErrorsChanged -= this.OnErrorsChanged;
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