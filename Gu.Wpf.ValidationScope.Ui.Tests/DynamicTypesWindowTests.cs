namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.WindowsAPI;

    public class DynamicTypesWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "DynamicTypesWindow";

        public TextBox TextBox1 => this.Window.Get<TextBox>("TextBox1");

        public TextBox TextBox2 => this.Window.Get<TextBox>("TextBox2");

        public ComboBox ComboBox1 => this.Window.Get<ComboBox>("ComboBox1");

        public ComboBox ComboBox2 => this.Window.Get<ComboBox>("ComboBox2");

        public ListBox TypeListBox => this.Window.Get<ListBox>("TypeListBox");

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
        public void Setup()
        {
            this.TextBox1.Enter("0");
            this.TextBox2.Enter("0");
            this.TypeListBox.Items[2].Click();
            this.Window.WaitWhileBusy();
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

            this.TextBox1.Enter('a');
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.TypeListBox.Select("System.Windows.Controls.TextBox");
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: a" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.TypeListBox.Select("System.Windows.Controls.Slider");
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

            this.TextBox1.Enter('a');
            this.TextBox2.Enter('b');
            this.ComboBox1.Enter('c');
            this.ComboBox2.Enter('d');
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);

            this.TypeListBox.Select("System.Windows.Controls.TextBox");
            var expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 2", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: a", "System.Windows.Controls.TextBox: b" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.Window.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            this.TypeListBox.Items[1].Click();
            this.Window.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
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

            //this.TypeListBox.Select("System.Windows.Controls.Primitives.Selector");
            this.TypeListBox.Items[1].Click();
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

            this.ComboBox1.Enter('1');
            expectedErrors = new[] { "Value 'd' could not be converted." };
            Assert.AreEqual("HasError: True", this.ScopeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.ScopeErrors);

            Assert.AreEqual("Children: 1", this.ChildCount);
            Assert.AreEqual("HasError: True", this.NodeHasError);
            CollectionAssert.AreEqual(expectedErrors, this.NodeErrors);
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.ComboBox Items.Count:0" }, this.NodeChildren);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", this.NodeType);

            this.TypeListBox.Select("System.Windows.Controls.Slider");
            Assert.AreEqual("HasError: False", this.ScopeHasError);
            CollectionAssert.IsEmpty(this.ScopeErrors);

            Assert.AreEqual("Children: 0", this.ChildCount);
            Assert.AreEqual("HasError: False", this.NodeHasError);
            CollectionAssert.IsEmpty(this.NodeErrors);
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", this.NodeType);
        }


        [Test]
        public void Updates()
        {
            Assert.Inconclusive();
            //var typesListBox = this.Window.Get<ListBox>(AutomationIDs.TypeListBox);
            //Assert.AreEqual(null, typesListBox.SelectedItem);
            //Assert.AreEqual(string.Empty, childCountBlock.Text);
            //CollectionAssert.IsEmpty(this.Window.GetErrors());

            //var textBox1 = this.Window.Get<TextBox>(AutomationIDs.TextBox1);
            //textBox1.Enter('a');
            //Assert.AreEqual(string.Empty, childCountBlock.Text);
            //CollectionAssert.IsEmpty(this.Window.GetErrors());

            //typesListBox.Select(0);
            //Assert.AreEqual("Children: 1", childCountBlock.Text);
            //CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            //typesListBox.Select(1);
            //Assert.AreEqual(string.Empty, childCountBlock.Text);
            //CollectionAssert.IsEmpty(this.Window.GetErrors());

            //var comboBox1 = this.Window.Get<ComboBox>(AutomationIDs.ComboBox1);
            //comboBox1.Enter('b');
            //Assert.AreEqual("Children: 1", childCountBlock.Text);
            //CollectionAssert.AreEqual(new[] { "Value 'b' could not be converted." }, this.Window.GetErrors());

            //this.Window.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            //typesListBox.Items[0].Click();
            //this.Window.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
            //Assert.AreEqual("Children: 2", childCountBlock.Text);
            //CollectionAssert.AreEqual(new[] { "Value 'b' could not be converted.", "Value 'a' could not be converted." }, this.Window.GetErrors());

            //textBox1.Text = "2";
            //this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            //this.Window.WaitWhileBusy();
            //Assert.AreEqual("", childCountBlock.Text);
            //CollectionAssert.IsEmpty(this.Window.GetErrors());   
        }
    }
}