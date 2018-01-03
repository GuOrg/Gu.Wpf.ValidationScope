namespace Gu.Wpf.ValidationScope.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class ParentChildScopesWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
        private const string WindowName = "ParentChildScopesWindow";

        [SetUp]
        public void SetUp()
        {
            if (Application.TryAttach(ExeFileName, WindowName, out var app))
            {
                using (app)
                {
                    var window = app.MainWindow;
                    window.FindTextBox("TextBoxScopeTextBox1").Text = "0";
                    window.FindTextBox("TextBoxScopeTextBox2").Text = "0";
                    Keyboard.Type(Key.TAB);
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => Application.KillLaunched(ExeFileName);

        [Test]
        public void CheckNodeType()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var nodeType = window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock");

                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", nodeType.Text);

                window.FindTextBox("TextBoxScopeTextBox1").Text = "a";
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", nodeType.Text);

                window.FindTextBox("TextBoxScopeTextBox1").Text = "1";
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", nodeType.Text);
            }
        }

        [Test]
        public void AddThenRemoveErrorTextBox()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var scope = window.FindGroupBox("Scope");
                var node = window.FindGroupBox("Node");

                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("TextBoxScopeTextBox1").Text = "a";
                var expectedErrors = new[] { "Value 'a' could not be converted." };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("TextBoxScopeTextBox1").Text = "1";
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
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
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var scope = window.FindGroupBox("Scope");
                var node = window.FindGroupBox("Node");

                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindComboBox("ComboBoxScopeComboBox2").EditableText = "a";
                var expectedErrors = new[] { "Value 'a' could not be converted." };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindComboBox("ComboBoxScopeComboBox2").EditableText = "1";
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
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
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var scope = window.FindGroupBox("Scope");
                var node = window.FindGroupBox("Node");

                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("NoScopeTextBox1").Text = "a";
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
        }

        [Test]
        public void NoErrorWhenNotScopedComboBox()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var scope = window.FindGroupBox("Scope");
                var node = window.FindGroupBox("Node");

                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindComboBox("NoScopeComboBox1").EditableText = "a";
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
        }

        [Test]
        public void AddTwoErrorsThenRemoveThemOneByOne()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var scope = window.FindGroupBox("Scope");
                var node = window.FindGroupBox("Node");

                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("TextBoxScopeTextBox1").Text = "a";
                var expectedErrors = new[] { "Value 'a' could not be converted." };

                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindComboBox("ComboBoxScopeComboBox2").EditableText = "b";
                expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ComboBoxScopeTextBox2").Text = "1";
                expectedErrors = new[] { "Value 'a' could not be converted." };

                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ComboBoxScopeTextBox1").Enter("2");
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
        }

        [Test]
        public void AddTwoErrorsThenThenRemoveBothAtOnce()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var scope = window.FindGroupBox("Scope");
                var node = window.FindGroupBox("Node");

                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("TextBoxScopeTextBox2").Text = "a";
                var expectedErrors = new[] { "Value 'a' could not be converted." };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindComboBox("ComboBoxScopeComboBox2").EditableText = "b";
                expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.StackPanel" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("NoScopeTextBox2").Text = "1";
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
        }
    }
}
