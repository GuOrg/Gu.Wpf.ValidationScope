namespace Gu.Wpf.ValidationScope.UiTests
{
    using System.Linq;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class LiveErrorsWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
        private const string WindowName = "LiveErrorsWindow";

        [OneTimeTearDown]
        public void OneTimeTearDown() => Application.KillLaunched(ExeFileName);

        [Test]
        public void Validation()
        {
            using var app = Application.Launch(ExeFileName, WindowName);
            var window = app.MainWindow;

            // this is used as reference
            var groupBox = window.FindGroupBox("Validation errors");
            var hasErrorBlock = groupBox.FindTextBlock("HasErrorTextBlock");
            var textBox = window.FindTextBox("ValidationIntValueTextBox");
            var errorBox = window.FindTextBox("ValidationErrorTextBox");
            var actual = groupBox.GetErrors();
            CollectionAssert.AreEqual("HasError: False", hasErrorBlock.Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "g";
            Assert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetErrors();
            CollectionAssert.AreEqual(new[] { "Value 'g' could not be converted." }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            errorBox.Text = "error 1";
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.GetErrors();
            CollectionAssert.AreEqual(new[] { "Value 'g' could not be converted.", "error 1" }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "1";
            Assert.AreEqual("HasError: True", hasErrorBlock.Text);
            actual = groupBox.FindTextBlocks("ErrorTextBlock").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(new[] { "error 1" }, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            errorBox.Text = string.Empty;
            Keyboard.Type(Key.TAB);
            window.WaitUntilResponsive();
            Assert.AreEqual("HasError: False", hasErrorBlock.Text);
            actual = groupBox.FindTextBlocks("ErrorTextBlock").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeTextBox()
        {
            using var app = Application.Launch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var groupBox = window.FindGroupBox("Scope textbox errors");
            var textBox = window.FindTextBox("ScopeIntValueTextBox");
            var errorBox = window.FindTextBox("ScopeErrorTextBox");
            var node = groupBox.FindGroupBox("Node");
            var scope = groupBox.FindGroupBox("Scope");

            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());

            textBox.Text = "g";
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            var expected = new[] { "Value 'g' could not be converted." };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox.Text = "error 1";
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox.Text = "1";
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox.Enter(string.Empty);
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());
        }

        [Test]
        public void ScopeGroupBoxOneError()
        {
            using var app = Application.Launch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var groupBox = window.FindGroupBox("Scope errors");
            var textBox = window.FindTextBox("ScopeIntValueTextBox1");
            var errorBox = window.FindTextBox("ScopeErrorTextBox1");
            var node = groupBox.FindGroupBox("Node");
            var scope = groupBox.FindGroupBox("Scope");

            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());

            textBox.Text = "g";
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            var expected = new[] { "Value 'g' could not be converted." };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox.Text = "error 1";
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox.Text = "1";
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox.Text = string.Empty;
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());
        }

        [Test]
        public void ScopeGroupBoxTwoErrors()
        {
            using var app = Application.Launch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var groupBox = window.FindGroupBox("Scope errors");
            var textBox1 = window.FindTextBox("ScopeIntValueTextBox1");
            var textBox2 = window.FindTextBox("ScopeIntValueTextBox2");
            var errorBox1 = window.FindTextBox("ScopeErrorTextBox1");
            var errorBox2 = window.FindTextBox("ScopeErrorTextBox2");
            var node = groupBox.FindGroupBox("Node");
            var scope = groupBox.FindGroupBox("Scope");

            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());

            textBox1.Text = "g";
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            var expected = new[] { "Value 'g' could not be converted." };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox1.Text = "error 1";
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox2.Text = "b";
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1", "Value 'b' could not be converted." };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox2.Text = "error 2";
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "Value 'g' could not be converted.", "error 1", "Value 'b' could not be converted.", "error 2" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox1.Text = "1";
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "error 1", "Value 'b' could not be converted.", "error 2" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            textBox2.Text = "2";
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "error 1", "error 2" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox2.Text = string.Empty;
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            expected = new[] { "error 1" };
            CollectionAssert.AreEqual(expected, node.GetErrors());
            CollectionAssert.AreEqual(expected, scope.GetErrors());

            errorBox1.Text = string.Empty;
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), node.GetErrors());
            CollectionAssert.AreEqual(Enumerable.Empty<string>(), scope.GetErrors());
        }
    }
}
