namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Linq;

    using Castle.Components.DictionaryAdapter.Xml;

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
            var hasErrorBlock = groupBox.Get<Label>("HasErrorTextBlock");
            var textBox = this.Window.Get<TextBox>("ValidationIntValueTextBox");
            var errorBox = this.Window.Get<TextBox>("ValidationErrorTextBox");
            var actual = groupBox.GetErrors();
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Enter('g');
            Assert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetErrors();
            CollectionAssert.AreEqual(new[] { "Value 'g' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            errorBox.Enter("error 1");
            this.PressTab();
            Assert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetErrors();
            CollectionAssert.AreEqual(new[] { "Value 'g' could not be converted.", "error 1" }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "1";
            Assert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "error 1" }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            errorBox.Enter("");
            this.PressTab();
            Assert.AreEqual("HasError: False", hasErrorBlock.Text);
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            actual = groupBox.GetMultiple<Label>(ErrorTextBlockName).Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeTextBox()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Scope textbox errors");
            var textBox = this.Window.Get<TextBox>("ScopeIntValueTextBox");
            var errorBox = this.Window.Get<TextBox>("ScopeErrorTextBox");
            var node = groupBox.GetByText<GroupBox>("Node");
            var scope = groupBox.GetByText<GroupBox>("Scope");

            Assert.AreEqual("HasError: False", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.Get<Label>("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());

            textBox.Enter('g');
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            var expected = new[] { "Value 'g' could not be converted." };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox.Enter("error 1");
            this.PressTab();
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox.Text = "1";
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] {  "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox.Enter("");
            this.PressTab();
            Assert.AreEqual("HasError: False", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.Get<Label>("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());
        }

        [Test]
        public void ScopeGroupBoxOneError()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Scope errors");
            var textBox = this.Window.Get<TextBox>("ScopeIntValueTextBox1");
            var errorBox = this.Window.Get<TextBox>("ScopeErrorTextBox1");
            var node = groupBox.GetByText<GroupBox>("Node");
            var scope = groupBox.GetByText<GroupBox>("Scope");

            Assert.AreEqual("HasError: False", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.Get<Label>("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());

            textBox.Enter('g');
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            var expected = new[] { "Value 'g' could not be converted." };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox.Enter("error 1");
            this.PressTab();
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox.Text = "1";
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] { "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox.Enter("");
            this.PressTab();
            Assert.AreEqual("HasError: False", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.Get<Label>("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());
        }

        [Test]
        public void ScopeGroupBoxTwoErrors()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Scope errors");
            var textBox1 = this.Window.Get<TextBox>("ScopeIntValueTextBox1");
            var textBox2 = this.Window.Get<TextBox>("ScopeIntValueTextBox2");
            var errorBox1 = this.Window.Get<TextBox>("ScopeErrorTextBox1");
            var errorBox2 = this.Window.Get<TextBox>("ScopeErrorTextBox2");
            var node = groupBox.GetByText<GroupBox>("Node");
            var scope = groupBox.GetByText<GroupBox>("Scope");

            Assert.AreEqual("HasError: False", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.Get<Label>("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());

            textBox1.Enter('g');
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            var expected = new[] { "Value 'g' could not be converted." };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox1.Enter("error 1");
            this.PressTab();
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox2.Enter('h');
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1", "Value 'h' could not be converted." };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox2.Enter("error 2");
            this.PressTab();
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1", "Value 'h' could not be converted.", "error 2" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox1.Text = "1";
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] { "error 1", "Value 'h' could not be converted.", "error 2" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox2.Enter("2");
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] { "error 1", "error 2" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox2.Enter("");
            this.PressTab();
            Assert.AreEqual("HasError: True", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.Get<Label>("HasErrorTextBlock").Text);
            expected = new[] { "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox1.Enter("");
            this.PressTab();
            Assert.AreEqual("HasError: False", node.Get<Label>("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.Get<Label>("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());
        }
    }
}