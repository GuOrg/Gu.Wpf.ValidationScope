namespace Gu.Wpf.ValidationScope;

using System;
using System.Diagnostics;
using System.Windows;

/// <summary>
/// A node that is not tracked but may have tracked children.
/// </summary>
[DebuggerDisplay("ScopeNode Errors: {Errors?.Count ?? 0}, Children: {Children.Count}, Source: {Source}")]
public sealed class ScopeNode : ErrorNode
{
    private readonly WeakReference<DependencyObject> sourceReference;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScopeNode"/> class.
    /// </summary>
    /// <param name="source">´The source element for this node.</param>
    public ScopeNode(DependencyObject source)
    {
        this.sourceReference = new WeakReference<DependencyObject>(source);
    }

    /// <summary>Gets the source element for this node.</summary>
    public override DependencyObject? Source => this.sourceReference.TryGetTarget(out var source) ? source : null;
}
