// ReSharper disable ArrangeThisQualifier
namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using JetBrains.Annotations;

    internal abstract class ErrorNode : Node, IErrorNode, INotifyErrorsChanged
    {
        private static readonly PropertyChangedEventArgs HasErrorsPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(HasErrors));
        private readonly Lazy<ChildCollection> children = new Lazy<ChildCollection>(() => new ChildCollection());

        protected ErrorNode()
        {
            this.ErrorCollection.ErrorsChanged += this.OnErrorCollectionErrorsChanged;
            ((INotifyCollectionChanged)this.ErrorCollection).CollectionChanged += this.OnErrorsCollectionChanged;
            ((INotifyPropertyChanged)this.ErrorCollection).PropertyChanged += this.OnErrorsPropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ErrorsChangedEventArgs> ErrorsChanged;

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                this.CollectionChanged += value;
            }
            remove
            {
                this.CollectionChanged -= value;
            }
        }

        [field: NonSerialized]
        private event NotifyCollectionChangedEventHandler CollectionChanged;

        public override bool HasErrors => this.ErrorCollection.Any();

        public override ReadOnlyObservableCollection<ValidationError> Errors => this.ErrorCollection;

        public override ReadOnlyObservableCollection<IErrorNode> Children => this.children.IsValueCreated ? this.children.Value : ChildCollection.Empty;

        int IReadOnlyCollection<ValidationError>.Count => this.Errors.Count;

        public abstract DependencyObject Source { get; }

        public ErrorNode ParentNode { get; private set; }

        internal ErrorCollection ErrorCollection { get; } = new ErrorCollection();

        protected bool Disposed { get; private set; }

        ValidationError IReadOnlyList<ValidationError>.this[int index] => this.Errors[index];

        IEnumerator<ValidationError> IEnumerable<ValidationError>.GetEnumerator() => this.Errors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.Errors.GetEnumerator();

        public void Dispose()
        {
            if (this.Disposed)
            {
                return;
            }

            this.Disposed = true;
            this.Dispose(true);
        }

        internal void RemoveChild(ErrorNode childNode)
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

        internal void AddChild(ErrorNode childNode)
        {
            if (ReferenceEquals(childNode.ParentNode, this))
            {
                return;
            }

            childNode.ParentNode?.RemoveChild(childNode);
            if (!this.children.Value.TryAdd(childNode))
            {
                return;
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
                ((INotifyCollectionChanged)this.ErrorCollection).CollectionChanged -= this.OnErrorsCollectionChanged;
                ((INotifyPropertyChanged)this.ErrorCollection).PropertyChanged -= this.OnErrorsPropertyChanged;
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

        private void OnErrorsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e);
        }

        private void OnErrorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }
    }
}