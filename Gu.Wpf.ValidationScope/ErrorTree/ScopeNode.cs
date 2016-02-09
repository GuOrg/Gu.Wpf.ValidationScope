namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

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

        protected internal override IReadOnlyList<ValidationError> GetAllErrors()
        {
            if (this.Source == null)
            {
                // not sure we need to protect against null here but doing it to be safe in case GC collects the binding.
                return EmptyValidationErrors;
            }

            if (this.AllChildren.Any())
            {
                var allErrors = this.AllChildren.OfType<ErrorNode>()
                                             .SelectMany(x => x.Errors)
                                             .ToList();
                return allErrors;
            }
            else
            {
                return EmptyValidationErrors;
            }
        }

        protected override void OnChildrenChanged()
        {
            this.HasErrors = this.AllChildren.Any();
        }
    }
}