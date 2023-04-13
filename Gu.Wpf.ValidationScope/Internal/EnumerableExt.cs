namespace Gu.Wpf.ValidationScope;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

internal static class EnumerableExt
{
    internal static IReadOnlyList<ValidationError> AsReadOnly(this IEnumerable<ValidationError> source)
    {
        // ReSharper disable PossibleMultipleEnumeration
#pragma warning disable CA1851
        if (!source.Any())
        {
            return ErrorCollection.EmptyValidationErrors;
        }

        if (source is ValidationError[] array)
        {
            return array;
        }

        return source.ToArray();
#pragma warning restore CA1851
    }
}
