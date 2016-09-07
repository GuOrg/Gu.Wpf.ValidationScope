namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;

    public class ParentChildScopesWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "ParentChildScopesWindow";

        public TextBox TextBoxScopeTextBox1 => this.Window.Get<TextBox>("TextBoxScopeTextBox1");

        public TextBox TextBoxScopeTextBox2 => this.Window.Get<TextBox>("TextBoxScopeTextBox2");

        public ComboBox TextBoxScopeComboBox => this.Window.Get<ComboBox>("TextBoxScopeComboBox");

        public TextBox ComboBoxScopeTextBox1 => this.Window.Get<TextBox>("ComboBoxScopeTextBox1");

        public TextBox ComboBoxScopeTextBox2 => this.Window.Get<TextBox>("ComboBoxScopeTextBox2");

        public ComboBox ComboBoxScopeComboBox => this.Window.Get<ComboBox>("ComboBoxScopeComboBox");

        public TextBox NoScopeTextBox1 => this.Window.Get<TextBox>("NoScopeTextBox1");

        public TextBox NoScopeTextBox2 => this.Window.Get<TextBox>("NoScopeTextBox2");

        public ComboBox NoScopeComboBox => this.Window.Get<ComboBox>("NoScopeComboBox");

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
            this.TextBoxScopeTextBox1.Enter('0');
            this.TextBoxScopeTextBox2.Enter('0');
            this.PressTab();
            this.Window.WaitWhileBusy();
        }

        [Test]
        public void CheckNodeType()
        {
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
            this.TextBoxScopeTextBox1.Enter('a');
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);
            this.TextBoxScopeTextBox1.Enter('1');
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

            this.TextBoxScopeTextBox1.Enter('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.TextBoxScopeTextBox1.Enter('1');
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

            this.ComboBoxScopeComboBox.Enter('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ComboBoxScopeComboBox.Enter('1');
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

            this.NoScopeTextBox1.Enter('a');
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

            this.NoScopeComboBox.Enter('a');
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

            this.TextBoxScopeTextBox1.Enter('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };

            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ComboBoxScopeTextBox2.Enter('b');
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ComboBoxScopeTextBox1.Enter('1');
            expectedErrors = new[] { "Value 'b' could not be converted." };

            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.TextBoxScopeTextBox2.Enter('2');
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

            this.TextBoxScopeTextBox1.Enter('a');
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ComboBoxScopeTextBox1.Enter('b');
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.NoScopeTextBox1.Enter('1');
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }
    }
}
