namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;

    public class ComplicatedScopeWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "ComplicatedScopeWindow";

        [Test]
        public void Updates()
        {
            this.AssertErrors(Enumerable.Empty<string>());
            var textBox1 = this.Window.Get<GroupBox>(AutomationIDs.TextBoxScope).Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.EnterSingle('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            this.AssertErrors(expectedErrors);

            var textBox2 = this.Window.Get<GroupBox>(AutomationIDs.TextBoxScope).Get<TextBox>(AutomationIDs.TextBox2);
            textBox2.EnterSingle('b');
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            this.AssertErrors(expectedErrors);

            var comboBox1 = this.Window.Get<GroupBox>(AutomationIDs.TextBoxScope).Get<ComboBox>(AutomationIDs.ComboBox1);
            comboBox1.EnterSingle('c');
            this.AssertErrors(expectedErrors);

            var comboBox2 = this.Window.Get<GroupBox>(AutomationIDs.ComboBoxScope).Get<ComboBox>(AutomationIDs.ComboBox1);
            comboBox2.EnterSingle('d');
            expectedErrors = new[]
                                 {
                                     "Value 'a' could not be converted.",
                                     "Value 'b' could not be converted.",
                                     "Value 'd' could not be converted."
                                 };
            this.AssertErrors(expectedErrors);

            var textBox3 = this.Window.Get<GroupBox>(AutomationIDs.ComboBoxScope).Get<TextBox>(AutomationIDs.TextBox1);
            textBox3.EnterSingle('e');
            this.AssertErrors(expectedErrors);

            textBox1.EnterSingle('1');
            this.AssertErrors(Enumerable.Empty<string>());
        }

        private void AssertErrors(IEnumerable<string> expected)
        {
            var nodeErrors = this.Window.GetByText<GroupBox>("Node").GetErrors();
            CollectionAssert.AreEqual(expected, nodeErrors);

            var errors = this.Window.GetByText<GroupBox>("Errors").GetErrors();
            CollectionAssert.AreEqual(expected, errors);
        }
    }
}