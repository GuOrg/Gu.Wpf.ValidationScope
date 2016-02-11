namespace Gu.Wpf.ValidationScope.Ui.Tests
{
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
                var page = window.Get<TabPage>(AutomationIDs.TwoLevelScopeTab);
                page.Select();
                var childCountBlock = page.Get<Label>(AutomationIDs.ChildCountTextBlock);

                Assert.AreEqual(string.Empty, childCountBlock.Text);
                CollectionAssert.IsEmpty(page.GetErrors());
                var textBox1 = page.Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.EnterSingle('a');
                Assert.AreEqual("Children: 1", childCountBlock.Text);
                CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, page.GetErrors());

                var textBox2 = page.Get<TextBox>(AutomationIDs.TextBox2);
                textBox2.EnterSingle('b');
                var expectedErrors = new[]
                {
                    "Value 'a' could not be converted.",
                    "Value 'b' could not be converted."
                };
                Assert.AreEqual("Children: 1", childCountBlock.Text);
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());
                textBox1.EnterSingle('1');
                Assert.AreEqual(string.Empty, childCountBlock.Text);
                CollectionAssert.IsEmpty(page.GetErrors());
            }
        }
    }
}