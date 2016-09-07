namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Controls;

    internal class ValidNode : Node, IErrorNode
    {
        public static readonly ValidNode Default = new ValidNode();

        private ValidNode()
        {
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            //// ReSharper disable ValueParameterNotUsed
            add { DoNothing(); }
            remove { DoNothing(); }
            //// ReSharper restore ValueParameterNotUsed
        }

        public override bool HasError => false;

        public override ReadOnlyObservableCollection<ValidationError> Errors { get; } = ErrorCollection.EmptyValidationErrors;

        public override ReadOnlyObservableCollection<IErrorNode> Children { get; } = ChildCollection.Empty;

        void IDisposable.Dispose() => DoNothing();

        [Conditional("DEBUG")]
        private static void DoNothing() { /* nop */ }
    }
}