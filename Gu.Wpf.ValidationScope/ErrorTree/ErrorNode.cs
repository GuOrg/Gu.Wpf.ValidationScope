namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    [DebuggerDisplay("ErrorNode Errors: {errors?.Count ?? 0}, Source: {Source}")]
    internal sealed class ErrorNode : Node, IWeakEventListener
    {
        private static readonly DependencyProperty ErrorsProxyProperty = DependencyProperty.RegisterAttached(
            "ErrorsProxy",
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(Scope),
            new PropertyMetadata(
                null,
                OnErrorsProxyChanged));

        private static readonly PropertyPath ErrorsPropertyPath = new PropertyPath("(Validation.Errors)");
        private readonly Binding errorsBinding;

        private ErrorNode(Binding errorsBinding)
            : base(false)
        {
            this.errorsBinding = errorsBinding;
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

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(CollectionChangedEventManager))
            {
                this.RefreshErrors();
                this.HasErrors = this.Errors.Count > 0 || this.Children.Count > 0;
                BubbleRoute.Notify(this);
                return true;
            }

            return false;
        }

        internal void BindToErrors()
        {
            BindingOperations.SetBinding((DependencyObject)this.errorsBinding.Source, ErrorsProxyProperty, this.errorsBinding);
        }

        internal IReadOnlyList<ValidationError> GetValidationErrors()
        {
            // not sure if  we need to protect against null here but doing it to be safe in case GC collects the binding.
            var source = this.Source;
            if (source == null || BindingOperations.GetBindingExpression(source, ErrorsProxyProperty) == null)
            {
                return EmptyValidationErrors;
            }

            return Validation.GetErrors(source) ?? EmptyValidationErrors;
        }

        protected internal override IReadOnlyList<ValidationError> GetAllErrors()
        {
            if (this.Source == null)
            {
                // not sure if  we need to protect against null here but doing it to be safe in case GC collects the binding.
                return EmptyValidationErrors;
            }

            var errors = this.GetValidationErrors();
            if (this.AllChildren.Any())
            {
                var allErrors = this.AllChildren.OfType<ErrorNode>()
                    .SelectMany(x => x.GetValidationErrors())
                    .ToList();
                if (errors != null)
                {
                    allErrors.AddRange(errors);
                }

                return allErrors;
            }
            else
            {
                return errors ?? EmptyValidationErrors;
            }
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
                var value = source.GetValue(ErrorsProxyProperty);
                if (value != null)
                {
                    CollectionChangedEventManager.RemoveListener((INotifyCollectionChanged)value, this);
                }

                BindingOperations.ClearBinding(source, ErrorsProxyProperty);
                this.RefreshErrors();
                BubbleRoute.Notify(this);
            }
        }

        protected override void OnChildrenChanged()
        {
            this.HasErrors = this.Errors.Count > 0 || this.AllChildren.Any();
        }

        private static void OnErrorsProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var node = (ErrorNode)d.GetValue(Scope.ErrorsProperty);
            if (node == null)
            {
                // this happens when disposing
                return;
            }

            var oldValue = (ReadOnlyObservableCollection<ValidationError>)e.OldValue;
            if (oldValue != null)
            {
                CollectionChangedEventManager.RemoveListener(oldValue, node);
            }

            var newValue = (ReadOnlyObservableCollection<ValidationError>)e.NewValue;
            if (newValue != null)
            {
                CollectionChangedEventManager.AddListener(newValue, node);
            }

            node.RefreshErrors();
            node.HasErrors = node.Errors.Count > 0 || node.Children.Count > 0;
            BubbleRoute.Notify(node);
        }
    }
}