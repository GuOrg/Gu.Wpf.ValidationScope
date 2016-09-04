namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class OneLevelScopeWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "OneLevelScopeWindow";

        [Test]
        public void UpdatesErrorThenReset()
        {
            var childCountBlock = this.Window.Get<Label>("ChildCountTextBlock");

            CollectionAssert.IsEmpty(this.Window.GetErrors());
            Assert.AreEqual("Children: 0", childCountBlock.Text);

            var intBox1 = this.Window.Get<TextBox>("IntValue1");
            intBox1.EnterSingle('a');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            intBox1.EnterSingle('b');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'b' could not be converted." }, this.Window.GetErrors());

            intBox1.EnterSingle('1');
            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }

        [Test]
        public void AddRemoveErrorTwice()
        {
            var childCountBlock = this.Window.Get<Label>("ChildCountTextBlock");

            CollectionAssert.IsEmpty(this.Window.GetErrors());
            Assert.AreEqual("Children: 0", childCountBlock.Text);

            var intBox1 = this.Window.Get<TextBox>("IntValue1");
            intBox1.EnterSingle('a');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            intBox1.EnterSingle('1');
            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());

            intBox1.EnterSingle('b');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'b' could not be converted." }, this.Window.GetErrors());

            intBox1.EnterSingle('2');
            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }

        [Test]
        public void UpdatesResetOneByOne()
        {
            var childCountBlock = this.Window.Get<Label>("ChildCountTextBlock");

            CollectionAssert.IsEmpty(this.Window.GetErrors());
            Assert.AreEqual("Children: 0", childCountBlock.Text);

            var intBox1 = this.Window.Get<TextBox>("IntValue1");
            intBox1.EnterSingle('a');
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());
            Assert.AreEqual("Children: 1", childCountBlock.Text);

            var doubleBox = this.Window.Get<TextBox>("DoubleValue");
            doubleBox.EnterSingle('b');
            Assert.AreEqual("Children: 2", childCountBlock.Text);
            var expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            intBox1.EnterSingle('1');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'b' could not be converted." }, this.Window.GetErrors());

            doubleBox.EnterSingle('2');
            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }

        [Test]
        public void UpdatesResetBothAtOnce()
        {
            var childCountBlock = this.Window.Get<Label>("ChildCountTextBlock");

            CollectionAssert.IsEmpty(this.Window.GetErrors());
            Assert.AreEqual("Children: 0", childCountBlock.Text);

            var intBox1 = this.Window.Get<TextBox>("IntValue1");
            intBox1.EnterSingle('a');
            Assert.AreEqual("Children: 1", childCountBlock.Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            var intBox2 = this.Window.Get<TextBox>("IntValue2");
            intBox2.EnterSingle('b');
            Assert.AreEqual("Children: 2", childCountBlock.Text);
            var expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            CollectionAssert.AreEqual(expectedErrors, this.Window.GetErrors());

            intBox1.EnterSingle('1');
            Assert.AreEqual("Children: 0", childCountBlock.Text);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }
    }
}
