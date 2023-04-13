namespace Gu.Wpf.ValidationScope.UiTests;

using Gu.Wpf.UiAutomation;
using NUnit.Framework;

public static class ScopeIsErrorScopeWindowTests
{
    private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
    private const string WindowName = "ScopeIsErrorScopeWindow";

    [SetUp]
    public static void SetUp()
    {
        if (Application.TryAttach(ExeFileName, WindowName, out var app))
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
    public static void OneTimeTearDown() => Application.KillLaunched(ExeFileName);

    [Test]
    public static void CheckNodeType()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var nodeType = window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock");
        Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", nodeType.Text);

        window.FindTextBox("TextBox").Text = "a";
        Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", nodeType.Text);

        window.FindTextBox("TextBox").Text = "1";
        Assert.AreEqual("Gu.Wpf.ValidationScope.InputNode", nodeType.Text);
    }

    [Test]
    public static void AddTextBoxErrorThenNotifyErrorThenRemoveNotifyThenRemoveTextBoxError()
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

    [Test]
    public static void AddTextBoxErrorThenNotifyErrorThenRemoveTextBoxThenRemoveNotifyError()
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
