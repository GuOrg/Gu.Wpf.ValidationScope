namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.WindowsAPI;

    public class LiveErrorsWindowTests : WindowTests
    {
        private const string ErrorTextBlockName = "ErrorTextBlock";

        protected override string WindowName { get; } = "LiveErrorsWindow";

        [Test]
        public void Validation()
        {
            // this is used as reference
            var groupBox = this.Window.GetByText<GroupBox>("Validation errors");
            var hasErrorBlock = groupBox.Get<Label>("ValidationHasErrorTextBlock");
            var textBox = this.Window.Get<TextBox>("ValidationIntValueTextBox");
            var actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            CollectionAssert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "Value 'g' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.EnterSingle('h');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            CollectionAssert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "Value 'h' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "1";
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeTextBox()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Scope textbox errors");
            var hasErrorBlock = groupBox.Get<Label>("ScopeTextBoxHasErrorTextBlock");
            var textBox = this.Window.Get<TextBox>("ScopeIntValueTextBox");
            var actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            CollectionAssert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "Value 'g' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.EnterSingle('h');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            CollectionAssert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "Value 'h' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "1";
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeGroupBoxOneError()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Scope errors");
            var hasErrorBlock = groupBox.Get<Label>("ScopeGroupBoxHasErrorTextBlock");
            var textBox = this.Window.Get<TextBox>("ScopeIntValueTextBox1");
            var actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            CollectionAssert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "Value 'g' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.EnterSingle('h');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            CollectionAssert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "Value 'h' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "1";
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeGroupBoxTwoErrors()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Scope errors");
            var hasErrorBlock = groupBox.Get<Label>("ScopeGroupBoxHasErrorTextBlock");
            var textBox1 = this.Window.Get<TextBox>("ScopeIntValueTextBox1");
            var textBox2 = this.Window.Get<TextBox>("ScopeIntValueTextBox2");
            var actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox1.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            CollectionAssert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "Value 'g' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox2.EnterSingle('h');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            CollectionAssert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "Value 'g' could not be converted.", "Value 'h' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox2.Text = "1";
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }
    }
}