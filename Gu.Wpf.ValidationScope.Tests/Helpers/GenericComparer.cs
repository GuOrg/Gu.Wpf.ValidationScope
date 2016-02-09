namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    public abstract class GenericComparer<T> : IComparer, IComparer<T>
    {
        int IComparer.Compare(object x, object y)
        {
            if (x is T && y is T)
            {
                return this.Compare((T)x, (T)y);
            }

            return -1;
        }

        public abstract int Compare(T x, T y);
    }
}