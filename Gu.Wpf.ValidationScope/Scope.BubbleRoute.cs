// ReSharper disable PossibleMultipleEnumeration
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
        private static void UpdateParent(this DependencyObject source, IEnumerable<ValidationError> removed, IEnumerable<ValidationError> added)
        {
            if (source is null)
            {
                return;
            }

            var parent = VisualTreeHelper.GetParent(source) as FrameworkElement;
            if (parent is null || GetForInputTypes(parent) is null)
            {
                return;
            }

            var childNode = GetNode(source) as ErrorNode;
            var parentNode = GetNode(parent) as ErrorNode;
            if (childNode is null)
            {
                return;
            }

            if (IsScopeFor(parent, source))
            {
                if (parentNode is null)
                {
#pragma warning disable IDISP001, CA2000 // Dispose created. Disposed in SetNode.
                    parentNode = new ScopeNode(parent);
#pragma warning restore IDISP001, CA2000 // Dispose created.
                }

                parentNode.ChildCollection.TryAdd(childNode);
                parentNode.ErrorCollection.Remove(removed);
                parentNode.ErrorCollection.Add(added.Where(e => parent.IsScopeFor(e)).AsReadOnly());
                SetNode(parent, parentNode);
            }
            else
            {
                if (parentNode is null)
                {
                    return;
                }

                parentNode.ChildCollection.Remove(childNode);
                parentNode.ErrorCollection.Remove(removed);
                parentNode.ErrorCollection.Remove(childNode.Errors);
                if (parentNode is ScopeNode &&
                    parentNode.Errors.Count == 0)
                {
                    SetNode(parent, ValidNode.Default);
                }
            }
        }

        private static bool IsScopeFor(this DependencyObject parent, ValidationError error)
        {
            var inputTypes = GetForInputTypes(parent as FrameworkElement);
            if (inputTypes is null)
            {
                return false;
            }

            if (inputTypes.Contains(typeof(Scope)))
            {
                return true;
            }

            return inputTypes.Contains(error.Target());
        }

        private static bool IsScopeFor(this DependencyObject parent, DependencyObject source)
        {
            if (parent is null || source is null)
            {
                return false;
            }

            var inputTypes = GetForInputTypes(parent as FrameworkElement);
            if (inputTypes is null)
            {
                return false;
            }

            var node = GetNode(source);
            if (node is ValidNode || node.Errors.Count == 0)
            {
                return false;
            }

            if (inputTypes.Contains(typeof(Scope)) && node is ScopeNode)
            {
                return true;
            }

            foreach (var error in GetErrors(source))
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
            if (binding is null)
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
