namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class TwoLevelScopeWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "TwoLevelScopeWindow";

        private TextBox IntTextBox1 => window.FindTextBox("IntTextBox1");

        private TextBox IntTextBox2 => window.FindTextBox("IntTextBox2");

        private TextBox DoubleTextBox => window.FindTextBox("DoubleTextBox");

        private GroupBox Scope => window.FindGroupBox("Scope");

        private IReadOnlyList<string> ScopeErrors => window.FindGroupBox("Scope").GetErrors();

        private string ScopeHasError => window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text;

        private GroupBox Node => window.FindGroupBox("Node");

        private string ChildCount => window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text;

        private IReadOnlyList<string> NodeErrors => window.FindGroupBox("Node").GetErrors();

        private IReadOnlyList<string> NodeChildren => window.FindGroupBox("Node").GetChildren();

        private string NodeHasError => window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text;

        private string NodeType => window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text;

        [SetUp]
        public void SetUp()
        {
            this.IntTextBox1.Text = "0";
            this.DoubleTextBox.Text = "0";
            Keyboard.Type(Key.TAB);
        }

        [Test]
        public void CheckNodeType()
        {
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
            this.IntTextBox1.Text = "a";
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
            this.IntTextBox1.Text = "1";
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
        }

        [Test]
        public void AddThenRemoveError()
        {
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.IntTextBox1.Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.IntTextBox1.Text = "1";
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
        }

        [Test]
        public void AddThenRemoveErrorTwice()
        {
            this.AddThenRemoveError();
            this.AddThenRemoveError();
        }

        [Test]
        public void AddTwoErrorsThenRemoveThemOneByOne()
        {
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.IntTextBox1.Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };

            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.DoubleTextBox.Text = "b";
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.IntTextBox1.Text = "1";
            expectedErrors = new[] { "Value 'b' could not be converted." };

            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.DoubleTextBox.Enter("2");
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
        }

        [Test]
        public void AddTwoErrorsThenThenRemoveBothAtOnce()
        {
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.IntTextBox1.Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.IntTextBox2.Text = "b";
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.IntTextBox1.Text = "1";
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
        }
    }
}