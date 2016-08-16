namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;

    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class OneWayToSourceBindingsWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "OneWayToSourceBindingsWindow";

        [Test]
        public void Updates()
        {
            CollectionAssert.IsEmpty(this.Window.Get<GroupBox>(AutomationIDs.ViewErrorsGroupBox).GetErrors());
            CollectionAssert.IsEmpty(this.Window.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).GetErrors());
            Assert.AreEqual(false, this.Window.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).Get<CheckBox>(AutomationIDs.HasErrorsBox).Checked);

            var textBox1 = this.Window.Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.EnterSingle('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            CollectionAssert.AreEqual(expectedErrors, this.Window.Get<GroupBox>(AutomationIDs.ViewErrorsGroupBox).GetErrors());
            CollectionAssert.AreEqual(expectedErrors, this.Window.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).GetErrors());
            Assert.AreEqual(true, this.Window.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).Get<CheckBox>(AutomationIDs.HasErrorsBox).Checked);

            var textBox2 = this.Window.Get<TextBox>(AutomationIDs.TextBox2);
            textBox2.EnterSingle('b');
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            CollectionAssert.AreEqual(expectedErrors, this.Window.Get<GroupBox>(AutomationIDs.ViewErrorsGroupBox).GetErrors());
            CollectionAssert.AreEqual(expectedErrors, this.Window.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).GetErrors());
            Assert.AreEqual(true, this.Window.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).Get<CheckBox>(AutomationIDs.HasErrorsBox).Checked);

            textBox1.EnterSingle('1');
            CollectionAssert.IsEmpty(this.Window.Get<GroupBox>(AutomationIDs.ViewErrorsGroupBox).GetErrors());
            CollectionAssert.IsEmpty(this.Window.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).GetErrors());
            Assert.AreEqual(false, this.Window.Get<GroupBox>(AutomationIDs.BoundErrorsGroupBox).Get<CheckBox>(AutomationIDs.HasErrorsBox).Checked);
        }
    }
}