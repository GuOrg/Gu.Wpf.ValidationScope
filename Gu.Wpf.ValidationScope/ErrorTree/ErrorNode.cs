namespace Gu.Wpf.ValidationScope
{
    using System;
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

        //public ReadOnlyObservableCollection<ValidationError> Errors => Validation.GetErrors(this.Source);

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

        public void Dispose()
        {
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

        protected internal override void RemoveChild(IErrorNode errorNode)
        {
            this.EditableChildren.Remove(errorNode);
            this.HasErrors = Validation.GetHasError(this.Source) || this.EditableChildren.Count > 0;
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