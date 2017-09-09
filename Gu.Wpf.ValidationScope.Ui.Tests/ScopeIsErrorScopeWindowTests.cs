namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class ScopeIsErrorScopeWindowTests
    {
        private static string WindowName { get; } = "ScopeIsErrorScopeWindow";

        [SetUp]
        public void SetUp()
        {
            if (Application.TryAttach(Info.ExeFileName, WindowName, out var app))
            {
                using (app)
                {
                    var window = app.MainWindow;
                    window.FindTextBox("TextBox").Text = "0";
                    window.FindCheckBox("HasErrorCheckBox").IsChecked = false;
                    Keyboard.Type(Key.TAB);
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => Application.KillLaunched(Info.ExeFileName);

        [Test]
        public void CheckNodeType()
        {
            using (var app = Application.AttachOrLaunch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var nodeType = window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock");
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", nodeType.Text);

                window.FindTextBox("TextBox").Text = "a";
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", nodeType.Text);

                window.FindTextBox("TextBox").Text = "1";
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", nodeType.Text);
            }
        }

        [Test]
        public void AddTextBoxErrorThenNotifyErrorThenRemoveNotifyThenRemoveTextBoxError()
        {
            using (var app = Application.AttachOrLaunch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var scope = window.FindGroupBox("Scope");
                var node = window.FindGroupBox("Node");

                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("TextBox").Text = "a";
                var expectedErrors = new[] { "Value 'a' could not be converted." };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindCheckBox("HasErrorCheckBox").IsChecked = true;
                expectedErrors = new[] { "Value 'a' could not be converted.", "INotifyDataErrorInfo error" };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindCheckBox("HasErrorCheckBox").IsChecked = false;
                expectedErrors = new[] { "Value 'a' could not be converted." };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("TextBox").Text = "1";
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
        }

        [Test]
        public void AddTextBoxErrorThenNotifyErrorThenRemoveTextBoxThenRemoveNotifyError()
        {
            using (var app = Application.AttachOrLaunch(Info.ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var scope = window.FindGroupBox("Scope");
                var node = window.FindGroupBox("Node");

                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("TextBox").Text = "a";
                var expectedErrors = new[] { "Value 'a' could not be converted." };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindCheckBox("HasErrorCheckBox").IsChecked = true;
                expectedErrors = new[] { "Value 'a' could not be converted.", "INotifyDataErrorInfo error" };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("TextBox").Text = "1";
                expectedErrors = new[] { "INotifyDataErrorInfo error" };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindCheckBox("HasErrorCheckBox").IsChecked = false;
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
        }
    }
}