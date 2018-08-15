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

    [DebuggerDisplay("InputNode Errors: {Errors?.Count ?? 0}, Source: {Source}")]
    public sealed class InputNode : ErrorNode
    {
        private static readonly DependencyProperty SourceErrorsProperty = DependencyProperty.RegisterAttached(
            "SourceErrors",
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(InputNode),
            new PropertyMetadata(ValidationScope.ErrorCollection.EmptyValidationErrors, OnSourceErrorsChanged));

        private static readonly PropertyPath ErrorsPropertyPath = new PropertyPath("(Validation.Errors)");
        private readonly Binding errorsBinding;

        internal InputNode(FrameworkElement source)
        {
            this.errorsBinding = new Binding
            {
                Path = ErrorsPropertyPath,
                Mode = BindingMode.OneWay,
                Source = source,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }

        public override DependencyObject Source => (DependencyObject)this.errorsBinding.Source;

        internal void BindToSourceErrors()
        {
            _ = BindingOperations.SetBinding((DependencyObject)this.errorsBinding.Source, SourceErrorsProperty, this.errorsBinding);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            var source = this.Source;
            if (source != null)
            {
                BindingOperations.ClearBinding(source, SourceErrorsProperty);
            }

            base.Dispose(true);
        }

        private static void OnSourceErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var node = Scope.GetNode(d) as InputNode;
            if (node == null)
            {
                // this happens when disposing
                return;
            }

            var oldErrors = (ReadOnlyObservableCollection<ValidationError>)e.OldValue;
            if (ShouldTrack(oldErrors))
            {
                CollectionChangedEventManager.RemoveHandler(oldErrors, node.OnSourceErrorsChanged);
            }

            var newErrors = (ReadOnlyObservableCollection<ValidationError>)e.NewValue;
            node.ErrorCollection.Remove(oldErrors);
            node.ErrorCollection.Add(newErrors);

            if (ShouldTrack(newErrors))
            {
                CollectionChangedEventManager.AddHandler(newErrors, node.OnSourceErrorsChanged);
            }
        }

        private static bool ShouldTrack(ReadOnlyObservableCollection<ValidationError> errors)
        {
            return errors != null && !ReferenceEquals(errors, ValidationScope.ErrorCollection.EmptyValidationErrors);
        }

        private void OnSourceErrorsChanged(object sender, NotifyCollectionChangedEventArgs e)
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
