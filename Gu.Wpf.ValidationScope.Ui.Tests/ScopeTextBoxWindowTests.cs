namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class ScopeTextBoxWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "ScopeTextBoxWindow";

        private TextBox TextBox => this.Window.FindTextBox("TextBox");

        private GroupBox Scope => this.Window.FindGroupBox("Scope");

        private IReadOnlyList<string> ScopeErrors => this.Scope.GetErrors();

        private string ScopeHasError => this.Scope.FindTextBlock("HasErrorTextBlock").Text;

        private GroupBox Node => this.Window.FindGroupBox("Node");

        private string ChildCount => this.Node.FindTextBlock("ChildCountTextBlock").Text;

        private IReadOnlyList<string> NodeErrors => this.Node.GetErrors();

        private string NodeHasError => this.Node.FindTextBlock("HasErrorTextBlock").Text;

        private string NodeType => this.Node.FindTextBlock("NodeTypeTextBlock").Text;

        [SetUp]
        public void SetUp()
        {
            this.TextBox.Text = "0";
            Keyboard.Type(Key.TAB);
        }

        [Test]
        public void CheckNodeType()
        {
            Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", this.NodeType);

            this.TextBox.Text = "a";
            Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", this.NodeType);

            this.TextBox.Text = "1";
            Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", this.NodeType);
        }

        [Test]
        public void AddThenRemoveError()
        {
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", this.NodeType);

            this.TextBox.Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", this.NodeType);

            this.TextBox.Text = "1";
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", this.NodeType);
        }

        [Test]
        public void AddThenRemoveErrorTwice()
        {
            this.AddThenRemoveError();
            this.AddThenRemoveError();
        }
    }
}