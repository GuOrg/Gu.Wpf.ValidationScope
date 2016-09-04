namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Controls;

    internal class ValidNode : Node, IErrorNode
    {
        public static readonly ValidNode Default = new ValidNode();

        private ValidNode()
        {
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { DoNothing(); }
            remove { DoNothing(); }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { DoNothing(); }
            remove { DoNothing(); }
        }

        public override bool HasErrors => false;

        public override ReadOnlyObservableCollection<ValidationError> Errors { get; } = ErrorCollection.EmptyValidationErrors;

        public override ReadOnlyObservableCollection<IErrorNode> Children { get; } = new ReadOnlyObservableCollection<IErrorNode>(new ObservableCollection<IErrorNode>());

        int IReadOnlyCollection<ValidationError>.Count => this.Errors.Count;

        ValidationError IReadOnlyList<ValidationError>.this[int index] => this.Errors[index];

        IEnumerator<ValidationError> IEnumerable<ValidationError>.GetEnumerator() => this.Errors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.Errors.GetEnumerator();

        void IDisposable.Dispose() => DoNothing();

        [Conditional("DEBUG")]
        private static void DoNothing()
        {
            Debug.Assert(false, "Should never be called");
        }
    }
}