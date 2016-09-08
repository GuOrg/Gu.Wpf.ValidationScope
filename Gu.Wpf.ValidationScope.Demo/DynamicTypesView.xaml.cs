namespace Gu.Wpf.ValidationScope.Demo
{
    using System;
    using System.Linq;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DynamicTypesView.xaml
    /// </summary>
    public partial class DynamicTypesView : UserControl
    {
        public DynamicTypesView()
        {
            this.InitializeComponent();
        }

        private void OnTypesListSelectionChanged(object _, SelectionChangedEventArgs __)
        {
            Scope.SetForInputTypes(this.Form, new InputTypeCollection(this.TypeListBox.SelectedItems.Cast<Type>()));
        }
    }
}
