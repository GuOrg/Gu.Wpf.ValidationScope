namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Specialized;

    internal static class BatchItemChangeActionExt
    {
        public static NotifyCollectionChangedAction AsCollectionChangedAction(this BatchItemChangeAction action)
        {
            switch (action)
            {
                case BatchItemChangeAction.Add:
                    return NotifyCollectionChangedAction.Add;
                case BatchItemChangeAction.Remove:
                    return NotifyCollectionChangedAction.Remove;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}