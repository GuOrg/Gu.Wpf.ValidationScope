namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using NUnit.Framework;

    using TestStack.White.UIItems;

    using Gu.Wpf.ValidationScope.Demo;

    public class OneLevelScopeWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "OneLevelScopeWindow";

        [Test]
        public void Updates()
        {
            var childCountBlock = this.Window.Get<Label>(AutomationIDs.ChildCountTextBlock);

            CollectionAssert.IsEmpty(this.Window.GetErrors());
            Assert.AreEqual(string.Empty, childCountBlock.Text);

            var textBox1 = this.Window.Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.EnterSingle('a');
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());
            Assert.AreEqual("Children: 1", childCountBlock.Text);

            var textBox2 = this.Window.Get<TextBox>(AutomationIDs.TextBox2);
            textBox2.EnterSingle('b');
            var expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());
            Assert.AreEqual("Children: 2", childCountBlock.Text);

            textBox1.EnterSingle('1');
            CollectionAssert.IsEmpty(this.Window.GetErrors());
            Assert.AreEqual(string.Empty, childCountBlock.Text);
        }
    }
}
