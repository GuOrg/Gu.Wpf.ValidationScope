using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class ErrorNode : DependencyObject, IDisposable
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
        private readonly ImmutableList<IErrorNode> children = ImmutableList<IErrorNode>.Empty;

        internal ErrorNode(UIElement source)
        {
            this.errorCountExpression = this.Bind(ErrorCountProperty)
                                            .OneWayTo(source, ErrorCountPropertyPath);
        }

        public bool HasErrors
        {
            get
            {
                var source = this.Source;
                if (source != null)
                {
                    if (Validation.GetHasError(source))
                    {
                        return true;
                    }
                }

                return this.children.Any(x => x.HasErrors);
            }
        }

        private DependencyObject Source => (DependencyObject)this.errorCountExpression?.ParentBinding.Source;

        public void Dispose()
        {
            var source = this.Source;
            if (source != null)
            {
                BindingOperations.ClearBinding(source, ErrorCountProperty);
            }
        }

        private static void OnErrorCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var node = (ErrorNode)d;
            var hasErrors = node.HasErrors;
                d.SetValue(HasErrorsPropertyKey, BooleanBoxes.Box(hasErrors));
                var parent = VisualTreeHelper.GetParent(d);
                while (parent != null)
                {
                    if (parent.GetValue(ForInputTypesProperty) != null)
                    {
                        break;
                    }

                    var parentErrors = (ErrorNode)parent.GetValue(ErrorsProperty);
                    if (parentErrors == null)
                    {
                        if (hasErrors)
                        {
                            parentErrors = new ErrorNode(node);
                            parent.SetValue(ErrorsProperty, parentErrors);
                        }
                    }
                    else
                    {
                        parentErrors.UpdateChildErrors(node, hasErrors);
                    }

                    parent.SetValue(HasErrorsPropertyKey, BooleanBoxes.Box(parentErrors?.HasErrors == true));
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
        }
    }
}