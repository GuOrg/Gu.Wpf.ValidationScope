namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Gu.Wpf.ValidationScope.Demo;

    using TestStack.White.InputDevices;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;
    using TestStack.White.UIItems.ListBoxItems;

    public static class UIItemExt
    {
        public static string ItemStatus(this IUIItem item)
        {
            return (string)item.AutomationElement.GetCurrentPropertyValue(AutomationElementIdentifiers.ItemStatusProperty);
        }

        public static IReadOnlyList<T> GetMultiple<T>(this UIItemContainer container, string automationId)
            where T : IUIItem
        {
            return container.GetMultiple(SearchCriteria.ByAutomationId(automationId))
                           .OfType<T>()
                           .ToList();
        }

        public static T GetByText<T>(this UIItemContainer container, string text)
            where T : UIItem
        {
            return container.Get<T>(SearchCriteria.ByText(text));
        }

        public static T GetByIndex<T>(this UIItemContainer container, int index)
            where T : UIItem
        {
            return container.Get<T>(SearchCriteria.Indexed(index));
        }

        public static IReadOnlyList<string> GetErrors(this UIItemContainer container)
        {
            return container.GetMultiple<Label>("ErrorTextBlock")
                            .Select(x => x.Text)
                            .ToList();
        }

        public static void EnterSingle(this TextBox textBox, char @char)
        {
            textBox.DoubleClick();
            Keyboard.Instance.Send(new string(@char, 1), textBox.ActionListener);
        }

        public static void EnterSingle(this ComboBox comboBox, char @char)
        {
            comboBox.DoubleClick();
            Keyboard.Instance.Send(new string(@char, 1), comboBox.ActionListener);
        }

        public static IEnumerable<IUIItem> Ancestors(this IUIItem item)
        {
            var parent = item.GetParent<IUIItem>();
            while (parent != null)
            {
                yield return parent;
                parent = parent.GetParent<IUIItem>();
            }
        }
    }
}