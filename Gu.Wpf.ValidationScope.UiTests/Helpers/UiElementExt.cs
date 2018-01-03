namespace Gu.Wpf.ValidationScope.UiTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Gu.Wpf.UiAutomation;

    public static class UiElementExt
    {
        public static IReadOnlyList<TextBlock> FindTextBlocks(this UiElement container, string automationId)
        {
            return container.FindAllDescendants(
                                new AndCondition(
                                    Conditions.ByClassName("TextBlock"),
                                    Conditions.ByNameOrAutomationId(automationId)))
                            .Cast<TextBlock>()
                            .ToList();
        }

        public static IReadOnlyList<TextBlock> FindTextBlocks(this UiElement container)
        {
            return container.FindAllDescendants(Conditions.ByClassName("TextBlock"))
                            .Cast<TextBlock>()
                            .ToList();
        }

        public static IReadOnlyList<TextBox> FindTextBoxes(this UiElement container)
        {
            return container.FindAllDescendants(Conditions.ByClassName("TextBox"))
                            .Cast<TextBox>()
                            .ToList();
        }

        public static IReadOnlyList<string> GetErrors(this UiElement container)
        {
            return container.FindTextBlocks("ErrorTextBlock")
                            .Select(x => x.Text)
                            .ToList();
        }

        public static IReadOnlyList<string> GetChildren(this UiElement container)
        {
            return container.FindTextBlocks("ChildTextBlock")
                            .Select(x => x.Text)
                            .ToList();
        }
    }
}
