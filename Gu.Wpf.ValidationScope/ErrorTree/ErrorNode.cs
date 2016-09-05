// ReSharper disable ArrangeThisQualifier
namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using JetBrains.Annotations;

    internal abstract class ErrorNode : Node, IErrorNode, INotifyErrorsChanged
    {
        private static readonly PropertyChangedEventArgs HasErrorsPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(HasError));
        private readonly Lazy<ChildCollection> children = new Lazy<ChildCollection>(() => new ChildCollection());

        protected ErrorNode()
        {
            this.ErrorCollection.ErrorsChanged += this.OnErrorCollectionErrorsChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ErrorsChangedEventArgs> ErrorsChanged;

        public override bool HasError => this.ErrorCollection.Any();

        public override ReadOnlyObservableCollection<ValidationError> Errors => this.ErrorCollection;

        public override ReadOnlyObservableCollection<IErrorNode> Children => this.children.IsValueCreated ? this.children.Value : ChildCollection.Empty;

        public abstract DependencyObject Source { get; }

        public ErrorNode ParentNode { get; private set; }

        internal ErrorCollection ErrorCollection { get; } = new ErrorCollection();

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

        internal void AddChild(ErrorNode childNode)
        {
            if (ReferenceEquals(childNode.ParentNode, this))
            {
                throw new InvalidOperationException("Trying to add child twice, child.Parent == this.");
            }

            childNode.ParentNode?.RemoveChild(childNode);
            if (!this.children.Value.TryAdd(childNode))
            {
                throw new InvalidOperationException("Trying to add child twice.");
            }

            childNode.ParentNode = this;
            this.ErrorCollection.Add(childNode);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ParentNode?.RemoveChild(this);
                this.ErrorCollection.ErrorsChanged -= this.OnErrorCollectionErrorsChanged;
                if (this.children.IsValueCreated)
                {
                    for (var i = this.children.Value.Count - 1; i >= 0; i--)
                    {
                        var child = this.children.Value[i];
                        this.ErrorCollection.Remove(child.Errors);
                        this.children.Value.RemoveAt(i);
                    }
                }

                this.ErrorCollection.Dispose();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            this.PropertyChanged?.Invoke(this, args);
        }

        private void OnErrorCollectionErrorsChanged(object sender, ErrorsChangedEventArgs e)
        {
            if ((this.Errors.Count == 0 && e.Removed.Any()) ||
                Enumerable.SequenceEqual(this.Errors, e.Added))
            {
                this.OnPropertyChanged(HasErrorsPropertyChangedEventArgs);
            }

            this.ErrorsChanged?.Invoke(this, e);
        }

        private void RemoveChild(ErrorNode childNode)
        {
            if (this.children.IsValueCreated == false)
            {
                throw new InvalidOperationException("Cannot remove child when no child is added");
            }

            if (!this.children.Value.Remove(childNode))
            {
                throw new InvalidOperationException("Cannot remove child that was not added");
            }

            childNode.ParentNode = null;
            this.ErrorCollection.Remove(childNode);
        }
    }
}