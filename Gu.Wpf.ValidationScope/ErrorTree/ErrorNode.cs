// ReSharper disable ArrangeThisQualifier
namespace Gu.Wpf.ValidationScope;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

/// <summary>Base class for a node that has validation errors.</summary>
public abstract class ErrorNode : Node, INotifyErrorsChanged, INotifyPropertyChanged, IDisposable
{
    private static readonly PropertyChangedEventArgs HasErrorsPropertyChangedEventArgs = new(nameof(HasError));
    private readonly Lazy<ChildCollection> children = new(() => new ChildCollection());
    private bool disposed;

    /// <summary>Initializes a new instance of the <see cref="ErrorNode"/> class.</summary>
    protected ErrorNode()
    {
        this.ErrorCollection.ErrorsChanged += this.OnErrorsChanged;
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Notifies when <see cref="Errors"/> changes.</summary>
    public event EventHandler<ErrorsChangedEventArgs>? ErrorsChanged;

    /// <summary>
    /// Gets a value indicating whether <see cref="Errors"/> is has items.
    /// </summary>
    public override bool HasError => this.ErrorCollection.Any();

    /// <summary>
    /// Gets the current errors.
    /// </summary>
    public override ReadOnlyObservableCollection<ValidationError> Errors => this.ErrorCollection;

    /// <summary>
    /// Gets the child error nodes of this instance.
    /// </summary>
    public override ReadOnlyObservableCollection<ErrorNode> Children => this.children.IsValueCreated ? this.children.Value : ChildCollection.Empty;

    /// <summary>Gets the source element for this node.</summary>
    public abstract DependencyObject? Source { get; }

    internal ErrorCollection ErrorCollection { get; } = new ErrorCollection();

    internal ChildCollection ChildCollection => this.children.Value;

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of this instance.
    /// </summary>
    /// <remarks>
    /// Called from Dispose() with disposing=true, and from the finalizer with disposing=false.
    /// Guidelines:
    /// 1. We may be called more than once: do nothing after the first call.
    /// 2. Avoid throwing exceptions if disposing is false, i.e. if we're being finalized.
    /// </remarks>
    /// <param name="disposing">True if called from Dispose(), false if called from the finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;

        if (disposing)
        {
            this.ErrorCollection.ErrorsChanged -= this.OnErrorsChanged;
        }
    }

    /// <summary>
    /// Raise <see cref="PropertyChanged"/>.
    /// </summary>
    /// <param name="args">The <see cref="PropertyChangedEventArgs"/>.</param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        this.PropertyChanged?.Invoke(this, args);
    }

    private void OnErrorsChanged(object? sender, ErrorsChangedEventArgs e)
    {
        if ((this.Errors.Count == 0 && e.Removed.Any()) ||
            Enumerable.SequenceEqual(this.Errors, e.Added))
        {
            this.OnPropertyChanged(HasErrorsPropertyChangedEventArgs);
        }

        this.ErrorsChanged?.Invoke(this, e);
    }
}