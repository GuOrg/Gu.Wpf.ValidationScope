namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class EventsOrderWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "EventsOrderWindow";

        [Test]
        public void Validation()
        {
            // this is used as reference
            var groupBox = this.Window.FindGroupBox("Validation events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox = this.Window.FindTextBox("ValidationTextBox");
            textBox.Text = "a";
            expected.AddRange(
                new[]
                    {
                        "ValidationError: Value 'a' could not be converted.",
                        "HasError: True",
                        "Action: Added Error: Value 'a' could not be converted. Source: ValidationTextBox OriginalSource: ValidationTextBox"
                    });

            actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "1";
            expected.AddRange(new[]
                           {
                               "HasError: False",
                               "Empty",
                               "Action: Removed Error: Value 'a' could not be converted. Source: ValidationTextBox OriginalSource: ValidationTextBox"
                           });

            actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeTextBox()
        {
            var groupBox = this.Window.FindGroupBox("Scope textbox events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox = this.Window.FindTextBox("ScopeTextBox");
            textBox.Text = "a";
            expected.AddRange(new[]
                               {
                                   "ValidationError: Value 'a' could not be converted.",
                                   "HasError: True",
                                   "Action: Added Error: Value 'a' could not be converted. Source: ScopeTextBox OriginalSource: ScopeTextBox"
                               });

            actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "1";
            expected.AddRange(new[]
                           {
                               "HasError: False",
                               "Empty",
                               "Action: Removed Error: Value 'a' could not be converted. Source: ScopeTextBox OriginalSource: ScopeTextBox"
                           });

            actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeGroupBoxOneError()
        {
            this.RestartApplication();
            var groupBox = this.Window.FindGroupBox("Scope events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox1 = this.Window.FindGroupBox("ScopeGroupBox").FindTextBox("ScopeTextBox1");
            textBox1.Text = "a";
            expected.AddRange(new[]
                           {
                               "ValidationError: Value 'a' could not be converted.",
                               "HasError: True",
                               "Action: Added Error: Value 'a' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox1.Text = "1";
            expected.AddRange(new[]
                           {
                               "HasError: False",
                               "Empty",
                               "Action: Removed Error: Value 'a' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeGroupBoxTwoErrors()
        {
            this.RestartApplication();
            var groupBox = this.Window.FindGroupBox("Scope events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox1 = this.Window.FindGroupBox("ScopeGroupBox").FindTextBox("ScopeTextBox1");
            textBox1.Text = "a";
            expected.AddRange(new[]
                           {
                               "ValidationError: Value 'a' could not be converted.",
                               "HasError: True",
                               "Action: Added Error: Value 'a' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox2 = this.Window.FindGroupBox("ScopeGroupBox").FindTextBox("ScopeTextBox2");
            textBox2.Text = "b";
            expected.AddRange(new[]
                           {
                               "Action: Added Error: Value 'b' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox1.Text = "1";
            expected.AddRange(new[]
            {
                "Action: Removed Error: Value 'a' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox",
                "HasError: False",
                "Empty",
                "Action: Removed Error: Value 'b' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox",
            });

            actual = groupBox.FindTextBlocks("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }
    }
}