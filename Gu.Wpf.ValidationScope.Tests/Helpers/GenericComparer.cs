namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections;

    internal abstract class GenericComparer<T> : IComparer
    {
        int IComparer.Compare(object? x, object? y)
        {
            if (x is null && y is null)
            {
                return 0;
            }

            if (x is null || y is null)
            {
                return -1;
            }

            if (x is T xt &&
                y is T yt)
            {
                return this.Compare(xt, yt);
            }

            return -1;
        }

        protected abstract int Compare(T x, T y);
    }
}
