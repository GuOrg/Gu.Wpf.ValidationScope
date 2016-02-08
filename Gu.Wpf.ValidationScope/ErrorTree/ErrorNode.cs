namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    public sealed class ErrorNode : Node, IDisposable
    {
        private static readonly DependencyProperty ErrorCountProperty = DependencyProperty.RegisterAttached(
            "ErrorCount",
            typeof(int),
            typeof(Scope),
            new PropertyMetadata(
                default(int),
                OnErrorCountChanged));

        private static readonly PropertyPath ErrorCountPropertyPath = new PropertyPath("(Validation.Errors).Count");
        private readonly BindingExpression errorCountExpression;

        private ErrorNode(BindingExpression errorCountExpression)
            : base(Validation.GetHasError((DependencyObject)errorCountExpression.ParentBinding.Source))
        {
            this.errorCountExpression = errorCountExpression;
            this.OnHasErrorsChanged();
        }

        public override DependencyObject Source => (DependencyObject)this.errorCountExpression?.ParentBinding.Source;

        public static ErrorNode CreateFor(DependencyObject dependencyObject)
        {
            var uiElement = dependencyObject as UIElement;
            if (uiElement != null)
            {
                var bindingExpression = uiElement.Bind(ErrorCountProperty)
                                        .OneWayTo(uiElement, ErrorCountPropertyPath);
                return new ErrorNode(bindingExpression);
            }

            var contentElement = dependencyObject as ContentElement;
            if (contentElement != null)
            {
                var bindingExpression = contentElement.Bind(ErrorCountProperty)
                                        .OneWayTo(contentElement, ErrorCountPropertyPath);
                return new ErrorNode(bindingExpression);
            }

            throw new InvalidOperationException($"Cannot create ErrorNode for type: {dependencyObject?.GetType()}");
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            var source = this.Source;
            if (source != null)
            {
                BindingOperations.ClearBinding(source, ErrorCountProperty);
                var parent = VisualTreeHelper.GetParent(this.Source);
                var node = (Node)parent?.GetValue(Scope.ErrorsProperty);
                node?.RemoveChild(this);
            }
        }

        protected override void OnHasErrorsChanged()
        {
            var hasErrors = this.HasErrors;
            Scope.SetHasErrors(this.Source, hasErrors);
            this.UpdateErrors();
            var parent = VisualTreeHelper.GetParent(this.Source);
            if (parent == null)
            {
                return;
            }

            var node = (Node)parent.GetValue(Scope.ErrorsProperty);
            if (hasErrors)
            {
                if (Scope.GetForInputTypes(parent)?.IsInputType(this.Source) == true)
                {
                    if (node == null)
                    {
                        Scope.SetErrors(parent, new ScopeNode(parent, this));
                    }
                    else
                    {
                        node.AddChild(this);
                    }
                }
            }
            else
            {
                node?.RemoveChild(this);
            }
        }

        protected override void OnChildrenChanged()
        {
            this.HasErrors = Validation.GetHasError(this.Source) || this.AllChildren.Any();
            this.UpdateErrors();
        }

        protected override void UpdateErrors()
        {
            if (this.LazyErrors.IsValueCreated)
            {
                var validationErrors = this.LazyErrors.Value;
                validationErrors.Clear();
                foreach (var validationError in Validation.GetErrors(this.Source))
                {
                    validationErrors.Add(validationError);
                }

                foreach (var child in this.AllChildren.OfType<ErrorNode>())
                {
                    foreach (var childError in child.Errors)
                    {
                        validationErrors.Add(childError);
                    }
                }
            }
        }

        private static void OnErrorCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var node = (ErrorNode)d.GetValue(Scope.ErrorsProperty);
            if (node == null)
            {
                return;
            }

            node.HasErrors = !Equals(e.NewValue, 0) || node.Children.Count > 0;
        }
    }
}