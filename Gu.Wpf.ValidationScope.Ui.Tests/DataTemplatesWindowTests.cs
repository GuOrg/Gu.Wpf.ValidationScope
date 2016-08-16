namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;

    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class DataTemplatesWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "DataTemplatesWindow";

        [Test]
        public void Updates()
        {
            CollectionAssert.IsEmpty(this.Window.GetErrors());
            var textBox1 = this.Window.Get<TextBox>(AutomationIDs.TextBox1);
            textBox1.EnterSingle('a');
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            var textBox2 = this.Window.Get<TextBox>(AutomationIDs.TextBox2);
            textBox2.EnterSingle('b');
            var expectedErrors = new[]
                                     {
                                             "Value 'a' could not be converted.",
                                             "Value 'b' could not be converted."
                                         };
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());
            textBox1.EnterSingle('1');
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }
    }
}