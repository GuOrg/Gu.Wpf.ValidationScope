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

    internal abstract class Node : IErrorNode, INotifyErrorsChanged
    {
        private readonly Lazy<ChildCollection> children = new Lazy<ChildCollection>(() => new ChildCollection());
        private bool disposed;

        protected Node()
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

        public ReadOnlyObservableCollection<IErrorNode> Children => this.children.IsValueCreated ? this.children.Value : ChildCollection.Empty;

        public virtual bool HasErrors => this.ErrorCollection.Any();

        public ReadOnlyObservableCollection<ValidationError> Errors => this.ErrorCollection;

        int IReadOnlyCollection<ValidationError>.Count => this.Errors.Count;

        public abstract DependencyObject Source { get; }

        public Node ParentNode { get; private set; }

        internal IEnumerable<Node> AllChildren
        {
            get
            {
                if (!this.children.IsValueCreated)
                {
                    yield break;
                }

                foreach (var errorNode in this.children.Value.Cast<Node>())
                {
                    yield return errorNode;
                    foreach (var child in errorNode.AllChildren)
                    {
                        yield return child;
                    }
                }
            }
        }

        internal ErrorCollection ErrorCollection { get; } = new ErrorCollection();

        ValidationError IReadOnlyList<ValidationError>.this[int index] => this.Errors[index];

        IEnumerator<ValidationError> IEnumerable<ValidationError>.GetEnumerator() => this.Errors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.Errors.GetEnumerator();

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.Dispose(true);
        }

        internal void RemoveChild(Node childNode)
        {
            if (this.children.IsValueCreated == false)
            {
                throw new InvalidOperationException("Cannot remove child when no child is added");
            }

            var hasErrorsBefore = this.HasErrors;
            if (!this.children.Value.Remove(childNode))
            {
                throw new InvalidOperationException("Cannot remove child that was not added");
            }

            childNode.ParentNode = null;
            this.ErrorCollection.Remove(childNode);
            if (hasErrorsBefore != this.HasErrors)
            {
                this.OnPropertyChanged(nameof(this.HasErrors));
            }
        }

        internal void AddChild(Node childNode)
        {
            if (ReferenceEquals(childNode.ParentNode, this))
            {
                return;
            }

            childNode.ParentNode?.RemoveChild(childNode);
            var hasErrorsBefore = this.HasErrors;
            if (!this.children.Value.TryAdd(childNode))
            {
                return;
            }

            childNode.ParentNode = this;
            this.ErrorCollection.Add(childNode);
            if (hasErrorsBefore != this.HasErrors)
            {
                this.OnPropertyChanged(nameof(this.HasErrors));
            }
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