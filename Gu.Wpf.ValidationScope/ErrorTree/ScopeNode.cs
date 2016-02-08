namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    public sealed class ScopeNode : Node
    {
        private readonly WeakReference<DependencyObject> sourceReference;

        public ScopeNode(DependencyObject source, IErrorNode child)
            : base(child)
        {
            this.sourceReference = new WeakReference<DependencyObject>(source);
            Scope.SetErrors(source, this);
            this.OnHasErrorsChanged();
        }

        public override DependencyObject Source
        {
            get
            {
                DependencyObject source;
                this.sourceReference.TryGetTarget(out source);
                return source;
            }
        }

        protected internal override void RemoveChild(IErrorNode errorNode)
        {
            this.EditableChildren.Remove(errorNode);
            this.HasErrors = this.EditableChildren.Count > 0;
            this.OnChildrenChanged();
        }

        protected override void OnChildrenChanged()
        {
            this.UpdateErrors();
        }

        protected override void OnHasErrorsChanged()
        {
            var source = this.Source;
            if (source == null)
            {
                return;
            }

            Scope.SetHasErrors(source, this.HasErrors);
            var parent = VisualTreeHelper.GetParent(source);
            var parentNode = parent == null ? null : (Node)Scope.GetErrors(parent);
            if (!this.HasErrors)
            {
                Scope.SetErrors(source, null);
                parentNode?.RemoveChild(this);
            }
            else if (parent != null)
            {
                if (parentNode == null)
                {
                    Scope.SetErrors(parent, new ScopeNode(parent, this));
                }
                else
                {
                    parentNode.AddChild(this);
                }
            }
        }

        protected override void UpdateErrors()
        {
            if (this.LazyErrors.IsValueCreated)
            {
                var validationErrors = this.LazyErrors.Value;
                validationErrors.Clear();

                foreach (var child in this.Children.OfType<ErrorNode>())
                {
                    foreach (var childError in child.Errors)
                    {
                        validationErrors.Add(childError);
                    }
                }
            }
        }
    }
}