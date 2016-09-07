// ReSharper disable PossibleMultipleEnumeration
namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;

    public class ComplicatedScopeWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "ComplicatedScopeWindow";

        public TextBox IntTextBox1 => this.Window.Get<TextBox>("IntTextBox1");

        public TextBox IntTextBox2 => this.Window.Get<TextBox>("IntTextBox2");

        public TextBox IntTextBox3 => this.Window.Get<TextBox>("IntTextBox3");

        public TextBox IntTextBox4 => this.Window.Get<TextBox>("IntTextBox4");

        public ComboBox IntComboBoxBox1 => this.Window.Get<ComboBox>("IntComboBox1");

        public ComboBox IntComboBoxBox2 => this.Window.Get<ComboBox>("IntComboBox2");

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
            this.IntComboBoxBox2.Enter('0');
            this.PressTab();
            this.Window.WaitWhileBusy();
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
        public void AddThenRemoveErrorTextBox()
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
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
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
        public void AddThenRemoveErrorTwiceTextBox()
        {
            this.AddThenRemoveErrorTextBox();
            this.AddThenRemoveErrorTextBox();
        }

        [Test]
        public void AddThenRemoveErrorComboBox()
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
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
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
        public void AddThenRemoveErrorTwiceComboBox()
        {
            this.AddThenRemoveErrorComboBox();
            this.AddThenRemoveErrorComboBox();
        }

        [Test]
        public void NoErrorWhenNotScopedTextBox()
        {
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.IntTextBox3.Enter('a');
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }

        [Test]
        public void NoErrorWhenNotScopedComboBox()
        {
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.IntComboBoxBox1.Enter('a');
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
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

            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntComboBoxBox2.Enter('b');
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntTextBox1.Enter('1');
            expectedErrors = new[] { "Value 'b' could not be converted." };

            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntComboBoxBox2.Enter('2');
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
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.IntTextBox2.Enter('b');
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
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