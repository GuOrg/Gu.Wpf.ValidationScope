namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class TwoLevelScopeWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "TwoLevelScopeWindow";

        [Test]
        public void Updates()
        {
            var childCountBlock = this.Window.Get<Label>(AutomationIDs.ChildCountTextBlock);

            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
            var textBox1 = this.Window.Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.EnterSingle('a');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            var textBox2 = this.Window.Get<TextBox>(AutomationIDs.TextBox2);
            textBox2.EnterSingle('b');
            var expectedErrors = new[]
            {
                    "Value 'a' could not be converted.",
                    "Value 'b' could not be converted."
                };
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());
            textBox1.EnterSingle('1');
            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }
    }
}