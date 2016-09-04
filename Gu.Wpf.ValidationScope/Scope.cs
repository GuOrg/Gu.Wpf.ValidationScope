namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
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
                OnScopeForChanged));

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
            new PropertyMetadata(ValidNode.Default, OnNodeChanged));

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

            var node = GetNode(source);
            if (node is ValidNode)
            {
                return false;
            }

            if (inputTypes.Contains(typeof(Scope)) && node is ScopeNode)
            {
                return true;
            }

            foreach (var error in node.Errors)
            {
                if (inputTypes.IsInputType(((BindingExpressionBase)error.BindingInError).Target))
                {
                    return true;
                }
            }

            return false;
        }

        private static void OnScopeForChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

            if (newValue.IsInputType(d))
            {
                var errorNode = GetNode(d) as InputNode;
                if (errorNode == null)
                {
                    errorNode = new InputNode((FrameworkElement)d);
                    SetNode(d, errorNode);
                }
                else
                {
                    var parent = VisualTreeHelper.GetParent(d);
                    if (parent.IsScopeFor(d))
                    {
                        var parentNode = (ErrorNode)GetNode(parent);
                        parentNode?.AddChild(errorNode);
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
                oldNode.Dispose();
                ErrorsChangedEventManager.RemoveHandler(oldNode, OnNodeErrorsChanged);
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

            d.SetCurrentValue(NodeProxyProperty, e.NewValue);
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
                SetHasErrors(dependencyObject, true);
            }
            else
            {
                SetHasErrors(dependencyObject, false);
                SetErrors(dependencyObject, ErrorCollection.EmptyValidationErrors);
            }

            // ReSharper disable PossibleMultipleEnumeration
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

            // ReSharper restore PossibleMultipleEnumeration
        }

        private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(d);
            if ((bool)e.NewValue)
            {
                if (parent.IsScopeFor(d))
                {
                    var parentNode = GetNode(parent) as ErrorNode;
                    var errorNode = (ErrorNode)GetNode(d);
                    if (parentNode == null)
                    {
                        parentNode = new ScopeNode(parent);
                        parentNode.AddChild(errorNode);
                        SetNode(parent, parentNode);
                    }
                    else
                    {
                        parentNode.AddChild(errorNode);
                    }
                }
            }
            else
            {
                var parentNode = GetNode(parent) as ErrorNode;
                if (parentNode != null)
                {
                    var childNode = GetNode(d) as ErrorNode;
                    parentNode.TryRemoveChild(childNode);
                }

                if (GetNode(d) is ScopeNode)
                {
                    SetNode(d, ValidNode.Default);
                }
            }

            d.SetCurrentValue(HasErrorProxyProperty, e.NewValue);
        }

        private static void OnErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(ErrorsProxyProperty, e.NewValue);
        }
    }
}
