namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    internal static class BubbleRoute
    {
        internal static void Notify(ErrorNode node, IReadOnlyList<BatchChangeItem<ValidationError>> changes)
        {
            var source = node.Source;
            if (source == null || changes?.Count == 0)
            {
                return;
            }

            Scope.SetHasErrors(source, node.HasErrors);
            var parent = VisualTreeHelper.GetParent(source);
            Node childNode = node;
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
                        }
                    }
                    else
                    {
                        parentNode?.RemoveChild(childNode);
                    }
                }
                else
                {
                    parentNode?.RemoveChild(childNode);
                }

                if (parentNode is ScopeNode && parentNode.Children.Count == 0)
                {
                    parentNode = null;
                    Scope.SetErrors(parent, null);
                }

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

                Scope.SetHasErrors(parent, parentNode?.HasErrors == true);
                childNode = parentNode;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        private static bool IsValidationScopeFor(this DependencyObject parent, DependencyObject source)
        {
            return Scope.GetForInputTypes(parent)?.IsInputType(source) == true;
        }
    }
}
