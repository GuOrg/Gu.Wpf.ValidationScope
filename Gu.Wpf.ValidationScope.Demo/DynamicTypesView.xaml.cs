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
            InitializeComponent();
        }

        private void ListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = ((ListBox)sender).SelectedItems.Cast<Type>();
            var types = new InputTypeCollection();
            types.AddRange(selectedItems);
            Scope.SetForInputTypes(this.Form, types);
        }
    }
}
