namespace Gu.Wpf.ValidationScope.Demo
{
    using System.Collections.ObjectModel;

    public class DataGridScopeVm
    {
        public ObservableCollection<ViewModel> Collection { get; } = new ObservableCollection<ViewModel>
                                                                         {
                                                                             new ViewModel(),
                                                                             new ViewModel()
                                                                         };
    }
}