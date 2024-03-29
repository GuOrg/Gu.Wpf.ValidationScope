﻿namespace Gu.Wpf.ValidationScope.UiTests;

using Gu.Wpf.UiAutomation;
using NUnit.Framework;

public static class DataTemplatesWindowTests
{
    private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
    private const string WindowName = "DataTemplatesWindow";

    [SetUp]
    public static void SetUp()
    {
        if (Application.TryAttach(ExeFileName, WindowName, out var app))
        {
            using (app)
            {
                var window = app.MainWindow;
                window.FindTextBoxes()[0].Text = "0";
                window.FindTextBoxes()[2].Text = "0";
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
        var typeNode = window.FindGroupBox("Node").FindTextBlock("NodeTypeTextBlock");
        Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", typeNode.Text);

        window.FindTextBoxes()[0].Text = "a";
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", typeNode.Text);

        window.FindTextBoxes()[0].Text = "1";
        Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", typeNode.Text);
    }

    [Test]
    public static void AddThenRemoveError()
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

        window.FindTextBoxes()[0].Text = "a";
        var expectedErrors = new[] { "Value 'a' could not be converted." };
        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(new[] { "System.Windows.Controls.ContentPresenter" }, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindTextBoxes()[0].Text = "1";
        Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(scope.GetErrors());

        Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(node.GetErrors());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
    }

    [Test]
    public static void AddThenRemoveErrorTwice()
    {
        AddThenRemoveError();
        AddThenRemoveError();
    }

    [Test]
    public static void AddTwoErrorsThenRemoveThemOneByOne()
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

        window.FindTextBoxes()[0].Text = "a";
        var expectedErrors = new[] { "Value 'a' could not be converted." };
        var expectedChildren = new[] { "System.Windows.Controls.ContentPresenter" };

        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(expectedChildren, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindTextBoxes()[2].Text = "b";
        expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
        expectedChildren = new[] { "System.Windows.Controls.ContentPresenter", "System.Windows.Controls.ContentPresenter" };
        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 2", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(expectedChildren, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindTextBoxes()[0].Text = "1";
        expectedErrors = new[] { "Value 'b' could not be converted." };
        expectedChildren = new[] { "System.Windows.Controls.ContentPresenter" };

        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(expectedChildren, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindTextBoxes()[2].Enter("2");
        Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(scope.GetErrors());

        Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(node.GetErrors());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
    }

    [Test]
    public static void AddTwoErrorsThenThenRemoveBothAtOnce()
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

        window.FindTextBoxes()[0].Text = "a";
        var expectedErrors = new[] { "Value 'a' could not be converted." };
        var expectedChildren = new[] { "System.Windows.Controls.ContentPresenter" };
        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 1", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(expectedChildren, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindTextBoxes()[1].Text = "b";
        expectedErrors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
        expectedChildren = new[] { "System.Windows.Controls.ContentPresenter", "System.Windows.Controls.ContentPresenter" };
        Assert.AreEqual("HasError: True", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, scope.GetErrors());

        Assert.AreEqual("Children: 2", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: True", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.AreEqual(expectedErrors, node.GetErrors());
        CollectionAssert.AreEqual(expectedChildren, node.GetChildren());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ScopeNode", node.FindTextBlock("NodeTypeTextBlock").Text);

        window.FindTextBoxes()[0].Text = "1";
        Assert.AreEqual("HasError: False", scope.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(scope.GetErrors());

        Assert.AreEqual("Children: 0", node.FindTextBlock("ChildCountTextBlock").Text);
        Assert.AreEqual("HasError: False", node.FindTextBlock("HasErrorTextBlock").Text);
        CollectionAssert.IsEmpty(node.GetErrors());
        Assert.AreEqual("Gu.Wpf.ValidationScope.ValidNode", node.FindTextBlock("NodeTypeTextBlock").Text);
    }
}
