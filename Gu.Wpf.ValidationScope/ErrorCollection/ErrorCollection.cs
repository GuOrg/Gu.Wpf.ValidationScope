namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    internal class ErrorCollection : ObservableBatchCollection<ValidationError>
    {
        public ErrorCollection()
            : base()
        {
        }

        public ErrorCollection(IEnumerable<ValidationError> errors)
            : base(errors)
        {
        }
    }
}
