namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows;

    internal class ErrorsChangedEventManager : WeakEventManager
    {
        private readonly EventHandler<ErrorsChangedEventArgs> eventHandler;

        private ErrorsChangedEventManager()
        {
            this.eventHandler = this.DeliverEvent;
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

        internal static void AddListener(INotifyErrorsChanged source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        internal static void RemoveListener(INotifyErrorsChanged source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        internal static void AddHandler(INotifyErrorsChanged source, EventHandler<ErrorsChangedEventArgs> handler)
        {
            CurrentManager.ProtectedAddHandler(source, handler);
        }

        internal static void RemoveHandler(INotifyErrorsChanged source, EventHandler<ErrorsChangedEventArgs> handler)
        {
            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        protected override ListenerList NewListenerList()
        {
            return new ListenerList<ErrorsChangedEventArgs>();
        }

        protected override void StartListening(object source)
        {
            var typedSource = (INotifyErrorsChanged)source;
            typedSource.ErrorsChanged += this.eventHandler;
        }

        protected override void StopListening(object source)
        {
            var typedSource = (INotifyErrorsChanged)source;
            typedSource.ErrorsChanged -= this.eventHandler;
        }
    }
}
