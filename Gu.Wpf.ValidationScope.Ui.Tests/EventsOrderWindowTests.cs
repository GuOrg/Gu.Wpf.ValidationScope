namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using TestStack.White.UIItems;
    using TestStack.White.WindowsAPI;

    public class EventsOrderWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "EventsOrderWindow";

        [Test]
        public void Validation()
        {
            // this is used as reference
            var groupBox = this.Window.GetByText<GroupBox>("Validation events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox = this.Window.Get<TextBox>("ValidationTextBox");
            textBox.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected.AddRange(
                new[]
                    {
                        "ValidationError: Value 'g' could not be converted.",
                        "HasError: True",
                        "Action: Added Error: Value 'g' could not be converted. Source: ValidationTextBox OriginalSource: ValidationTextBox"
                    });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "1";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            expected.AddRange(new[]
                           {
                               "HasError: False",
                               "Empty",
                               "Action: Removed Error: Value 'g' could not be converted. Source: ValidationTextBox OriginalSource: ValidationTextBox"
                           });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeTextBox()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Scope textbox events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox = this.Window.Get<TextBox>("ScopeTextBox");
            textBox.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected.AddRange(new[]
                               {
                                   "ValidationError: Value 'g' could not be converted.",
                                   "HasError: True",
                                   "Action: Added Error: Value 'g' could not be converted. Source: ScopeTextBox OriginalSource: ScopeTextBox"
                               });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox.Text = "1";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            expected.AddRange(new[]
                           {
                               "HasError: False",
                               "Empty",
                               "Action: Removed Error: Value 'g' could not be converted. Source: ScopeTextBox OriginalSource: ScopeTextBox"
                           });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeGroupBoxOneError()
        {
            this.RestartApplication();
            var groupBox = this.Window.GetByText<GroupBox>("Scope events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox1 = this.Window.Get<GroupBox>("ScopeGroupBox").Get<TextBox>("ScopeTextBox1");
            textBox1.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected.AddRange(new[]
                           {
                               "ValidationError: Value 'g' could not be converted.",
                               "HasError: True",
                               "Action: Added Error: Value 'g' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox1.Text = "1";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            expected.AddRange( new[]
                           {
                               "HasError: False",
                               "Empty",
                               "Action: Removed Error: Value 'g' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeGroupBoxTwoErrors()
        {
            this.RestartApplication();
            var groupBox = this.Window.GetByText<GroupBox>("Scope events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox1 = this.Window.Get<GroupBox>("ScopeGroupBox").Get<TextBox>("ScopeTextBox1");
            textBox1.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected.AddRange( new[]
                           {
                               "ValidationError: Value 'g' could not be converted.",
                               "HasError: True",
                               "Action: Added Error: Value 'g' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox2 = this.Window.Get<GroupBox>("ScopeGroupBox").Get<TextBox>("ScopeTextBox2");
            textBox2.EnterSingle('h');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected.AddRange(new[]
                           {
                               "Action: Added Error: Value 'h' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox1.Text = "1";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            expected.AddRange(new[]
            {
                "Action: Removed Error: Value 'g' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox",
                "HasError: False",
                "Empty",
                "Action: Removed Error: Value 'h' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox",
            });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }
    }
}