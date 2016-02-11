namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;

    public class ScopeIsErrorScopeTests
    {
        [Test]
        public void Updates1()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIDs.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(AutomationIDs.ScopeIsErrorScopeTab);
                page.Select();
                var childCountBlock = page.Get<Label>(AutomationIDs.ChildCountTextBlock);

                Assert.AreEqual("Children: 0", childCountBlock.Text);
                CollectionAssert.IsEmpty(page.GetErrors());
                var textBox1 = page.Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.EnterSingle('a');
                Assert.AreEqual("Children: 1", childCountBlock.Text);
                CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, page.GetErrors());

                var hasErrorBox = page.Get<CheckBox>(AutomationIDs.HasErrorsBox);
                hasErrorBox.Checked = true;
                var expectedErrors = new[]
                {
                    "Value 'a' could not be converted.",
                    "INotifyDataErrorInfo error"
                };
                Assert.AreEqual("Children: 1", childCountBlock.Text);
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());

                hasErrorBox.Checked = false;
                expectedErrors = new[]
                                     {
                                         "Value 'a' could not be converted.",
                                     };
                Assert.AreEqual("Children: 1", childCountBlock.Text);
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());

                textBox1.EnterSingle('1');
                Assert.AreEqual("Children: 0", childCountBlock.Text);
                CollectionAssert.IsEmpty(page.GetErrors());
            }
        }

        [Test]
        public void Updates2()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIDs.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(AutomationIDs.ScopeIsErrorScopeTab);
                page.Select();
                var childCountBlock = page.Get<Label>(AutomationIDs.ChildCountTextBlock);

                Assert.AreEqual("Children: 0", childCountBlock.Text);
                CollectionAssert.IsEmpty(page.GetErrors());
                var textBox1 = page.Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.EnterSingle('a');
                Assert.AreEqual("Children: 1", childCountBlock.Text);
                CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, page.GetErrors());

                var hasErrorBox = page.Get<CheckBox>(AutomationIDs.HasErrorsBox);
                hasErrorBox.Checked = true;
                var expectedErrors = new[]
                {
                    "Value 'a' could not be converted.",
                    "INotifyDataErrorInfo error"
                };
                Assert.AreEqual("Children: 1", childCountBlock.Text);
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());

                textBox1.EnterSingle('1');
                expectedErrors = new[]
                                     {
                                         "INotifyDataErrorInfo error"
                                     };
                Assert.AreEqual("Children: 0", childCountBlock.Text);
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());

                hasErrorBox.Checked = false;
                Assert.AreEqual("Children: 0", childCountBlock.Text);
                CollectionAssert.IsEmpty(page.GetErrors());
            }
        }
    }
}