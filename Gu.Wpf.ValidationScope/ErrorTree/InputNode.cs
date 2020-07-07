namespace Gu.Wpf.ValidationScope
{
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

        private static readonly PropertyPath ErrorsPropertyPath = new PropertyPath("(Validation.Errors)");
        private readonly Binding errorsBinding;

        internal InputNode(FrameworkElement source)
        {
            this.Source = source;
            this.errorsBinding = new Binding
            {
                Path = ErrorsPropertyPath,
                Mode = BindingMode.OneWay,
                RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };
        }

        /// <summary>
        /// Gets the <see cref="DependencyObject"/> to track validity for.
        /// </summary>
        public override DependencyObject Source { get; }

        internal void BindToSourceErrors()
        {
            _ = BindingOperations.SetBinding(this.Source, SourceErrorsProperty, this.errorsBinding);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var source = this.Source;
                if (source != null)
                {
                    BindingOperations.ClearBinding(source, SourceErrorsProperty);
                }
            }

            base.Dispose(disposing);
        }

        private static void OnSourceErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var node = Scope.GetNode(d) as InputNode;
            if (node is null)
            {
                // this happens when disposing
                return;
            }

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

        private void OnSourceErrorsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.ErrorCollection.Add(e.NewItems.Cast<ValidationError>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    this.ErrorCollection.Remove(e.OldItems.Cast<ValidationError>());
                    break;
                default:
                    // http://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Validation.cs,507
                    throw new ArgumentOutOfRangeException(nameof(e), e.Action, "Should only ever be add or remove.");
            }
        }
    }
}
