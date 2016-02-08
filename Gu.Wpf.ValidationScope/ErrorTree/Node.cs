namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using JetBrains.Annotations;

    public abstract class Node : IErrorNode
    {
        private readonly Lazy<ObservableCollection<IErrorNode>> innerChildren = new Lazy<ObservableCollection<IErrorNode>>(() => new ObservableCollection<IErrorNode>());
        private ReadOnlyObservableCollection<IErrorNode> children;
        private ReadOnlyObservableCollection<ValidationError> errors;
        private bool hasErrors;

        protected Node(bool hasErrors)
        {
            this.hasErrors = hasErrors;
        }

        protected Node(IErrorNode child)
        {
            this.EditableChildren.Add(child);
            this.hasErrors = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyObservableCollection<IErrorNode> Children => this.children ?? (this.children = new ReadOnlyObservableCollection<IErrorNode>(this.EditableChildren));

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

        protected ObservableCollection<IErrorNode> EditableChildren => this.innerChildren.Value;

        protected internal abstract void RemoveChild(IErrorNode errorNode);

        protected internal virtual void AddChild(IErrorNode errorNode)
        {
            if (this.EditableChildren.Contains(errorNode))
            {
                return;
            }

            this.EditableChildren.Add(errorNode);
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