// ReSharper disable PossibleMultipleEnumeration
namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;

    internal static class EnumerableExt
    {
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
