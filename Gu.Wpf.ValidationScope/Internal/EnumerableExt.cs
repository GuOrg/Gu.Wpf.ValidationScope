// ReSharper disable PossibleMultipleEnumeration
namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;

    internal static class EnumerableExt
    {
        internal static IEnumerable<ValidationError> Except(this IEnumerable<ValidationError> first, IEnumerable<ValidationError> other)
        {
            if (ReferenceEquals(first, ErrorCollection.EmptyValidationErrors))
            {
                return first;
            }

            if (ReferenceEquals(other, ErrorCollection.EmptyValidationErrors))
            {
                return first;
            }

            if (Enumerable.SequenceEqual(first, other))
            {
                return ErrorCollection.EmptyValidationErrors;
            }

            return Enumerable.Except(first, other);
        }

        internal static IEnumerable<ValidationError> Concat(this IEnumerable<ValidationError> first, IEnumerable<ValidationError> other)
        {
            if (ReferenceEquals(first, ErrorCollection.EmptyValidationErrors))
            {
                return other;
            }

            if (ReferenceEquals(other, ErrorCollection.EmptyValidationErrors))
            {
                return first;
            }

            return Enumerable.Concat(first, other);
        }

        internal static IReadOnlyList<ValidationError> AsReadOnly(this IEnumerable<ValidationError> source)
        {
            if (!source.Any())
            {
                return ErrorCollection.EmptyValidationErrors;
            }

            if (source is ValidationError[] array)
            {
                return array;
            }

            return source.ToArray();
        }
    }
}
