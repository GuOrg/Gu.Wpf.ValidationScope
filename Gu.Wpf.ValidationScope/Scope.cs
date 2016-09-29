namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>Provides attached properties and events for validation scopes.</summary>
    public static partial class Scope
    {
#pragma warning disable SA1202 // Elements must be ordered by access

        public static readonly DependencyProperty ForInputTypesProperty = DependencyProperty.RegisterAttached(
            "ForInputTypes",
            typeof(InputTypeCollection),
            typeof(Scope),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnForInputTypesChanged));

        private static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "HasError",
            typeof(bool),
            typeof(Scope),
            new PropertyMetadata(BooleanBoxes.False));

        public static readonly DependencyProperty HasErrorProperty = HasErrorPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Errors",
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(Scope),
            new PropertyMetadata(ErrorCollection.EmptyValidationErrors));

        public static readonly DependencyProperty ErrorsProperty = ErrorsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey NodePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Node",
            typeof(Node),
            typeof(Scope),
            new PropertyMetadata(ValidNode.Default, OnNodeChanged));

        public static readonly DependencyProperty NodeProperty = NodePropertyKey.DependencyProperty;

        public static readonly DependencyProperty OneWayToSourceBindingsProperty = DependencyProperty.RegisterAttached(
            "OneWayToSourceBindings",
            typeof(OneWayToSourceBindings),
            typeof(Scope),
            new PropertyMetadata(default(OneWayToSourceBindings), OnWayToSourceBindingsChanged));

        public static void SetOneWayToSourceBindings(this UIElement element, OneWayToSourceBindings value)
        {
            element.SetValue(OneWayToSourceBindingsProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static OneWayToSourceBindings GetOneWayToSourceBindings(this UIElement element)
        {
            return (OneWayToSourceBindings)element.GetValue(OneWayToSourceBindingsProperty);
        }

        public static void SetForInputTypes(FrameworkElement element, InputTypeCollection value) => element.SetValue(ForInputTypesProperty, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static InputTypeCollection GetForInputTypes(FrameworkElement element) => (InputTypeCollection)element?.GetValue(ForInputTypesProperty);

        private static void SetHasError(DependencyObject element, bool value) => element.SetValue(HasErrorPropertyKey, BooleanBoxes.Box(value));

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetHasError(UIElement element) => (bool)element.GetValue(HasErrorProperty);

        private static void SetErrors(this DependencyObject element, ReadOnlyObservableCollection<ValidationError> value)
        {
            element.SetValue(ErrorsPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static ReadOnlyObservableCollection<ValidationError> GetErrors(DependencyObject element)
        {
            return (ReadOnlyObservableCollection<ValidationError>)element.GetValue(ErrorsProperty);
        }

        private static void SetNode(DependencyObject element, Node value) => element.SetValue(NodePropertyKey, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Node GetNode(DependencyObject element) => (Node)element?.GetValue(NodeProperty);

#pragma warning restore SA1202 // Elements must be ordered by access

        private static void OnWayToSourceBindingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OneWayToSourceBindings)e.OldValue)?.ClearValue(OneWayToSourceBindings.ElementProperty);
            ((OneWayToSourceBindings)e.NewValue)?.SetValue(OneWayToSourceBindings.ElementProperty, d);
        }

        private static void OnForInputTypesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d.GetType().FullName == "System.Windows.Documents.CaretElement")
            {
                return;
            }

            var newValue = (InputTypeCollection)e.NewValue;
            if (newValue == null)
            {
                d.SetValue(NodePropertyKey, ValidNode.Default);
                return;
            }

            if (newValue.Contains(d))
            {
                var inputNode = GetNode(d) as InputNode;
                if (inputNode == null)
                {
                    inputNode = new InputNode((FrameworkElement)d);
                    SetNode(d, inputNode);
                }
            }
            else
            {
                // below looks pretty expensive but
                // a) Not expecting scope to change often.
                // b) Not expecting many errors often.
                // optimize if profiler points at it
                // ReSharper disable once UseNullPropagation
                var errorNode = GetNode(d) as ErrorNode;
                if (errorNode == null)
                {
                    return;
                }

                var removedErrors = errorNode.Errors.Where(error => !IsScopeFor(d, error)).ToArray();
                if (removedErrors.Length > 0)
                {
                    errorNode.ErrorCollection.Remove(removedErrors);
                    if (errorNode.Errors.Count == 0)
                    {
                        SetNode(d, ValidNode.Default);
                    }
                    else
                    {
                        var removeChildren = errorNode.Children.Where(c => !errorNode.Errors.Intersect(c.Errors).Any()).ToArray();
                        foreach (var removeChild in removeChildren)
                        {
                            errorNode.ChildCollection.Remove(removeChild);
                        }
                    }
                }
            }
        }

        private static void OnNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.Print($"Set Node = {e.NewValue?.GetType().Name ?? "null"} for {d}");
            var oldNode = e.OldValue as ErrorNode;
            if (oldNode != null)
            {
                ErrorsChangedEventManager.RemoveHandler(oldNode, OnNodeErrorsChanged);
                oldNode.Dispose();
            }

            var newNode = e.NewValue as ErrorNode;
            if (newNode != null)
            {
                (newNode as InputNode)?.BindToSourceErrors();
                UpdateErrorsAndHasErrors(d, GetErrors(d), newNode.Errors, newNode.Errors);
                ErrorsChangedEventManager.AddHandler(newNode, OnNodeErrorsChanged);
            }
            else
            {
                UpdateErrorsAndHasErrors(d, GetErrors(d), ErrorCollection.EmptyValidationErrors, ErrorCollection.EmptyValidationErrors);
            }
        }

        private static void OnNodeErrorsChanged(object sender, ErrorsChangedEventArgs e)
        {
            var node = (ErrorNode)sender;
            UpdateErrorsAndHasErrors(node.Source, e.Removed, e.Added, node.Errors);
        }

        // this helper sets properties and raises events in the same order as System.Controls.Validation
        private static void UpdateErrorsAndHasErrors(
            DependencyObject dependencyObject,
            IEnumerable<ValidationError> removedErrors,
            IEnumerable<ValidationError> addedErrors,
            ReadOnlyObservableCollection<ValidationError> errors)
        {
            if (errors.Any())
            {
                SetErrors(dependencyObject, errors);
                SetHasError(dependencyObject, true);
            }
            else
            {
                SetHasError(dependencyObject, false);
                SetErrors(dependencyObject, ErrorCollection.EmptyValidationErrors);
            }

            // ReSharper disable PossibleMultipleEnumeration
            var removed = removedErrors.Except(addedErrors).AsReadOnly();
            var added = addedErrors.Except(removedErrors).AsReadOnly();
            if (added.Count == 0 && removed.Count == 0)
            {
                return;
            }

            dependencyObject.UpdateParent(removed, added);
            (dependencyObject as UIElement)?.RaiseEvent(new ErrorsChangedEventArgs(removed, added));
            (dependencyObject as ContentElement)?.RaiseEvent(new ErrorsChangedEventArgs(removed, added));

            foreach (var error in removed)
            {
                (dependencyObject as UIElement)?.RaiseEvent(new ScopeValidationErrorEventArgs(error, ValidationErrorEventAction.Removed));
                (dependencyObject as ContentElement)?.RaiseEvent(new ScopeValidationErrorEventArgs(error, ValidationErrorEventAction.Removed));
            }

            foreach (var error in added)
            {
                (dependencyObject as UIElement)?.RaiseEvent(new ScopeValidationErrorEventArgs(error, ValidationErrorEventAction.Added));
                (dependencyObject as ContentElement)?.RaiseEvent(new ScopeValidationErrorEventArgs(error, ValidationErrorEventAction.Added));
            }

            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
