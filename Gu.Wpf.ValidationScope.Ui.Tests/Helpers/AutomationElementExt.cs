namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Gu.Wpf.UiAutomation;

    public static class AutomationElementExt
    {
        public static IReadOnlyList<TextBlock> FindTextBlocks(this AutomationElement container, string automationId)
        {
            return container.FindAllDescendants(
                                x => new AndCondition(
                                    x.ByClassName("TextBlock"),
                                    new OrCondition(
                                        x.ByAutomationId(automationId),
                                        x.ByName(automationId))))
                            .Select(x => x.AsTextBlock())
                            .ToList();
        }

        public static IReadOnlyList<TextBlock> FindTextBlocks(this AutomationElement container)
        {
            return container.FindAllDescendants(x => x.ByClassName("TextBlock"))
                            .Select(x => x.AsTextBlock())
                            .ToList();
        }

        public static IReadOnlyList<TextBox> FindTextBoxes(this AutomationElement container)
        {
            return container.FindAllDescendants(x => x.ByClassName("TextBox"))
                            .Select(x => x.AsTextBox())
                            .ToList();
        }

        public static IReadOnlyList<string> GetErrors(this AutomationElement container)
        {
            return container.FindTextBlocks("ErrorTextBlock")
                            .Select(x => x.Text)
                            .ToList();
        }

        public static IReadOnlyList<string> GetChildren(this AutomationElement container)
        {
            return container.FindTextBlocks("ChildTextBlock")
                            .Select(x => x.Text)
                            .ToList();
        }
    }
}