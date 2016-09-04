namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    [DebuggerDisplay("InputNode Errors: {Errors?.Count ?? 0}, Source: {Source}")]
    internal sealed class InputNode : ErrorNode
    {
        private static readonly DependencyProperty ValidationErrorsProxyProperty = DependencyProperty.RegisterAttached(
            "ValidationErrorsProxy",
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(Scope),
            new PropertyMetadata(null, OnErrorsProxyChanged));

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
            BindingOperations.SetBinding((DependencyObject)this.errorsBinding.Source, ValidationErrorsProxyProperty, this.errorsBinding);
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
            var node = Scope.GetNode(d) as InputNode;
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