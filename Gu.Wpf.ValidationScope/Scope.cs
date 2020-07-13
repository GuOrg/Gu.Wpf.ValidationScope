namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>Provides attached properties for validation scopes.</summary>
    public static partial class Scope
    {
#pragma warning disable SA1202 // Elements must be ordered by access
        /// <summary>
        /// Attached property for specifying what input types to track validation for.
        /// </summary>
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

        /// <summary>
        /// Attached property similar to <see cref="Validation.HasErrorProperty"/> but aggregated for the child elements in the scope.
        /// </summary>
        public static readonly DependencyProperty HasErrorProperty = HasErrorPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Errors",
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(Scope),
            new PropertyMetadata(ErrorCollection.EmptyValidationErrors));

        /// <summary>
        /// Attached property similar to <see cref="Validation.ErrorsProperty"/> but aggregated for the child elements in the scope.
        /// </summary>
        public static readonly DependencyProperty ErrorsProperty = ErrorsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey NodePropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Node",
            typeof(Node),
            typeof(Scope),
            new PropertyMetadata(ValidNode.Default, OnNodeChanged));

        /// <summary>
        /// Attached property for the corresponding node.
        /// </summary>
        public static readonly DependencyProperty NodeProperty = NodePropertyKey.DependencyProperty;

        /// <summary>
        /// Attached property for <see cref="OneWayToSourceBindings"/>.
        /// </summary>
        public static readonly DependencyProperty OneWayToSourceBindingsProperty = DependencyProperty.RegisterAttached(
            "OneWayToSourceBindings",
            typeof(OneWayToSourceBindings),
            typeof(Scope),
            new PropertyMetadata(default(OneWayToSourceBindings), OnOneWayToSourceBindingsChanged));

        /// <summary>Helper for setting <see cref="ForInputTypesProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="FrameworkElement"/> to set <see cref="ForInputTypesProperty"/> on.</param>
        /// <param name="value">ForInputTypes property value.</param>
        public static void SetForInputTypes(UIElement element, InputTypeCollection? value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(ForInputTypesProperty, value);
        }

        /// <summary>Helper for getting <see cref="ForInputTypesProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="ForInputTypesProperty"/> from.</param>
        /// <returns>ForInputTypesProperty property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static InputTypeCollection? GetForInputTypes(UIElement element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (InputTypeCollection?)element?.GetValue(ForInputTypesProperty);
        }

        private static void SetHasError(DependencyObject element, bool value) => element.SetValue(HasErrorPropertyKey, BooleanBoxes.Box(value));

        /// <summary>Helper for getting <see cref="HasErrorProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="HasErrorProperty"/> from.</param>
        /// <returns>HasError property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetHasError(UIElement element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(HasErrorProperty);
        }

        private static void SetErrors(this DependencyObject element, ReadOnlyObservableCollection<ValidationError> value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(ErrorsPropertyKey, value);
        }

        /// <summary>Helper for getting <see cref="ErrorsProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ErrorsProperty"/> from.</param>
        /// <returns>Errors property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static ReadOnlyObservableCollection<ValidationError> GetErrors(DependencyObject element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (ReadOnlyObservableCollection<ValidationError>)element.GetValue(ErrorsProperty);
        }

        private static void SetNode(DependencyObject element, Node? value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(NodePropertyKey, value);
        }

        /// <summary>Helper for getting <see cref="NodeProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="NodeProperty"/> from.</param>
        /// <returns>Node property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static Node? GetNode(DependencyObject element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (Node)element.GetValue(NodeProperty);
        }

        /// <summary>Helper for setting <see cref="OneWayToSourceBindingsProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="OneWayToSourceBindingsProperty"/> on.</param>
        /// <param name="value">OneWayToSourceBindings property value.</param>
        public static void SetOneWayToSourceBindings(this UIElement element, OneWayToSourceBindings value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(OneWayToSourceBindingsProperty, value);
        }

        /// <summary>Helper for getting <see cref="OneWayToSourceBindingsProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="OneWayToSourceBindingsProperty"/> from.</param>
        /// <returns>OneWayToSourceBindings property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static OneWayToSourceBindings GetOneWayToSourceBindings(this UIElement element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (OneWayToSourceBindings)element.GetValue(OneWayToSourceBindingsProperty);
        }

#pragma warning restore SA1202 // Elements must be ordered by access

        private static void OnForInputTypesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Adorner)
            {
                return;
            }

            if (e.NewValue is InputTypeCollection newValue)
            {
                if (newValue.Contains(d))
                {
                    var inputNode = GetNode(d) as InputNode;
                    if (inputNode is null)
                    {
#pragma warning disable IDISP001, CA2000 // Dispose created. Disposed in SetNode
                        inputNode = new InputNode((FrameworkElement)d);
#pragma warning restore IDISP001, CA2000 // Dispose created.
                        SetNode(d, inputNode);
                    }
                }
                else
                {
                    // below looks pretty expensive but
                    // a) Not expecting scope to change often.
                    // b) Not expecting many errors often.
                    // optimize if profiler points at it
                    var errorNode = GetNode(d) as ErrorNode;
                    if (errorNode is null)
                    {
                        return;
                    }

                    if (errorNode.ErrorCollection.RemoveAll(x => !IsScopeFor(d, x)) > 0)
                    {
                        if (errorNode.Errors.Count == 0)
                        {
                            SetNode(d, ValidNode.Default);
                        }
                        else
                        {
                            var removeChildren = errorNode.Children.Where(x => !errorNode.Errors.Intersect(x.Errors).Any()).ToArray();
                            foreach (var removeChild in removeChildren)
                            {
                                errorNode.ChildCollection.Remove(removeChild);
                            }
                        }
                    }
                }
            }
            else
            {
                d.SetValue(NodePropertyKey, ValidNode.Default);
            }
        }

        private static void OnNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ErrorNode oldNode)
            {
                ErrorsChangedEventManager.RemoveHandler(oldNode, OnNodeErrorsChanged);
                oldNode.Dispose();
            }

            if (e.NewValue is ErrorNode newNode)
            {
                (newNode as InputNode)?.BindToSource();
                UpdateErrorsAndHasErrors(d, GetErrors(d), newNode.Errors, newNode.Errors);
                ErrorsChangedEventManager.AddHandler(newNode, OnNodeErrorsChanged);
            }
            else
            {
                UpdateErrorsAndHasErrors(d, GetErrors(d), ErrorCollection.EmptyValidationErrors, ErrorCollection.EmptyValidationErrors);
            }
        }

        private static void OnNodeErrorsChanged(object? sender, ErrorsChangedEventArgs e)
        {
            if (sender is ErrorNode { Source: { } source, Errors: { } errors })
            {
                UpdateErrorsAndHasErrors(source, e.Removed, e.Added, errors);
            }
        }

        private static void OnOneWayToSourceBindingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OneWayToSourceBindings)e.OldValue)?.ClearValue(OneWayToSourceBindings.ElementProperty);
            ((OneWayToSourceBindings)e.NewValue)?.SetCurrentValue(OneWayToSourceBindings.ElementProperty, d as UIElement);
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
