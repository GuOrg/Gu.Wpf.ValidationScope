namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    using TestStack.White;
    using TestStack.White.InputDevices;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.UIItems.WindowItems;

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

        public static void Enter(this TextBox textBox, char @char)
        {
            textBox.Click();
            textBox.DoubleClick();
            Keyboard.Instance.Send(new string(@char, 1), textBox.ActionListener);
            WindowTests.StaticWindow?.WaitWhileBusy();
        }

        public static void Enter(this ComboBox comboBox, char @char)
        {
            comboBox.Click();
            comboBox.DoubleClick();
            Keyboard.Instance.Send(new string(@char, 1), comboBox.ActionListener);
            WindowTests.StaticWindow?.WaitWhileBusy();
        }

        public static IEnumerable<IUIItem> Ancestors(this IUIItem item)
        {
            var parent = item.GetParent<IUIItemContainer>();
            while (parent != null)
            {
                yield return parent;
                parent = parent.GetParent<IUIItemContainer>();
            }
        }
    }
}