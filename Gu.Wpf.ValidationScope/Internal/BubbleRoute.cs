namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    internal static class BubbleRoute
    {
        internal static void NotifyParents(ErrorNode node)
        {
            var source = node.Source;
            if (source == null)
            {
                return;
            }

            Scope.SetHasErrors(source, node.HasErrors);

            var parent = VisualTreeHelper.GetParent(source);
            Node childNode = node;
            while (parent != null && childNode != null)
            {
                var parentNode = (Node)parent.GetValue(Scope.ErrorsProperty);
                if (childNode.HasErrors)
                {
                    if (parent.IsValidationScopeFor(source))
                    {
                        if (parentNode == null)
                        {
                            parentNode = new ScopeNode(parent, childNode);
                            Scope.SetErrors(parent, childNode);
                        }
                        else
                        {
                            parentNode.AddChild(childNode);
                        }
                    }
                }
                else
                {
                    parentNode?.RemoveChild(childNode);
                }

                Scope.SetHasErrors(parent, parentNode?.HasErrors == true);
                childNode = parentNode;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        internal static void NotifyParents(ErrorNode node, IReadOnlyList<ValidationErrorChange> changes)
        {
            if (changes == null || changes.Count == 0)
            {
                return;
            }

            throw new NotImplementedException();
        }

        private static bool IsValidationScopeFor(this DependencyObject parent, DependencyObject source)
        {
            return Scope.GetForInputTypes(parent)?.IsInputType(source) == true;
        }
    }
}
