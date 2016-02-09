namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    internal static class BubbleRoute
    {
        internal static void Notify(ErrorNode errorNode, IReadOnlyList<BatchChangeItem<ValidationError>> changes)
        {
            var source = errorNode.Source;
            if (source == null || changes?.Count == 0)
            {
                return;
            }

            Scope.SetHasErrors(source, errorNode.HasErrors);
            var parent = VisualTreeHelper.GetParent(source);
            Node childNode = errorNode;
            while (parent != null)
            {
                if (Scope.GetForInputTypes(parent) == null)
                {
                    break;
                }

                var parentNode = (Node)parent.GetValue(Scope.ErrorsProperty);
                if (childNode?.HasErrors == true)
                {
                    if (parent.IsValidationScopeFor(source))
                    {
                        if (parentNode == null)
                        {
                            parentNode = new ScopeNode(parent, childNode);
                            Scope.SetErrors(parent, parentNode);
                        }
                        else
                        {
                            parentNode.AddChild(childNode);
                            UpdateErrors(parentNode, changes);
                        }
                    }
                    else
                    {
                        parentNode?.RemoveChild(childNode);
                        UpdateErrors(parentNode, changes);
                    }
                }
                else
                {
                    parentNode?.RemoveChild(childNode);
                    UpdateErrors(parentNode, changes);
                }

                if (parentNode is ScopeNode && parentNode.Children.Count == 0)
                {
                    parentNode = null;
                    Scope.SetErrors(parent, null);
                }

                Scope.SetHasErrors(parent, parentNode?.HasErrors == true);
                childNode = parentNode;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        private static void UpdateErrors(Node parentNode, IReadOnlyList<BatchChangeItem<ValidationError>> changes)
        {
            if (parentNode != null && parentNode.LazyErrors.IsValueCreated)
            {
                if (changes != null)
                {
                    parentNode.LazyErrors.Value.Update(changes);
                }
                else
                {
                    parentNode.LazyErrors.Value.Refresh(parentNode.GetAllErrors());
                }
            }
        }

        private static bool IsValidationScopeFor(this DependencyObject parent, DependencyObject source)
        {
            return Scope.GetForInputTypes(parent)?.IsInputType(source) == true;
        }
    }
}
