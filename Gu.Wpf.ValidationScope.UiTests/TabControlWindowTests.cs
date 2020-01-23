namespace Gu.Wpf.ValidationScope.UiTests
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class TabControlWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
        private const string WindowName = "TabControlWindow";

        [SetUp]
        public void SetUp()
        {
            if (Application.TryAttach(ExeFileName, WindowName, out var app))
            {
                using (app)
                {
                    var window = app.MainWindow;
                    window.FindTextBox("IntTextBox1").Text = "0";
                    window.FindTextBox("IntTextBox2").Text = "0";
                    Keyboard.Type(Key.TAB);
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => Application.KillLaunched(ExeFileName);

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
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, scope.GetErrors());

            window.FindTextBox("IntTextBox2").Text = "b";
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." }, scope.GetErrors());

            window.FindTabControl().Select("Tab 2");
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." }, scope.GetErrors());

            window.FindTextBox("IntTextBox3").Text = "c";
            Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted.", "Value 'c' could not be converted." }, scope.GetErrors());
        }
    }
}
