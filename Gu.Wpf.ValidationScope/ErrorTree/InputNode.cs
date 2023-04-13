namespace Gu.Wpf.ValidationScope;

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

/// <summary>
/// Node corresponding to an input element.
/// </summary>
[DebuggerDisplay("InputNode Errors: {Errors?.Count ?? 0}, Source: {Source}")]
public sealed class InputNode : ErrorNode
{
    private static readonly DependencyProperty SourceErrorsProperty = DependencyProperty.RegisterAttached(
        "SourceErrors",
        typeof(ReadOnlyObservableCollection<ValidationError>),
        typeof(InputNode),
        new PropertyMetadata(ErrorCollection.EmptyValidationErrors, OnSourceErrorsChanged));

    private static readonly PropertyPath ErrorsPropertyPath = new("(Validation.Errors)");
    private readonly Binding errorsBinding;

    internal InputNode(UIElement source)
    {
        this.errorsBinding = new Binding
        {
            Path = ErrorsPropertyPath,
            Mode = BindingMode.OneWay,
            Source = source,
        };
    }

    /// <summary>
    /// Gets the <see cref="DependencyObject"/> to track validity for.
    /// </summary>
    public override DependencyObject? Source => (DependencyObject?)this.errorsBinding.Source;

    internal void BindToSource()
    {
        _ = BindingOperations.SetBinding(this.Source, SourceErrorsProperty, this.errorsBinding);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            BindingOperations.ClearBinding(this.Source, SourceErrorsProperty);
        }

        base.Dispose(disposing);
    }

    private static void OnSourceErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (Scope.GetNode(d) is InputNode node)
        {
            if (e.OldValue is ReadOnlyObservableCollection<ValidationError> oldErrors &&
                !ReferenceEquals(oldErrors, ErrorCollection.EmptyValidationErrors))
            {
                CollectionChangedEventManager.RemoveHandler(oldErrors, node.OnSourceErrorsChanged);
                node.ErrorCollection.Remove(oldErrors);
            }

            if (e.NewValue is ReadOnlyObservableCollection<ValidationError> newErrors &&
                !ReferenceEquals(newErrors, ErrorCollection.EmptyValidationErrors))
            {
                CollectionChangedEventManager.AddHandler(newErrors, node.OnSourceErrorsChanged);
                node.ErrorCollection.Add(newErrors);
            }
        }
    }

    private void OnSourceErrorsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
#pragma warning disable CS8604 // Possible null reference argument.
                this.ErrorCollection.Add(e.NewItems.Cast<ValidationError>());
#pragma warning restore CS8604 // Possible null reference argument.
                break;
            case NotifyCollectionChangedAction.Remove:
#pragma warning disable CS8604 // Possible null reference argument.
                this.ErrorCollection.Remove(e.OldItems.Cast<ValidationError>());
#pragma warning restore CS8604 // Possible null reference argument.
                break;
            default:
                // http://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Validation.cs,507
                throw new ArgumentOutOfRangeException(nameof(e), e.Action, "Should only ever be add or remove.");
        }
#pragma warning restore IDE0079 // Remove unnecessary suppression
    }
}