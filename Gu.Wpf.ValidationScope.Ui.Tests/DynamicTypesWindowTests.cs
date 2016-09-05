namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.WindowsAPI;

    public class DynamicTypesWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "DynamicTypesWindow";

        [Test]
        public void Updates()
        {
            var childCountBlock = this.Window.Get<Label>(AutomationIDs.ChildCountTextBlock);
            var typesListBox = this.Window.Get<ListBox>(AutomationIDs.TypeListBox);
            Assert.AreEqual(null, typesListBox.SelectedItem);
            Assert.AreEqual(string.Empty, childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());

            var textBox1 = this.Window.Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.Enter('a');
            Assert.AreEqual(string.Empty, childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());

            typesListBox.Select(0);
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            typesListBox.Select(1);
            Assert.AreEqual(string.Empty, childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());

            var comboBox1 = this.Window.Get<ComboBox>(AutomationIDs.ComboBox1);
            comboBox1.Enter('b');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'b' could not be converted." }, this.Window.GetErrors());

            this.Window.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            typesListBox.Items[0].Click();
            this.Window.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
            Assert.AreEqual("Children: 2", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'b' could not be converted.", "Value 'a' could not be converted." }, this.Window.GetErrors());

            textBox1.Text = "2";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            this.Window.WaitWhileBusy();
            Assert.AreEqual("", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }
    }
}