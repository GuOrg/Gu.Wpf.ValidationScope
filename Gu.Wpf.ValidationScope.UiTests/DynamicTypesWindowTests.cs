namespace Gu.Wpf.ValidationScope.UiTests;

using Gu.Wpf.UiAutomation;

using NUnit.Framework;

public static class DynamicTypesWindowTests
{
    private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
    private const string WindowName = "DynamicTypesWindow";

    [SetUp]
    public static void SetUp()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        window.FindTextBox("TextBox1").Text = "0";
        window.FindTextBox("TextBox2").Text = "0";
        window.FindComboBox("ComboBox1").EditableText = "0";
        window.FindComboBox("ComboBox2").EditableText = "0";
        window.FindListBox("TypeListBox").Select("Slider");
    }

    [OneTimeTearDown]
    public static void OneTimeTearDown() => Application.KillLaunched(ExeFileName);

    [Test]
    public static void SetTextBoxErrorThenSelectTextBoxThenSelectSlider()
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

        window.FindTextBox("TextBox1").Text = "a";
        Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(scope.GetErrors());

        Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(node.GetErrors());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindListBox("TypeListBox").Select("TextBox");
        var expectedErrors = new[] { "Value 'a' could not be converted." };
        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: a" }, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindListBox("TypeListBox").Select("Slider");
        Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(scope.GetErrors());

        Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(node.GetErrors());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
    }

    [Test]
    public static void SetTextBoxErrorThenSelectTextBoxThenSelectSliderTwice()
    {
        SetTextBoxErrorThenSelectTextBoxThenSelectSlider();
        SetTextBoxErrorThenSelectTextBoxThenSelectSlider();
    }

    [Test]
    public static void SetAllErrorsThenSetDifferentScopes()
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

        window.FindTextBox("TextBox1").Text = "a";
        window.FindTextBox("TextBox2").Text = "b";
        window.FindComboBox("ComboBox1").EditableText = "c";
        window.FindComboBox("ComboBox2").EditableText = "d";
        Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(scope.GetErrors());

        Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(node.GetErrors());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindListBox("TypeListBox").Select("TextBox");
        var expectedErrors = new[]
        {
            "Value 'a' could not be converted.",
            "Value 'b' could not be converted.",
        };
        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 2", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: a", "System.Windows.Controls.TextBox: b" }, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        using (Keyboard.Hold(Key.CONTROL))
        {
            window.FindListBox("TypeListBox").Items[1].Click();
        }

        expectedErrors = new[]
        {
            "Value 'a' could not be converted.",
            "Value 'b' could not be converted.",
            "Value 'c' could not be converted.",
            "Value 'd' could not be converted.",
        };
        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 4", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(new[] { "System.Windows.Controls.TextBox: a", "System.Windows.Controls.TextBox: b", "System.Windows.Controls.ComboBox Items.Count:0", "System.Windows.Controls.ComboBox Items.Count:0" }, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindListBox("TypeListBox").Select("Selector");
        expectedErrors = new[]
        {
            "Value 'c' could not be converted.",
            "Value 'd' could not be converted.",
        };
        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 2", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(new[] { "System.Windows.Controls.ComboBox Items.Count:0", "System.Windows.Controls.ComboBox Items.Count:0" }, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindComboBox("ComboBox1").EditableText = "1";
        expectedErrors = new[] { "Value 'd' could not be converted." };
        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(new[] { "System.Windows.Controls.ComboBox Items.Count:0" }, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindListBox("TypeListBox").Select("Slider");
        Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(scope.GetErrors());

        Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(node.GetErrors());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
    }
}