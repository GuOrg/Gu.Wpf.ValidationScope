namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class NotifyDataErrorInfoWindowTests
    {
        private static string WindowName { get; } = "NotifyDataErrorInfoWindow";

        [SetUp]
        public void SetUp()
        {
            if (Application.TryAttach(Info.ExeFileName, WindowName, out var app))
            {
                using (app)
                {
                    var window = app.MainWindow;
                    window.FindTextBox("IntTextBox1").Text = "1";
                    window.FindTextBox("IntTextBox2").Text = "2";
                    window.FindTextBox("ErrorTextBox1").Text = string.Empty;
                    window.FindTextBox("ErrorTextBox2").Text = string.Empty;
                    Keyboard.Type(Key.TAB);
                    window.WaitUntilResponsive();
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
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", nodeType.Text);

                window.FindTextBox("ErrorTextBox1").Text = "error 1";
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", nodeType.Text);

                window.FindTextBox("ErrorTextBox1").Text = string.Empty;
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", nodeType.Text);
            }
        }

        [Test]
        public void AddThenRemoveError()
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
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ErrorTextBox1").Text = "error 1";
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                var expectedErrors = new[] { "error 1" };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: 1" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ErrorTextBox1").Text = string.Empty;
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
        }

        [Test]
        public void AddThenRemoveErrorTwice()
        {
            this.AddThenRemoveError();
            this.AddThenRemoveError();
        }

        [Test]
        public void AddTwoErrorsThenRemoveThemOneByOne()
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
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ErrorTextBox1").Text = "error 1";
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                var expectedErrors = new[] { "error 1" };
                var expectedChildren = new[] { "System.Windows.Controls.TextBox: 1" };

                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(expectedChildren, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ErrorTextBox2").Text = "error 2";
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                expectedErrors = new[] { "error 1", "error 2" };
                expectedChildren = new[] { "System.Windows.Controls.TextBox: 1", "System.Windows.Controls.TextBox: 2" };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 2", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(expectedChildren, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ErrorTextBox1").Text = string.Empty;
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                expectedErrors = new[] { "error 2" };
                expectedChildren = new[] { "System.Windows.Controls.TextBox: 2" };

                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(expectedChildren, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ErrorTextBox2").Text = string.Empty;
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(scope.GetErrors());

                Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.IsEmpty(node.GetErrors());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
            }
        }

        [Test]
        public void AddThenUpdateErrorThenRemoveIt()
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
                Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ErrorTextBox1").Text = "error 1";
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                var expectedErrors = new[] { "error 1" };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: 1" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ErrorTextBox1").Enter("error 2");
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
                expectedErrors = new[] { "error 2" };
                Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

                Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
                Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
                CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
                CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: 1" }, node.GetChildren());
                Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

                window.FindTextBox("ErrorTextBox1").Enter(string.Empty);
                Keyboard.Type(Key.TAB);
                window.WaitUntilResponsive();
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