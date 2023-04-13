namespace Gu.Wpf.ValidationScope;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

/// <summary>Implementation.</summary>
public static partial class Scope
{
    private static void UpdateParent(this DependencyObject source, IEnumerable<ValidationError> removed, IEnumerable<ValidationError> added)
    {
        if (VisualTreeHelper.GetParent(source) is UIElement parent &&
            GetForInputTypes(parent) is { })
        {
            if (GetNode(source) is ErrorNode childNode)
            {
                if (IsScopeFor(parent, source))
                {
#pragma warning disable IDISP001, CA2000 // Dispose created. Disposed in SetNode.
                    var parentNode = GetNode(parent) is ErrorNode errorNode
                        ? errorNode
                        : new ScopeNode(parent);
#pragma warning restore IDISP001, CA2000 // Dispose created.

                    _ = parentNode.ChildCollection.TryAdd(childNode);
                    parentNode.ErrorCollection.Remove(removed);
                    parentNode.ErrorCollection.Add(added.Where(e => parent.IsScopeFor(e)).AsReadOnly());
                    SetNode(parent, parentNode);
                }
                else if (GetNode(parent) is ErrorNode parentNode)
                {
                    _ = parentNode.ChildCollection.Remove(childNode);
                    parentNode.ErrorCollection.Remove(removed);
                    parentNode.ErrorCollection.Remove(childNode.Errors);
                    if (parentNode is ScopeNode { Errors.Count: 0 })
                    {
                        SetNode(parent, ValidNode.Default);
                    }
                }
            }
            else
            {
                SetNode(parent, ValidNode.Default);
            }
        }
    }

    private static bool IsScopeFor(this DependencyObject parent, ValidationError error)
    {
        if (parent is UIElement element &&
            GetForInputTypes(element) is { } inputTypes)
        {
            foreach (var inputType in inputTypes)
            {
                if (inputType == typeof(Scope))
                {
                    return true;
                }

                if (inputType.IsInstanceOfType(error.Target()))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsScopeFor(this DependencyObject parent, DependencyObject source)
    {
        if (parent is null || source is null)
        {
            return false;
        }

        if (parent is UIElement element &&
            GetForInputTypes(element) is { } inputTypes &&
            GetNode(source) is { } node)
        {
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
        }

        return false;
    }

    private static DependencyObject Target(this ValidationError error)
    {
        return error switch
        {
            { BindingInError: null } => throw new ArgumentNullException(nameof(error), "error.BindingInError == null"),
            { BindingInError: BindingExpressionBase bindingExpression } => bindingExpression.Target,
            _ => throw new ArgumentOutOfRangeException(nameof(error), error, $"ValidationError.BindingInError == {error.BindingInError}"),
        };
    }
}
