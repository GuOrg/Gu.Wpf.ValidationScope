namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Linq;
    using System.Windows;

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

        protected override void OnChildrenChanged()
        {
            this.HasErrors = this.AllChildren.Any();
        }
    }
}