namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

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
                OnScopeForChanged,
                OnScopeForCoerce));

        private static object OnScopeForCoerce(DependencyObject d, object basevalue)
        {
            if (d is Adorner || d is AdornerLayer)
            {
                return null;
            }

            return d is FrameworkElement ? basevalue : null;
        }

        private static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "HasError",
            typeof(bool),
            typeof(Scope),
            new PropertyMetadata(BooleanBoxes.False, OnHasErrorChanged));

        public static readonly DependencyProperty HasErrorProperty = HasErrorPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Errors",
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(Scope),
            new PropertyMetadata(ErrorCollection.EmptyValidationErrors, OnErrorsChanged));

        public static readonly DependencyProperty ErrorsProperty = ErrorsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey NodePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Node",
            typeof(IErrorNode),
            typeof(Scope),
            new PropertyMetadata(default(IErrorNode), OnNodeChanged));

        public static readonly DependencyProperty NodeProperty = NodePropertyKey.DependencyProperty;

        public static void SetForInputTypes(this FrameworkElement element, InputTypeCollection value) => element.SetValue(ForInputTypesProperty, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static InputTypeCollection GetForInputTypes(FrameworkElement element) => (InputTypeCollection)element?.GetValue(ForInputTypesProperty);

        private static void SetHasErrors(DependencyObject element, bool value) => element.SetValue(HasErrorPropertyKey, BooleanBoxes.Box(value));

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetHasError(UIElement element) => (bool)element.GetValue(HasErrorProperty);

        private static void SetErrors(this DependencyObject element, ReadOnlyObservableCollection<ValidationError> value)
        {
            element.SetValue(ErrorsPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static ReadOnlyObservableCollection<ValidationError> GetErrors(this DependencyObject element)
        {
            return (ReadOnlyObservableCollection<ValidationError>)element.GetValue(ErrorsProperty);
        }

        internal static void SetNode(DependencyObject element, IErrorNode value) => element.SetValue(NodePropertyKey, value);

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static IErrorNode GetNode(DependencyObject element) => (IErrorNode)element?.GetValue(NodeProperty);

#pragma warning restore SA1202 // Elements must be ordered by access

        internal static bool IsScopeFor(this DependencyObject parent, DependencyObject source)
        {
            if (parent == null || source == null)
            {
                return false;
            }

            var inputTypes = GetForInputTypes(parent as FrameworkElement);
            if (inputTypes == null)
            {
                return false;
            }

            if (inputTypes.IsInputType(source) ||
                inputTypes.Contains(typeof(Scope)))
            {
                return true;
            }

            return false;
        }

        private static void OnScopeForChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (InputTypeCollection)e.NewValue;
            if (newValue == null)
            {
                Debug.Print($"Removed Node for {d}");
                d.ClearValue(NodePropertyKey);
                return;
            }

            if (newValue.IsInputType(d))
            {
                var errorNode = GetNode(d) as ErrorNode;
                if (errorNode == null)
                {
                    Debug.Print($"Created ErrorNode for {d}");
                    errorNode = ErrorNode.CreateFor(d);
                    SetNode(d, errorNode);
                }
                else
                {
                    var parent = VisualTreeHelper.GetParent(d);
                    if (parent.IsScopeFor(d))
                    {
                        var parentNode = (Node)GetNode(parent);
                        parentNode?.AddChild(errorNode);
                    }
                }
            }
            else
            {
                var scopeNode = GetNode(d) as ScopeNode;
                if (scopeNode != null)
                {
                    Debug.Print($"Created ScopeNode for {d}");
                    SetNode(d, new ScopeNode(d));
                }
            }
        }

        private static void OnNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newNode = (Node)e.NewValue;
            var parent = VisualTreeHelper.GetParent(d);
            var oldNode = (Node)e.OldValue;
            if (oldNode != null)
            {
                oldNode.Dispose();
                ErrorsChangedEventManager.RemoveHandler(oldNode, OnNodeErrorsChanged);
            }

            if (newNode != null)
            {
                UpdateErrorsAndHasErrors(d, GetErrors(d), newNode.Errors, newNode.Errors);
                if (parent.IsScopeFor(d))
                {
                    var parentNode = (Node)GetNode(parent);
                    parentNode?.AddChild(newNode);
                }

                ErrorsChangedEventManager.AddHandler(newNode, OnNodeErrorsChanged);
            }
            else
            {
                UpdateErrorsAndHasErrors(d, GetErrors(d), ErrorCollection.EmptyValidationErrors, ErrorCollection.EmptyValidationErrors);
            }

            d.SetCurrentValue(NodeProxyProperty, e.NewValue);
        }

        private static void OnNodeErrorsChanged(object sender, ErrorsChangedEventArgs e)
        {
            var node = (Node)sender;
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
                SetHasErrors(dependencyObject, true);
            }
            else
            {
                SetHasErrors(dependencyObject, false);
                SetErrors(dependencyObject, ErrorCollection.EmptyValidationErrors);
            }

            foreach (var error in removedErrors.Except(addedErrors))
            {
                (dependencyObject as UIElement)?.RaiseEvent(new ScopeValidationErrorEventArgs(error, ValidationErrorEventAction.Removed));
                (dependencyObject as ContentElement)?.RaiseEvent(new ScopeValidationErrorEventArgs(error, ValidationErrorEventAction.Removed));
            }

            foreach (var error in addedErrors.Except(removedErrors))
            {
                (dependencyObject as UIElement)?.RaiseEvent(new ScopeValidationErrorEventArgs(error, ValidationErrorEventAction.Added));
                (dependencyObject as ContentElement)?.RaiseEvent(new ScopeValidationErrorEventArgs(error, ValidationErrorEventAction.Added));
            }
        }

        private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(HasErrorProxyProperty, e.NewValue);
        }

        private static void OnErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(ErrorsProxyProperty, e.NewValue);
        }
    }
}
