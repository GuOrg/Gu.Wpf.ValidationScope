namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    public static partial class Scope
    {
        private static void NotifyParent(this DependencyObject source, IEnumerable<ValidationError> removed, IEnumerable<ValidationError> added)
        {
            if (source == null)
            {
                return;
            }

            var parent = VisualTreeHelper.GetParent(source) as FrameworkElement;
            if (parent == null || GetForInputTypes(parent) == null)
            {
                return;
            }

            var childNode = GetNode(source) as ErrorNode;
            var parentNode = GetNode(parent) as ErrorNode;
            if (childNode == null)
            {
                return;
            }

            if (IsScopeFor(parent, source))
            {
                if (parentNode == null)
                {
                    parentNode = new ScopeNode(parent);
                }

                parentNode.ChildCollection.TryAdd(childNode);
                parentNode.ErrorCollection.Remove(removed);
                parentNode.ErrorCollection.Add(added.Where(e => parent.IsScopeFor(e)).AsReadOnly());
                SetNode(parent, parentNode);
            }
            else
            {
                if (parentNode == null)
                {
                    return;
                }

                parentNode.ChildCollection.Remove(childNode);
                parentNode.ErrorCollection.Remove(removed);
                parentNode.ErrorCollection.Remove(childNode.Errors);
            }
        }

        private static bool IsScopeFor(this DependencyObject parent, ValidationError error)
        {
            var inputTypes = GetForInputTypes(parent as FrameworkElement);
            if (inputTypes == null)
            {
                return false;
            }

            return inputTypes.Contains(error.Target());
        }

        private static bool IsScopeFor(this DependencyObject parent, DependencyObject source)
        {
            if (parent == null || source == null)
            {
                return false;
            }

            var inputTypes = GetForInputTypes(parent as FrameworkElement);
            if (inputTypes == null)
            {
                return false;
            }

            var node = GetNode(source);
            if (node is ValidNode)
            {
                return false;
            }

            if (inputTypes.Contains(typeof(Scope)) && node is ScopeNode)
            {
                return true;
            }

            foreach (var error in Scope.GetErrors(source))
            {
                if (inputTypes.Contains(error.Target()))
                {
                    return true;
                }
            }

            return false;
        }

        private static DependencyObject Target(this ValidationError error)
        {
            var binding = error?.BindingInError;
            if (binding == null)
            {
                throw new ArgumentNullException(nameof(error), "error.BindingInError == null");
            }

            var bindingExpression = binding as BindingExpressionBase;
            if (bindingExpression != null)
            {
                return bindingExpression.Target;
            }

            throw new ArgumentOutOfRangeException(nameof(error), error, $"ValidationError.BindingInError == {binding}");
        }
    }
}