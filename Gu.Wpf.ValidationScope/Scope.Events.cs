namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows;

    public static partial class Scope
    {
        /// <summary>The error event is raised even if the bindings does not notify.</summary>
        public static readonly RoutedEvent ErrorEvent = EventManager.RegisterRoutedEvent(
            "ValidationError",
            RoutingStrategy.Direct,
            typeof(EventHandler<ScopeValidationErrorEventArgs>),
            typeof(Scope));

        public static readonly RoutedEvent ErrorsChangedEvent = EventManager.RegisterRoutedEvent(
            "ErrorsChanged",
            RoutingStrategy.Direct,
            typeof(EventHandler<ErrorsChangedEventArgs>),
            typeof(Scope));

        /// <summary>
        ///     Adds a handler for the ValidationError attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddErrorHandler(DependencyObject element, EventHandler<ScopeValidationErrorEventArgs> handler)
        {
            (element as UIElement)?.AddHandler(ErrorEvent, handler);
            (element as ContentElement)?.AddHandler(ErrorEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the ValidationError attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveErrorHandler(DependencyObject element, EventHandler<ScopeValidationErrorEventArgs> handler)
        {
            (element as UIElement)?.RemoveHandler(ErrorEvent, handler);
            (element as ContentElement)?.RemoveHandler(ErrorEvent, handler);
        }

        public static void AddErrorsChangedHandler(this DependencyObject element, EventHandler<ErrorsChangedEventArgs> handler)
        {
            (element as UIElement)?.AddHandler(ErrorEvent, handler);
            (element as ContentElement)?.AddHandler(ErrorEvent, handler);
        }

        public static void RemoveErrorsChangedHandler(this DependencyObject element, EventHandler<ErrorsChangedEventArgs> handler)
        {
            (element as UIElement)?.RemoveHandler(ErrorEvent, handler);
            (element as ContentElement)?.RemoveHandler(ErrorEvent, handler);
        }
    }
}