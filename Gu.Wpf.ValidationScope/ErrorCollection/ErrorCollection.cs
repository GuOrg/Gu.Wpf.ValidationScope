namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Windows.Controls;

    internal class ErrorCollection : ObservableBatchCollection<ValidationError>
    {
        public ErrorCollection()
        {
        }

        public ErrorCollection(IEnumerable<ValidationError> errors)
            : base(errors)
        {
        }

        protected override void NotifyBatch()
        {
            var args = this.GetMergedCollectionChangedEventArgs();
            if (args == null)
            {
                return;
            }

            this.Notify(args);
        }

        private void Notify(NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    this.OnPropertyChanged(CountPropertyChangedEventArgs);
                    this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
                    this.OnCollectionChanged(args);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.OnPropertyChanged(CountPropertyChangedEventArgs);
                    this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
                    this.OnCollectionChanged(new ErrorCollectionResetEventArgs(this.Changes));
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
                    this.Notify(args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
