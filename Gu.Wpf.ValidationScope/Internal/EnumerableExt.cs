namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    internal static class EnumerableExt
    {
        internal static IEnumerable<ValidationError> Except(
            this IEnumerable<ValidationError> first,
            IEnumerable<ValidationError> other)
        {
            if (ReferenceEquals(first, ErrorCollection.EmptyValidationErrors))
            {
                return first;
            }

            if (ReferenceEquals(other, ErrorCollection.EmptyValidationErrors))
            {
                return first;
            }

            return first.Except(other);
        }
    }
}
