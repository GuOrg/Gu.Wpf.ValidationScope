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
    using System.Windows.Media;

    public sealed class ErrorNode : Node, IDisposable, IWeakEventListener
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
            Debugger.Break();
            if (managerType == typeof(CollectionChangedEventManager))
            {
                var args = (NotifyCollectionChangedEventArgs)e;
                IReadOnlyList<BatchChangeItem<ValidationError>> changes;
                if (this.ErrorCollection.CanUpdate(args))
                {
                    changes = this.ErrorCollection.Update(args);
                }
                else
                {
                    if (this.AllChildren.Any())
                    {
                        var errors = this.AllChildren.OfType<ErrorNode>()
                                                     .SelectMany(x => x.ErrorCollection)
                                                     .ToList();
                        errors.AddRange((IEnumerable<ValidationError>)sender);
                        changes = this.ErrorCollection.Refresh((IReadOnlyList<ValidationError>)sender);
                    }
                    else
                    {
                        changes = this.ErrorCollection.Refresh((IReadOnlyList<ValidationError>)sender);
                    }
                }

                this.HasErrors = this.ErrorCollection.Count != 0;
                BubbleRoute.Notify(this, changes);
                return true;
            }

            return false;
        }

        internal void BindToErrors()
        {
            BindingOperations.SetBinding((DependencyObject)this.errorsBinding.Source, ErrorsProxyProperty, this.errorsBinding);
        }

        protected internal override IReadOnlyList<ValidationError> GetAllErrors()
        {
            if (this.Source == null)
            {
                // not sure we need to protect against null here but doing it to be safe in case GC collects the binding.
                return EmptyValidationErrors;
            }

            var errors = Validation.GetErrors(this.Source);
            if (this.AllChildren.Any())
            {
                var allErrors = this.AllChildren.OfType<ErrorNode>()
                                             .SelectMany(x => x.ErrorCollection)
                                             .ToList();
                if (errors != null)
                {
                    allErrors.AddRange((IEnumerable<ValidationError>)errors);
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
                BindingOperations.ClearBinding(source, ErrorsProxyProperty);
                var parent = VisualTreeHelper.GetParent(this.Source);
                var node = (Node)parent?.GetValue(Scope.ErrorsProperty);
                node?.RemoveChild(this);
            }
        }

        protected override void OnChildrenChanged()
        {
            this.HasErrors = Validation.GetHasError(this.Source) || this.AllChildren.Any();
        }

        private static void OnErrorsProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var node = (ErrorNode)d.GetValue(Scope.ErrorsProperty);
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

            var changes = node.ErrorCollection.Update(oldValue, newValue);
            node.HasErrors = !Equals(e.NewValue, 0) || node.Children.Count > 0;
            BubbleRoute.Notify(node, changes);
        }
    }
}