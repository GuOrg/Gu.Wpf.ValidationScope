namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class DynamicTypesWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "DynamicTypesWindow";

        private TextBox TextBox1 => this.Window.FindTextBox("TextBox1");

        private TextBox TextBox2 => this.Window.FindTextBox("TextBox2");

        private ComboBox ComboBox1 => this.Window.FindComboBox("ComboBox1");

        private ComboBox ComboBox2 => this.Window.FindComboBox("ComboBox2");

        private ListBox TypeListBox => this.Window.FindListBox("TypeListBox");

        private GroupBox Scope => this.Window.FindGroupBox("Scope");

        private IReadOnlyList<string> ScopeErrors => this.Scope.GetErrors();

        private string ScopeHasError => this.Scope.FindTextBlock("HasErrorTextBlock").Text;

        private GroupBox Node => this.Window.FindGroupBox("Node");

        private string ChildCount => this.Node.FindTextBlock("ChildCountTextBlock").Text;

        private IReadOnlyList<string> NodeErrors => this.Node.GetErrors();

        private IReadOnlyList<string> NodeChildren => this.Node.GetChildren();

        private string NodeHasError => this.Node.FindTextBlock("HasErrorTextBlock").Text;

        private string NodeType => this.Node.FindTextBlock("NodeTypeTextBlock").Text;

        [SetUp]
        public void Setup()
        {
            this.TextBox1.Text = "0";
            this.TextBox2.Text = "0";
            this.TypeListBox.Select(2);
        }

        [Test]
        public void SetTextBoxErrorThenSelectTextBoxThenSelectSlider()
        {
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.TextBox1.Text = "a";
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.TypeListBox.Select(0);
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: a" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.TypeListBox.Select(3);
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }

        [Test]
        public void SetTextBoxErrorThenSelectTextBoxThenSelectSliderTwice()
        {
            this.SetTextBoxErrorThenSelectTextBoxThenSelectSlider();
            this.SetTextBoxErrorThenSelectTextBoxThenSelectSlider();
        }

        [Test]
        public void SetAllErrorsThenSetDifferentScopes()
        {
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.TextBox1.Text = "a";
            this.TextBox2.Text = "b";
            this.ComboBox1.EditableText = "c";
            this.ComboBox2.EditableText = "d";
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.TypeListBox.Select(0);
            var expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 2", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: a", "System.Windows.Controls.TextBox: b" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            using (Keyboard.Pressing(Key.CONTROL))
            {
                this.TypeListBox.Items[1].Click();
            }

            expectedErrors = new[]
            {
                "Value 'a' could not be converted.",
                "Value 'b' could not be converted.",
                "Value 'c' could not be converted.",
                "Value 'd' could not be converted."
            };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 4", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: a", "System.Windows.Controls.TextBox: b", "System.Windows.Controls.ComboBox Items.Count:0", "System.Windows.Controls.ComboBox Items.Count:0" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            ////this.TypeListBox.Select("System.Windows.Controls.Primitives.Selector");
            this.TypeListBox.Select(1);
            expectedErrors = new[]
            {
                "Value 'c' could not be converted.",
                "Value 'd' could not be converted."
            };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 2", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.ComboBox Items.Count:0", "System.Windows.Controls.ComboBox Items.Count:0" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.ComboBox1.EditableText = "1";
            expectedErrors = new[] { "Value 'd' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.ComboBox Items.Count:0" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.TypeListBox.Select(3);
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }
    }
}