namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows;

    public static partial class Scope
    {
        /// <summary>
        /// Notifies when validation errors occurs in the scope.
        /// Bindings does not need NotifyOnValidationError=True for this event to fire.
        /// </summary>
        public static readonly RoutedEvent ValidationErrorEvent = EventManager.RegisterRoutedEvent(
            "ValidationError",
            RoutingStrategy.Direct,
            typeof(EventHandler<ScopeValidationErrorEventArgs>),
            typeof(Scope));

        /// <summary>
        /// Notifies when validation errors occurs in the scope.
        /// Bindings does not need NotifyOnValidationError=True for this event to fire.
        /// </summary>
        public static readonly RoutedEvent ErrorsChangedEvent = EventManager.RegisterRoutedEvent(
            "ErrorsChanged",
            RoutingStrategy.Direct,
            typeof(EventHandler<ErrorsChangedEventArgs>),
            typeof(Scope));

        /// <summary>Adds a handler for the Scope.ValidationError attached event.</summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddErrorHandler(DependencyObject element, EventHandler<ScopeValidationErrorEventArgs> handler)
        {
            (element as UIElement)?.AddHandler(ValidationErrorEvent, handler);
            (element as ContentElement)?.AddHandler(ValidationErrorEvent, handler);
        }

        /// <summary>Removes a handler for the Scope.ValidationError attached event.</summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveErrorHandler(DependencyObject element, EventHandler<ScopeValidationErrorEventArgs> handler)
        {
            (element as UIElement)?.RemoveHandler(ValidationErrorEvent, handler);
            (element as ContentElement)?.RemoveHandler(ValidationErrorEvent, handler);
        }

        /// <summary>Adds a handler for the Scope.ErrorsChanged attached event.</summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddErrorsChangedHandler(this DependencyObject element, EventHandler<ErrorsChangedEventArgs> handler)
        {
            (element as UIElement)?.AddHandler(ValidationErrorEvent, handler);
            (element as ContentElement)?.AddHandler(ValidationErrorEvent, handler);
        }

        /// <summary>Removes a handler for the Scope.ErrorsChanged attached event.</summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveErrorsChangedHandler(this DependencyObject element, EventHandler<ErrorsChangedEventArgs> handler)
        {
            (element as UIElement)?.RemoveHandler(ValidationErrorEvent, handler);
            (element as ContentElement)?.RemoveHandler(ValidationErrorEvent, handler);
        }
    }
}
