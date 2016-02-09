namespace Gu.Wpf.ValidationScope.Tests.ErrorCollection
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    public partial class ErrorCollectionTests
    {
        private static ReadOnlyObservableCollection<ValidationError> Create(int n)
        {
            var errors = new ObservableCollection<ValidationError>();
            for (int i = 0; i < n; i++)
            {
                errors.Add(TestValidationError.Create());
            }

            return new ReadOnlyObservableCollection<ValidationError>(errors);
        }
    }
}
