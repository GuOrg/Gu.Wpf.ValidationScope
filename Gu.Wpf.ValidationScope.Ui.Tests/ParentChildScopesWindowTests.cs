namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;

    public class ParentChildScopesWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "ParentChildScopesWindow";

        [Test]
        public void Updates()
        {
            CollectionAssert.IsEmpty(this.Window.GetErrors());
            var textBox1 = this.Window.Get<GroupBox>(AutomationIDs.TextBoxScope).Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.Enter('a');
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            var textBox2 = this.Window.Get<GroupBox>(AutomationIDs.TextBoxScope).Get<TextBox>(AutomationIDs.TextBox2);
            textBox2.Enter('b');
            var expectedErrors = new[]
            {
                    "Value 'a' could not be converted.",
                    "Value 'b' could not be converted."
                };
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            var comboBox1 = this.Window.Get<GroupBox>(AutomationIDs.TextBoxScope).Get<ComboBox>(AutomationIDs.ComboBox1);
            comboBox1.Enter('c');
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            var comboBox2 = this.Window.Get<GroupBox>(AutomationIDs.ComboBoxScope).Get<ComboBox>(AutomationIDs.ComboBox1);
            comboBox2.Enter('d');
            expectedErrors = new[]
            {
                    "Value 'a' could not be converted.",
                    "Value 'b' could not be converted.",
                    "Value 'd' could not be converted."
                };
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            var textBox3 = this.Window.Get<GroupBox>(AutomationIDs.ComboBoxScope).Get<TextBox>(AutomationIDs.TextBox1);
            textBox3.Enter('e');
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            textBox1.Enter('1');
            CollectionAssert.IsEmpty(this.Window.GetErrors());

            var textBox4 = this.Window.Get<GroupBox>(AutomationIDs.NoScope).Get<TextBox>(AutomationIDs.TextBox1);
            textBox4.Enter('e');
            CollectionAssert.IsEmpty(this.Window.GetErrors());

            var comboBox3 = this.Window.Get<GroupBox>(AutomationIDs.NoScope).Get<ComboBox>(AutomationIDs.ComboBox1);
            comboBox3.Enter('f');
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }
    }
}
