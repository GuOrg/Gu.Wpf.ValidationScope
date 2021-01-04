namespace Gu.Wpf.ValidationScope.UiTests
{
    using Gu.Wpf.UiAutomation;

    using NUnit.Framework;

    public static class DataTemplateSelectorWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
        private const string WindowName = "DataTemplateSelectorWindow";

        [SetUp]
        public static void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("TemplateType").Text = "int";
            window.FindButton("Reset").Click();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown() => Application.KillLaunched(ExeFileName);

        [Test]
        public static void SelectTemplate()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var nodeType = window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock");
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", nodeType.Text);

            window.FindTextBox("IntBox").Text = "a";
            Wait.UntilInputIsProcessed();
            Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", nodeType.Text);

            window.FindTextBox("TemplateType").Text = "string";
            Wait.UntilInputIsProcessed();
            Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", nodeType.Text);
        }
    }
}
