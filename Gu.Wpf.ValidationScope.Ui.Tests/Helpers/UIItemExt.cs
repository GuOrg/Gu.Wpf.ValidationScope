using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using TestStack.White.UIItems.TabItems;

    public static class UIItemExt
    {
        public static string ItemStatus(this IUIItem item)
        {
            return (string)item.AutomationElement.GetCurrentPropertyValue(AutomationElementIdentifiers.ItemStatusProperty);
        }

        public static IReadOnlyList<T> GetMultiple<T>(this UIItemContainer contaner, string automationId)
            where T : IUIItem
        {
            return contaner.GetMultiple(SearchCriteria.ByAutomationId(automationId))
                           .OfType<T>()
                           .ToList();
        }

        public static IReadOnlyList<string> GetErrors(this TabPage page)
        {
            return page.GetMultiple<Label>(AutomationIDs.ErrorText)
                       .Select(x => x.Text)
                       .ToList();
        } 
    }
}