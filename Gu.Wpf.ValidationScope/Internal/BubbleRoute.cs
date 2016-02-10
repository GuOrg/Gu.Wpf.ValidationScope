namespace Gu.Wpf.ValidationScope
{
    using System.Windows;
    using System.Windows.Media;

    internal static class BubbleRoute
    {
        internal static void Notify(ErrorNode errorNode)
        {
            var source = errorNode.Source;
            if (source == null)
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
                if (ShouldRemoveChildNode(parentNode, childNode))
                {
                    parentNode.RemoveChild(childNode);
                    if (parentNode is ScopeNode && parentNode.Children.Count == 0)
                    {
                        Scope.SetErrors(parent, null);
                    }
                    else
                    {
                        parentNode.RefreshErrors();
                    }
                }
                else if (childNode?.HasErrors == true &&
                         parent.IsValidationScopeFor(source))
                {
                    if (parentNode == null)
                    {
                        parentNode = new ScopeNode(parent, childNode);
                        Scope.SetErrors(parent, parentNode);
                    }
                    else
                    {
                        parentNode.AddChild(childNode);
                        parentNode.RefreshErrors();
                    }
                }

                Scope.SetHasErrors(parent, parentNode?.HasErrors == true);
                childNode = parentNode;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        private static bool ShouldRemoveChildNode(Node parentNode, Node childNode)
        {
            if (parentNode == null || childNode == null)
            {
                return false;
            }

            if (!childNode.HasErrors)
            {
                return true;
            }

            var parent = parentNode.Source;
            if (parent.IsValidationScopeFor(childNode.Source))
            {
                return false;
            }

            foreach (var child in childNode.AllChildren)
            {
                if (parent.IsValidationScopeFor(child.Source))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValidationScopeFor(this DependencyObject parent, DependencyObject source)
        {
            if (parent == null || source == null)
            {
                return false;
            }

            return Scope.GetForInputTypes(parent)?.IsInputType(source) == true;
        }
    }
}
