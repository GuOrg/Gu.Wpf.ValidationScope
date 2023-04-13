namespace Gu.Wpf.ValidationScope.Tests.InputTypes;

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using NUnit.Framework;

public static class InputTypeCollectionConverterTests
{
    public static IReadOnlyList<TestCaseData> TestCases { get; } = new[]
    {
        new TestCaseData(nameof(TextBox), new[] { typeof(TextBox) }),
        new TestCaseData("TextBox ComboBox", new[] { typeof(TextBox), typeof(ComboBox) }),
        new TestCaseData(typeof(TextBox).FullName, new[] { typeof(TextBox) }),
    };

    [TestCaseSource(nameof(TestCases))]
    public static void ConvertHappyPath(string text, Type[] expected)
    {
        var converter = new InputTypeCollectionConverter();
        var actual = (InputTypeCollection)converter.ConvertFrom(text)!;
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestCase("Visual3D")]
    public static void ConvertFailsFor(string name)
    {
        var converter = new InputTypeCollectionConverter();
        var exception = Assert.Throws<InvalidOperationException>(() => converter.ConvertFrom(name));
        Assert.AreEqual("Did not find a match for for Visual3D", exception!.Message);
    }

    [TestCase(typeof(string))]
    public static void CanConvertFrom(Type type)
    {
        var converter = new InputTypeCollectionConverter();
        Assert.AreEqual(true, converter.CanConvertFrom(null!, type));
    }
}
