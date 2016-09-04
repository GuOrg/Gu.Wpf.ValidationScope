namespace Gu.Wpf.ValidationScope.Tests
{
    internal class ScopeValidationErrorEventArgsComparer : GenericComparer<ScopeValidationErrorEventArgs>
    {
        public static readonly ScopeValidationErrorEventArgsComparer Default = new ScopeValidationErrorEventArgsComparer();

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
}