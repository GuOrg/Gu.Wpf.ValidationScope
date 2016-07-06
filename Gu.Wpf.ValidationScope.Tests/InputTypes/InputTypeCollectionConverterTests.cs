namespace Gu.Wpf.ValidationScope.Tests.InputTypes
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using NUnit.Framework;

    public class InputTypeCollectionConverterTests
    {
        private IReadOnlyList<HappyPathData> HappyPathSource { get; } = new HappyPathData[]
        {
            new HappyPathData(typeof(TextBox).Name, new[] {typeof(TextBox)}),
            new HappyPathData("TextBox ComboBox", new[] {typeof(TextBox), typeof(ComboBox)}),
            new HappyPathData(typeof(TextBox).FullName, new[] {typeof(TextBox)}),
        };

        [TestCaseSource(nameof(HappyPathSource))]
        public void ConvertHappyPath(HappyPathData data)
        {
            var converter = new InputTypeCollectionConverter();
            var actual = (InputTypeCollection)converter.ConvertFrom(data.Text);
            CollectionAssert.AreEqual(data.Expected, actual);
        }

        [TestCase(typeof(string))]
        public void CanConvertFrom(Type type)
        {
            var converter = new InputTypeCollectionConverter();
            Assert.AreEqual(true, converter.CanConvertFrom(null, type));
        }

        public class HappyPathData
        {
            public readonly string Text;
            public readonly IReadOnlyList<Type> Expected;

            public HappyPathData(string text, IReadOnlyList<Type> expected)
            {
                this.Text = text;
                this.Expected = expected;
            }

            public override string ToString() => this.Text;
        }
    }
}
