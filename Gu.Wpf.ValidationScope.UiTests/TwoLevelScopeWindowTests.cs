namespace Gu.Wpf.ValidationScope.UiTests
{
    using Gu.Wpf.UiAutomation;

    using NUnit.Framework;

    public class TwoLevelScopeWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
        private const string WindowName = "TwoLevelScopeWindow";

        [SetUp]
        public void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("IntTextBox1").Text = "0";
            window.FindTextBox("DoubleTextBox").Text = "0";
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => Application.KillLaunched(ExeFileName);

        [Test]
        public void CheckNodeType()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var nodeType = window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock");

            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", nodeType.Text);
            window.FindTextBox("IntTextBox1").Text = "a";

            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", nodeType.Text);

            window.FindTextBox("IntTextBox1").Text = "1";
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", nodeType.Text);
        }

        [Test]
        public void AddThenRemoveError()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var scope = window.FindGroupBox("Scope");
            var node = window.FindGroupBox("Node");

            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(scope.GetErrors());

            Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(node.GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            window.FindTextBox("IntTextBox1").Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

            Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, node.GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            window.FindTextBox("IntTextBox1").Text = "1";
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(scope.GetErrors());

            Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(node.GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
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
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var scope = window.FindGroupBox("Scope");
            var node = window.FindGroupBox("Node");

            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(scope.GetErrors());

            Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(node.GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            window.FindTextBox("IntTextBox1").Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };

            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

            Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, node.GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            window.FindTextBox("DoubleTextBox").Text = "b";
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

            Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, node.GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            window.FindTextBox("IntTextBox1").Text = "1";
            expectedErrors = new[] { "Value 'b' could not be converted." };

            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

            Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, node.GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            window.FindTextBox("DoubleTextBox").Enter("2");
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(scope.GetErrors());

            Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(node.GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
        }

        [Test]
        public void AddTwoErrorsThenThenRemoveBothAtOnce()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var scope = window.FindGroupBox("Scope");
            var node = window.FindGroupBox("Node");

            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(scope.GetErrors());

            Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(node.GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            window.FindTextBox("IntTextBox1").Text = "a";
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

            Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, node.GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            window.FindTextBox("IntTextBox2").Text = "b";
            expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

            Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.Grid" }, node.GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            window.FindTextBox("IntTextBox1").Text = "1";
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(scope.GetErrors());

            Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(node.GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
        }
    }
}
