namespace Gu.Wpf.ValidationScope
{
    using System.Windows.Controls;

    public struct ValidationErrorChange
    {
        internal readonly ValidationError Error;
        internal readonly int index;
        internal readonly ValidationErrorEventAction Action;

        public ValidationErrorChange(ValidationError error, int index, ValidationErrorEventAction action)
        {
            Ensure.NotNull(error, nameof(error));
            this.Error = error;
            this.index = index;
            this.Action = action;
        }
    }
}