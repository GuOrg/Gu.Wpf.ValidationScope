﻿namespace Gu.Wpf.ValidationScope.UiTests;

using Gu.Wpf.UiAutomation;
using NUnit.Framework;

public static class OneWayToSourceBindingsWindowTests
{
    private const string ExeFileName = "Gu.Wpf.ValidationScope.Demo.exe";
    private const string WindowName = "OneWayToSourceBindingsWindow";

    [SetUp]
    public static void SetUp()
    {
        if (Application.TryAttach(ExeFileName, WindowName, out var app))
        {
            using (app)
            {
                var window = app.MainWindow;
                window.FindTextBox("TextBox1").Text = "0";
                window.FindTextBox("TextBox2").Text = "0";
                Keyboard.Type(Key.TAB);
            }
        }
    }

    [OneTimeTearDown]
    public static void OneTimeTearDown() => Application.KillLaunched(ExeFileName);

    [Test]
    public static void Updates()
    {
        using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
        var window = app.MainWindow;
        var elementName = window.FindGroupBox("ElementName binding");
        var viewModel = window.FindGroupBox("ViewModel binding");

        var hasError = "HasError: False";
        Assert.AreEqual(hasError, elementName.FindTextBlocks()[1].Text);
        CollectionAssert.IsEmpty(elementName.FindGroupBox("Errors").GetErrors());
        CollectionAssert.IsEmpty(elementName.FindGroupBox("Node").GetErrors());

        Assert.AreEqual(hasError, viewModel.FindTextBlocks()[1].Text);
        CollectionAssert.IsEmpty(viewModel.FindGroupBox("Errors").GetErrors());
        CollectionAssert.IsEmpty(viewModel.FindGroupBox("Node").GetErrors());

        window.FindTextBox("TextBox1").Text = "a";
        hasError = "HasError: True";
        var errors = new[] { "Value 'a' could not be converted." };
        Assert.AreEqual(hasError, elementName.FindTextBlocks()[1].Text);
        CollectionAssert.AreEqual(errors, elementName.FindGroupBox("Errors").GetErrors());
        CollectionAssert.AreEqual(errors, elementName.FindGroupBox("Node").GetErrors());

        Assert.AreEqual(hasError, viewModel.FindTextBlocks()[1].Text);
        CollectionAssert.AreEqual(errors, viewModel.FindGroupBox("Errors").GetErrors());
        CollectionAssert.AreEqual(errors, viewModel.FindGroupBox("Node").GetErrors());

        window.FindTextBox("TextBox2").Text = "b";
        errors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
        Assert.AreEqual(hasError, elementName.FindTextBlocks()[1].Text);
        CollectionAssert.AreEqual(errors, elementName.FindGroupBox("Errors").GetErrors());
        CollectionAssert.AreEqual(errors, elementName.FindGroupBox("Node").GetErrors());

        Assert.AreEqual(hasError, viewModel.FindTextBlocks()[1].Text);
        CollectionAssert.AreEqual(errors, viewModel.FindGroupBox("Errors").GetErrors());
        CollectionAssert.AreEqual(errors, viewModel.FindGroupBox("Node").GetErrors());

        window.FindTextBox("TextBox1").Text = "1";
        hasError = "HasError: False";
        Assert.AreEqual(hasError, elementName.FindTextBlocks()[1].Text);
        CollectionAssert.IsEmpty(elementName.FindGroupBox("Errors").GetErrors());
        CollectionAssert.IsEmpty(elementName.FindGroupBox("Node").GetErrors());

        Assert.AreEqual(hasError, viewModel.FindTextBlocks()[1].Text);
        CollectionAssert.IsEmpty(viewModel.FindGroupBox("Errors").GetErrors());
        CollectionAssert.IsEmpty(viewModel.FindGroupBox("Node").GetErrors());
    }
}
