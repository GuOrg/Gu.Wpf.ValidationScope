namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Linq;
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;

    public class TwoLevelScopeTests
    {
        [Test]
        public void Updates()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIDs.MainWindow, InitializeOption.NoCache);
                var keyboard = window.Keyboard;
                var page = window.Get<TabPage>(AutomationIDs.TwoLevelScopeTab);
                page.Select();
                CollectionAssert.IsEmpty(page.GetErrors());
                var textBox1 = page.Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.Click();
                keyboard.Enter("g");
                CollectionAssert.AreEqual(new[] { "Value '0g' could not be converted." }, page.GetErrors());

                var textBox2 = page.Get<TextBox>(AutomationIDs.TextBox2);
                textBox2.Click();
                keyboard.Enter("h");
                var expectedErrors = new[]
                {
                    "Value '0g' could not be converted.",
                    "Value '0h' could not be converted."
                };
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());
                textBox1.BulkText = "1";
                CollectionAssert.IsEmpty(page.GetErrors());
            }
        }
    }
}