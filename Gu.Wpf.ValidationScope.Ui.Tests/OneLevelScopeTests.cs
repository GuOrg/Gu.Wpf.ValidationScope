namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;

    using Gu.Wpf.ValidationScope.Demo;

    public class OneLevelScopeTests
    {
        [Test]
        public void Updates()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIDs.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(AutomationIDs.OneLevelScopeTab);
                page.Select();
                var childCountBlock = page.Get<Label>(AutomationIDs.ChildCountTextBlock);

                CollectionAssert.IsEmpty(page.GetErrors());
                Assert.AreEqual(string.Empty, childCountBlock.Text);

                var textBox1 = page.Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.EnterSingle('a');
                CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, page.GetErrors());
                Assert.AreEqual("Children: 1", childCountBlock.Text);

                var textBox2 = page.Get<TextBox>(AutomationIDs.TextBox2);
                textBox2.EnterSingle('b');
                var expectedErrors = new[]
                {
                    "Value 'a' could not be converted.",
                    "Value 'b' could not be converted."
                };
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());
                Assert.AreEqual("Children: 2", childCountBlock.Text);

                textBox1.EnterSingle('1');
                CollectionAssert.IsEmpty(page.GetErrors());
                Assert.AreEqual(string.Empty, childCountBlock.Text);
            }
        }
    }
}
