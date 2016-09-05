namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class ScopeIsErrorScopeWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "ScopeIsErrorScopeWindow";

        [Test]
        public void Updates1()
        {
            var childCountBlock = this.Window.Get<Label>(AutomationIDs.ChildCountTextBlock);

            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
            var textBox1 = this.Window.Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.Enter('a');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            var hasErrorBox = this.Window.Get<CheckBox>(AutomationIDs.HasErrorsBox);
            hasErrorBox.Checked = true;
            var expectedErrors = new[]
            {
                    "Value 'a' could not be converted.",
                    "INotifyDataErrorInfo error"
                };
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            hasErrorBox.Checked = false;
            expectedErrors = new[]
                                 {
                                         "Value 'a' could not be converted.",
                                     };
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            textBox1.Enter('1');
            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }

        [Test]
        public void Updates2()
        {
            var childCountBlock = this.Window.Get<Label>(AutomationIDs.ChildCountTextBlock);

            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
            var textBox1 = this.Window.Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.Enter('a');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            var hasErrorBox = this.Window.Get<CheckBox>(AutomationIDs.HasErrorsBox);
            hasErrorBox.Checked = true;
            var expectedErrors = new[]
            {
                    "Value 'a' could not be converted.",
                    "INotifyDataErrorInfo error"
                };
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            textBox1.Enter('1');
            expectedErrors = new[]
                                 {
                                         "INotifyDataErrorInfo error"
                                     };
            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            hasErrorBox.Checked = false;
            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }
    }
}