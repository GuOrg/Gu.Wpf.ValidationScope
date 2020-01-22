// ReSharper disable StaticMemberInGenericType
// ReSharper disable PossibleMultipleEnumeration
namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    internal class ObservableBatchCollection<T> : ObservableCollection<T>
    {
        private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
        private static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
        private static readonly NotifyCollectionChangedEventArgs NotifyCollectionResetEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        internal void AddRange(IEnumerable<T> items)
        {
            switch (EmptyOneOrMany(items))
            {
                case EmptyOneOrMore.Empty:
                    return;
                case EmptyOneOrMore.One:
                    this.Add(items.Single());
                    break;
                case EmptyOneOrMore.Many:
                    foreach (var item in items)
                    {
                        this.Items.Add(item);
                    }

                    this.RaiseReset();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(items), items, "Did not find a match.");
            }
        }

        internal void RemoveRange(IEnumerable<T> items)
        {
            switch (EmptyOneOrMany(items))
            {
                case EmptyOneOrMore.Empty:
                    return;
                case EmptyOneOrMore.One:
                    this.Remove(items.Single());
                    break;
                case EmptyOneOrMore.Many:
                    foreach (var item in items)
                    {
                        this.Items.Remove(item);
                    }

                    this.RaiseReset();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(items), items, "Did not find a match.");
            }
        }

        private static EmptyOneOrMore EmptyOneOrMany(IEnumerable<T> items)
        {
            if (items is null)
            {
                return EmptyOneOrMore.Empty;
            }

            int count = 0;
            if (items is IReadOnlyList<T> list)
            {
                count = list.Count;
            }
            else
            {
                foreach (var _ in items)
                {
                    count++;
                    if (count > 1)
                    {
                        break;
                    }
                }
            }

            if (count == 0)
            {
                return EmptyOneOrMore.Empty;
            }

            if (count == 1)
            {
                return EmptyOneOrMore.One;
            }

            return EmptyOneOrMore.Many;
        }

        private void RaiseReset()
        {
            this.OnPropertyChanged(CountPropertyChangedEventArgs);
            this.OnPropertyChanged(IndexerPropertyChangedEventArgs);
            this.OnCollectionChanged(NotifyCollectionResetEventArgs);
        }

#pragma warning disable SA1201 // I want it here
        private enum EmptyOneOrMore
        {
            Empty,
            One,
            Many,
        }
    }
}
