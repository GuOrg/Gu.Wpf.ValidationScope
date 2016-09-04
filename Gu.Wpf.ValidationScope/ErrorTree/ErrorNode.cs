namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    [DebuggerDisplay("ErrorNode Errors: {errors?.Count ?? 0}, Source: {Source}")]
    internal sealed class ErrorNode : Node
    {
        private static readonly DependencyProperty ValidationErrorsProxyProperty = DependencyProperty.RegisterAttached(
            "ValidationErrorsProxy",
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(Scope),
            new PropertyMetadata(
                null,
                OnErrorsProxyChanged));

        private static readonly PropertyPath ErrorsPropertyPath = new PropertyPath("(Validation.Errors)");
        private readonly Binding errorsBinding;

        private ErrorNode(Binding errorsBinding)
        {
            this.errorsBinding = errorsBinding;
            BindingOperations.SetBinding((DependencyObject)this.errorsBinding.Source, ValidationErrorsProxyProperty, this.errorsBinding);
        }

        public override DependencyObject Source => (DependencyObject)this.errorsBinding.Source;

        public static ErrorNode CreateFor(DependencyObject dependencyObject)
        {
            if (!(dependencyObject is UIElement || dependencyObject is ContentElement))
            {
                throw new InvalidOperationException($"Cannot create ErrorNode for type: {dependencyObject?.GetType()}");
            }

            var binding = new Binding
            {
                Path = ErrorsPropertyPath,
                Mode = BindingMode.OneWay,
                Source = dependencyObject,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            return new ErrorNode(binding);
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
                BindingOperations.ClearBinding(source, ValidationErrorsProxyProperty);
            }

            base.Dispose(true);
        }

        private static void OnErrorsProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var node = (ErrorNode)d.GetValue(Scope.NodeProperty);
            if (node == null)
            {
                // this happens when disposing
                return;
            }

            node.ErrorCollection.Remove((ReadOnlyObservableCollection<ValidationError>)e.OldValue);
            node.ErrorCollection.Add((ReadOnlyObservableCollection<ValidationError>)e.NewValue);
        }
    }
}