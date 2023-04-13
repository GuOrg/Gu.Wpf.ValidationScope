namespace Gu.Wpf.ValidationScope.Tests;

using System.Linq;

internal sealed class ErrorsChangedEventArgsComparer : GenericComparer<ErrorsChangedEventArgs>
{
    internal static readonly ErrorsChangedEventArgsComparer Default = new();

    protected override int Compare(ErrorsChangedEventArgs x, ErrorsChangedEventArgs y)
    {
        if (!x.Added.SequenceEqual(y.Added))
        {
            return -1;
        }

        if (!x.Removed.SequenceEqual(y.Removed))
        {
            return -1;
        }

        return 0;
    }
}