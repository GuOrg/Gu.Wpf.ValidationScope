namespace Gu.Wpf.ValidationScope;

using System.Collections.ObjectModel;
using System.Windows.Controls;

/// <summary>A node in a validation scope.</summary>
public abstract class Node
{
    /// <summary>Gets a value indicating whether this node or a sub node has one or more validation errors.</summary>
    public abstract bool HasError { get; }

    /// <summary>Gets a collection with all errors for this node and all nested sub nodes in the validation scope.</summary>
    public abstract ReadOnlyObservableCollection<ValidationError> Errors { get; }

    /// <summary>Gets the child nodes with errors.</summary>
    public abstract ReadOnlyObservableCollection<ErrorNode> Children { get; }
}
