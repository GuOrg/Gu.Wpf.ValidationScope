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
            if (d is TextBox textBox)
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
