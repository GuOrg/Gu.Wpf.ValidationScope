namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows;

    /// <summary>
    /// Manager for the INotifyErrorsChanged.ErrorsChanged event.
    /// Inspired by: http://referencesource.microsoft.com/#WindowsBase/Base/System/Collections/Specialized/CollectionChangedEventManager.cs,7537b339109a7418
    /// </summary>
    internal class ErrorsChangedEventManager : WeakEventManager
    {
        private ErrorsChangedEventManager()
        {
        }

        // get the event manager for the current thread
        private static ErrorsChangedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(ErrorsChangedEventManager);
                var manager = (ErrorsChangedEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new ErrorsChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        /// <summary>Add a listener to the given source's event.</summary>
        public static void AddListener(INotifyErrorsChanged source, IWeakEventListener listener)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(listener, nameof(listener));
            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>Remove a listener to the given source's event.</summary>
        public static void RemoveListener(INotifyErrorsChanged source, IWeakEventListener listener)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(listener, nameof(listener));
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        /// <summary>Add a handler for the given source's event.</summary>
        public static void AddHandler(INotifyErrorsChanged source, EventHandler<ErrorsChangedEventArgs> handler)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(handler, nameof(handler));
            CurrentManager.ProtectedAddHandler(source, handler);
        }

        /// <summary>Remove a handler for the given source's event.</summary>
        public static void RemoveHandler(INotifyErrorsChanged source, EventHandler<ErrorsChangedEventArgs> handler)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(handler, nameof(handler));
            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        /// <inheritdoc/>
        protected override ListenerList NewListenerList()
        {
            return new ListenerList<ErrorsChangedEventArgs>();
        }

        /// <inheritdoc/>
        protected override void StartListening(object source)
        {
            var typedSource = (INotifyErrorsChanged)source;
            typedSource.ErrorsChanged += this.OnErrorsChanged;
        }

        /// <inheritdoc/>
        protected override void StopListening(object source)
        {
            var typedSource = (INotifyErrorsChanged)source;
            typedSource.ErrorsChanged -= this.OnErrorsChanged;
        }

        // event handler for CollectionChanged event
        private void OnErrorsChanged(object sender, ErrorsChangedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }
}