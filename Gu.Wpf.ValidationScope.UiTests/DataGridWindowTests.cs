namespace Gu.Wpf.ValidationScope.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static class DataGridWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
        private const string WindowName = "DataGridWindow";

        [Test]
        public static void AddThenRemoveError()
        {
            using var app = Application.Launch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var scope = window.FindGroupBox("Scope");
            var node = window.FindGroupBox("Node");

            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(scope.GetErrors());

            Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(node.GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            var cell = window.FindDataGrid("DataGrid")[0, 0];
            cell.Click();
            cell.Enter("a");
            Keyboard.Type(Key.TAB);
            var expectedErrors = new[] { "Value 'a' could not be converted." };
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

            Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
            CollectionAssert.AreEqual(new[] { "System.Windows.Controls.DataGrid Items.Count:3" }, node.GetChildren());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

            cell.Click();
            Keyboard.Type(Key.BACK);
            cell.Enter("2");
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(scope.GetErrors());

            Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
            Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.IsEmpty(node.GetErrors());
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
        }
    }
}
