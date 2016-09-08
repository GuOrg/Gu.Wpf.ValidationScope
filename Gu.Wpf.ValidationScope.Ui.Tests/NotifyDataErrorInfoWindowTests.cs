namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class NotifyDataErrorInfoWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "NotifyDataErrorInfoWindow";

        public TextBox IntTextBox1 => this.Window.Get<TextBox>("IntTextBox1");

        public TextBox ErrorTextBox1 => this.Window.Get<TextBox>("ErrorTextBox1");

        public TextBox IntTextBox2 => this.Window.Get<TextBox>("IntTextBox2");

        public TextBox ErrorTextBox2 => this.Window.Get<TextBox>("ErrorTextBox2");

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
            this.IntTextBox1.Enter('1');
            this.IntTextBox2.Enter('2');
            this.ErrorTextBox1.Enter("");
            this.ErrorTextBox2.Enter("");
            this.PressTab();
        }

        [Test]
        public void CheckNodeType()
        {
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
            this.ErrorTextBox1.Enter("error 1");
            this.PressTab();
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);
            this.ErrorTextBox1.Enter("");
            this.PressTab();
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

            this.ErrorTextBox1.Enter("error 1");
            this.PressTab();
            var expectedErrors = new[] { "error 1" };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: 1" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ErrorTextBox1.Enter("");
            this.PressTab();
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

            this.ErrorTextBox1.Enter("error 1");
            this.PressTab();
            var expectedErrors = new[] { "error 1" };
            var expectedChildren = new[] { "System.Windows.Controls.TextBox: 1" };

            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(expectedChildren, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ErrorTextBox2.Enter("error 2");
            this.PressTab();
            expectedErrors = new[] { "error 1", "error 2" };
            expectedChildren = new[] { "System.Windows.Controls.TextBox: 1", "System.Windows.Controls.TextBox: 2" };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 2", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(expectedChildren, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ErrorTextBox1.Enter("");
            this.PressTab();
            expectedErrors = new[] { "error 2" };
            expectedChildren = new[] { "System.Windows.Controls.TextBox: 2" };

            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(expectedChildren, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ErrorTextBox2.Enter("");
            this.PressTab();
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }

        [Test]
        public void AddThenUpdateErrorThenRemoveIt()
        {
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.ErrorTextBox1.Enter("error 1");
            this.PressTab();
            var expectedErrors = new[] { "error 1" };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: 1" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ErrorTextBox1.Enter("error 2");
            this.PressTab();
            expectedErrors = new[] { "error 2" };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: 1" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ErrorTextBox1.Enter("");
            this.PressTab();
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }
    }
}