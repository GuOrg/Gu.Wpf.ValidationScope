namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.WindowsAPI;

    public class NotifyDataErrorInfoWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "NotifyDataErrorInfoWindow";

        [Test]
        public void Updates()
        {
            var childCountBlock = this.Window.Get<Label>(AutomationIDs.ChildCountTextBlock);

            Assert.AreEqual(string.Empty, childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
            var textBox1 = this.Window.Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.EnterSingle('a');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            var textBox2 = this.Window.Get<TextBox>(AutomationIDs.TextBox2);
            textBox2.EnterSingle('b');
            var expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("Children: 2", childCountBlock.Text);
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            var hasErrorBox = this.Window.Get<CheckBox>(AutomationIDs.HasErrorsBox);
            hasErrorBox.Checked = true;
            expectedErrors = new[]
                                 {
                                     "Value 'a' could not be converted.",
                                     "Value 'b' could not be converted.",
                                     "INotifyDataErrorInfo error"
                                 };
            Assert.AreEqual("Children: 3", childCountBlock.Text);
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            hasErrorBox.Checked = false;
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted.", };
            Assert.AreEqual("Children: 2", childCountBlock.Text);
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            textBox1.BulkText = "2";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(string.Empty, childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }
    }
}