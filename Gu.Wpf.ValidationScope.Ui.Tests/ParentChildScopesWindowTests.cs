namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class ParentChildScopesWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "ParentChildScopesWindow";

        private TextBox TextBoxScopeTextBox1 => window.FindTextBox("TextBoxScopeTextBox1");

        private TextBox TextBoxScopeTextBox2 => window.FindTextBox("TextBoxScopeTextBox2");

        private TextBox ComboBoxScopeTextBox1 => window.FindTextBox("ComboBoxScopeTextBox1");

        private TextBox ComboBoxScopeTextBox2 => window.FindTextBox("ComboBoxScopeTextBox2");

        private ComboBox ComboBoxScopeComboBox2 => window.FindComboBox("ComboBoxScopeComboBox2");

        private TextBox NoScopeTextBox1 => window.FindTextBox("NoScopeTextBox1");

        private TextBox NoScopeTextBox2 => window.FindTextBox("NoScopeTextBox2");

        private ComboBox NoScopeComboBox1 => window.FindComboBox("NoScopeComboBox1");

        [SetUp]
        public void SetUp()
        {
            this.TextBoxScopeTextBox1.Text = "0";
            this.TextBoxScopeTextBox2.Text = "0";
            Keyboard.Type(Key.TAB);
        }

        [Test]
        public void CheckNodeType()
        {
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.TextBoxScopeTextBox1.Text = "a";
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.TextBoxScopeTextBox1.Text = "1";
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
        }

        [Test]
        public void AddThenRemoveErrorTextBox()
        {
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.TextBoxScopeTextBox1.Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.TextBoxScopeTextBox1.Text = "1";
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
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
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.ComboBoxScopeComboBox2.EditableText = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.ComboBoxScopeComboBox2.EditableText = "1";
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
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
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.NoScopeTextBox1.Text = "a";
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
        }

        [Test]
        public void NoErrorWhenNotScopedComboBox()
        {
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.NoScopeComboBox1.EditableText = "a";
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
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

            this.TextBoxScopeTextBox1.Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };

            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.ComboBoxScopeComboBox2.EditableText = "b";
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.ComboBoxScopeTextBox2.Text = "1";
            expectedErrors = new[] { "Value 'a' could not be converted." };

            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.ComboBoxScopeTextBox1.Enter("2");
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

            this.TextBoxScopeTextBox2.Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.ComboBoxScopeComboBox2.EditableText = "b";
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 1", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors,  window.FindGroupBox("Node").GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, window.FindGroupBox("Node").GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);

            this.NoScopeTextBox2.Text = "1";
            Assert.AreEqual("HasError: False", window.FindGroupBox("Scope").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(window.FindGroupBox("Scope").GetErrors());

            Assert.AreEqual("Children: 0", window.FindGroupBox("Node").FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", window.FindGroupBox("Node").FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty( window.FindGroupBox("Node").GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock").Text);
        }
    }
}
