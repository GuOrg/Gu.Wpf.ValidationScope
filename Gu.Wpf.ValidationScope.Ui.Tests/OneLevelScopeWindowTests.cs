namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class OneLevelScopeWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "OneLevelScopeWindow";

        public TextBox IntTextBox1 => this.Window.Get<TextBox>("IntTextBox1");

        public TextBox IntTextBox2 => this.Window.Get<TextBox>("IntTextBox2");

        public TextBox DoubleTextBox => this.Window.Get<TextBox>("DoubleTextBox");

        public GroupBox Scope => this.Window.GetByText<GroupBox>("Scope");

        public IReadOnlyList<string> ScopeErrors => this.Scope.GetErrors();

        public string ScopeHasError => this.Scope.Get<Label>("HasErrorTextBlock").Text;

        public GroupBox Node => this.Window.GetByText<GroupBox>("Node");

        public string ChildCount => this.Node.Get<Label>("ChildCountTextBlock").Text;

        public IReadOnlyList<string> NodeErrors => this.Node.GetErrors();

        public IReadOnlyList<string> NodeChildren => this.Node.GetChildren();

        public string NodeHasError => this.Node.Get<Label>("HasErrorTextBlock").Text;

        public string NodeType => this.Node.Get<Label>("NodeTypeTextBlock").Text;

        [SetUp]
        public void SetUp()
        {
            this.IntTextBox1.Enter('0');
            this.DoubleTextBox.Enter('0');
            this.PressTab();
        }

        [Test]
        public void CheckNodeType()
        {
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
            this.IntTextBox1.Enter('a');
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);
            this.IntTextBox1.Enter('1');
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }

        [Test]
        public void AddThenRemoveError()
        {
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.IntTextBox1.Enter('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: a" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntTextBox1.Enter('1');
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
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
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.IntTextBox1.Enter('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            var expectedChildren = new[] { "System.Windows.Controls.TextBox: a" };

            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(expectedChildren, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.DoubleTextBox.Enter('b');
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            expectedChildren = new[] { "System.Windows.Controls.TextBox: a", "System.Windows.Controls.TextBox: b" };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 2", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(expectedChildren, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntTextBox1.Enter('1');
            expectedErrors = new[] { "Value 'b' could not be converted." };
            expectedChildren = new[] { "System.Windows.Controls.TextBox: b" };

            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(expectedChildren, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.DoubleTextBox.Enter('2');
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }

        [Test]
        public void AddTwoErrorsThenThenRemoveBothAtOnce()
        {
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.IntTextBox1.Enter('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            var expectedChildren = new[] { "System.Windows.Controls.TextBox: a" };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(expectedChildren, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntTextBox2.Enter('b');
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            expectedChildren = new[] { "System.Windows.Controls.TextBox: a", "System.Windows.Controls.TextBox: b" };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 2", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(expectedChildren, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntTextBox1.Enter('1');
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }

        [Test, Explicit("dunno if wpf can do this")]
        public void AddThenUpdateErrorThenRemoveIt()
        {
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.IntTextBox1.Enter('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntTextBox1.Enter('b');
            expectedErrors = new[] {  "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntTextBox1.Enter('1');
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }
    }
}
