namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;

    using NUnit.Framework;

    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;

    public class OneWayToSourceBindingsViewTests
    {
        [Test]
        public void Updates()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIDs.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(AutomationIDs.OneWayToSourceBindingTab);
                page.Select();
                CollectionAssert.IsEmpty(page.Get<GroupBox>(AutomationIDs.ViewErrorsGroupBox).GetErrors());
                CollectionAssert.IsEmpty(page.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).GetErrors());
                Assert.AreEqual(false, page.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).Get<CheckBox>(AutomationIDs.HasErrorsBox).Checked);

                var textBox1 = page.Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.EnterSingle('a');
                var expectedErrors = new[] { "Value 'a' could not be converted." };
                CollectionAssert.AreEqual(expectedErrors, page.Get<GroupBox>(AutomationIDs.ViewErrorsGroupBox).GetErrors());
                CollectionAssert.AreEqual(expectedErrors, page.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).GetErrors());
                Assert.AreEqual(true, page.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).Get<CheckBox>(AutomationIDs.HasErrorsBox).Checked);

                var textBox2 = page.Get<TextBox>(AutomationIDs.TextBox2);
                textBox2.EnterSingle('b');
                expectedErrors = new[]
                {
                    "Value 'a' could not be converted.",
                    "Value 'b' could not be converted."
                };
                CollectionAssert.AreEqual(expectedErrors, page.Get<GroupBox>(AutomationIDs.ViewErrorsGroupBox).GetErrors());
                CollectionAssert.AreEqual(expectedErrors, page.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).GetErrors());
                Assert.AreEqual(true, page.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).Get<CheckBox>(AutomationIDs.HasErrorsBox).Checked);

                textBox1.EnterSingle('1');
                CollectionAssert.IsEmpty(page.Get<GroupBox>(AutomationIDs.ViewErrorsGroupBox).GetErrors());
                CollectionAssert.IsEmpty(page.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).GetErrors());
                Assert.AreEqual(false, page.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).Get<CheckBox>(AutomationIDs.HasErrorsBox).Checked);
            }
        }
    }
}