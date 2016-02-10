namespace Gu.Wpf.ValidationScope.Demo
{
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Controls;

    public static class AutomationIDs
    {
        public static readonly DependencyProperty TextBoxIdProperty = DependencyProperty.RegisterAttached(
            "TextBoxId",
            typeof(string),
            typeof(AutomationIDs),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits, OnTextBoxIdChanged));

        public static readonly string MainWindow = Create();
        public static readonly string OneLevelScopeTab = Create();
        public static readonly string TwoLevelScopeTab = Create();
        public static readonly string ScopeWithDataTemplatesTab = Create();
        public static readonly string ScopeWithControlTemplatesTab = Create();
        public static readonly string ComplicatedScopeTab = Create();
        public static readonly string DynamicScopeTab = Create();
        public static readonly string DataGridScopeTab = Create();
        public static readonly string NotifyDataErrorInfoTab = Create();
        public static readonly string OneWayToSourceBindingTab = Create();
        public static readonly string ParentChildScopesTab = Create();

        public static readonly string TextBoxScope = Create();
        public static readonly string ComboBoxScope = Create();
        public static readonly string TextBox1 = Create();
        public static readonly string TextBox2 = Create();
        public static readonly string ComboBox1 = Create();
        public static readonly string HasErrorsBox = Create();
        public static readonly string TypeListBox = Create();

        public static readonly string ErrorList = Create();
        public static readonly string ErrorText = Create();


        public static void SetTextBoxId(this UIElement element, string value)
        {
            element.SetValue(TextBoxIdProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static string GetTextBoxId(this UIElement element)
        {
            return (string)element.GetValue(TextBoxIdProperty);
        }
        private static void OnTextBoxIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                AutomationProperties.SetAutomationId(textBox, (string)e.NewValue);
            }
        }

        private static string Create([CallerMemberName] string name = null)
        {
            return name;
        }
    }
}
