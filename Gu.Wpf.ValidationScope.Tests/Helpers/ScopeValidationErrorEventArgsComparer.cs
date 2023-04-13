namespace Gu.Wpf.ValidationScope.Tests;

internal sealed class ScopeValidationErrorEventArgsComparer : GenericComparer<ScopeValidationErrorEventArgs>
{
    internal static readonly ScopeValidationErrorEventArgsComparer Default = new();

    protected override int Compare(ScopeValidationErrorEventArgs x, ScopeValidationErrorEventArgs y)
    {
        if (!ReferenceEquals(x.Error, y.Error))
        {
            return -1;
        }

        if (x.Action != y.Action)
        {
            return -1;
        }

        return 0;
    }
}
