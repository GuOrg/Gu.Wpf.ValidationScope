namespace Gu.Wpf.ValidationScope.Tests.InputTypes
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using NUnit.Framework;

    public static class InputTypeCollectionConverterTests
    {
        public static IReadOnlyList<HappyPathData> HappyPathSource { get; } = new[]
        {
            new HappyPathData(typeof(TextBox).Name, new[] { typeof(TextBox) }),
            new HappyPathData("TextBox ComboBox", new[] { typeof(TextBox), typeof(ComboBox) }),
            new HappyPathData(typeof(TextBox).FullName, new[] { typeof(TextBox) }),
        };

        [TestCaseSource(nameof(HappyPathSource))]
        public static void ConvertHappyPath(HappyPathData data)
        {
            var converter = new InputTypeCollectionConverter();
            var actual = (InputTypeCollection)converter.ConvertFrom(data.Text);
            CollectionAssert.AreEqual(data.Expected, actual);
        }

        [TestCase("Visual3D")]
        public static void ConvertFailsFor(string name)
        {
            var converter = new InputTypeCollectionConverter();
            var exception = Assert.Throws<InvalidOperationException>(() => converter.ConvertFrom(name));
            Assert.AreEqual("Did not find a match for for Visual3D", exception.Message);
        }

        [TestCase(typeof(string))]
        public static void CanConvertFrom(Type type)
        {
            var converter = new InputTypeCollectionConverter();
            Assert.AreEqual(true, converter.CanConvertFrom(null!, type));
        }

        public class HappyPathData
        {
            public HappyPathData(string text, IReadOnlyList<Type> expected)
            {
                this.Text = text;
                this.Expected = expected;
            }

            public string Text { get; }

            public IReadOnlyList<Type> Expected { get; }

            public override string ToString() => this.Text;
        }
    }
}
