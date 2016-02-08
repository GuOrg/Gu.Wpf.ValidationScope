namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using JetBrains.Annotations;

    public abstract class Node : IErrorNode
    {
        private readonly Lazy<ObservableCollection<IErrorNode>> lazyChildren = new Lazy<ObservableCollection<IErrorNode>>(() => new ObservableCollection<IErrorNode>());
        private ReadOnlyObservableCollection<IErrorNode> children;
        private ReadOnlyObservableCollection<ValidationError> errors;
        private bool hasErrors;

        protected Node(bool hasErrors)
        {
            this.hasErrors = hasErrors;
        }

        protected Node(IErrorNode child)
        {
            this.lazyChildren.Value.Add(child);
            this.hasErrors = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyObservableCollection<IErrorNode> Children => this.children ?? (this.children = new ReadOnlyObservableCollection<IErrorNode>(this.lazyChildren.Value));

        public virtual bool HasErrors
        {
            get
            {
                return this.hasErrors;
            }

            protected set
            {
                if (value == this.hasErrors)
                {
                    return;
                }

                this.hasErrors = value;
                this.OnPropertyChanged();
                this.OnHasErrorsChanged();
            }
        }

        public ReadOnlyObservableCollection<ValidationError> Errors
        {
            get
            {
                if (this.errors == null)
                {
                    this.errors = new ReadOnlyObservableCollection<ValidationError>(this.LazyErrors.Value);
                    this.UpdateErrors();
                }

                return this.errors;
            }
        }

        public abstract DependencyObject Source { get; }

        protected Lazy<ObservableCollection<ValidationError>> LazyErrors { get; } = new Lazy<ObservableCollection<ValidationError>>(() => new ObservableCollection<ValidationError>());

        protected IEnumerable<Node> AllChildren
        {
            get
            {
                if (!this.lazyChildren.IsValueCreated)
                {
                    yield break;
                }

                foreach (Node errorNode in this.lazyChildren.Value)
                {
                    yield return errorNode;
                    foreach (var child in errorNode.AllChildren)
                    {
                        yield return child;
                    }
                }
            }
        }

        internal void RemoveChild(IErrorNode errorNode)
        {
            if (this.lazyChildren.IsValueCreated == false)
            {
                return;
            }

            if (this.lazyChildren.Value.Remove(errorNode))
            {
                this.OnChildrenChanged();
            }
        }

        internal void AddChild(IErrorNode errorNode)
        {
            var editableChildren = this.lazyChildren.Value;
            if (editableChildren.Contains(errorNode))
            {
                return;
            }

            editableChildren.Add(errorNode);
            this.HasErrors = true;
            this.OnChildrenChanged();
        }

        protected abstract void OnChildrenChanged();

        protected abstract void OnHasErrorsChanged();

        protected abstract void UpdateErrors();

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}