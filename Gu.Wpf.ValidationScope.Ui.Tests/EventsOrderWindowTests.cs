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
            var groupBox = this.Window.GetByText<GroupBox>("Scope events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox1 = this.Window.Get<GroupBox>("ScopeGroupBox").Get<TextBox>("ScopeTextBox1");
            textBox1.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected.AddRange(new[]
                           {
                               "Action: Added Error: Value 'g' could not be converted. Source: ScopeTextBox1 OriginalSource: ScopeTextBox1",
                               "Action: Added Error: Value 'g' could not be converted. Source: ScopeStackPanel OriginalSource: ScopeStackPanel",
                               "Action: Added Error: Value 'g' could not be converted. Source:  OriginalSource: ",
                               "Action: Added Error: Value 'g' could not be converted. Source:  OriginalSource: ",
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
                               "Action: Removed Error: Value 'g' could not be converted. Source: ScopeTextBox1 OriginalSource: ScopeTextBox1",
                               "Action: Removed Error: Value 'g' could not be converted. Source: ScopeStackPanel OriginalSource: ScopeStackPanel",
                               "Action: Removed Error: Value 'g' could not be converted. Source:  OriginalSource: ",
                               "Action: Removed Error: Value 'g' could not be converted. Source:  OriginalSource: ",
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
            var groupBox = this.Window.GetByText<GroupBox>("Scope events");
            var expected = new List<string> { "HasError: False", "Empty" };
            var actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox1 = this.Window.Get<GroupBox>("ScopeGroupBox").Get<TextBox>("ScopeTextBox1");
            textBox1.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected.AddRange( new[]
                           {
                               "Action: Added Error: Value 'g' could not be converted. Source: ScopeTextBox1 OriginalSource: ScopeTextBox1",
                               "Action: Added Error: Value 'g' could not be converted. Source: ScopeStackPanel OriginalSource: ScopeStackPanel",
                               "Action: Added Error: Value 'g' could not be converted. Source:  OriginalSource: ",
                               "Action: Added Error: Value 'g' could not be converted. Source:  OriginalSource: ",
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
                               "Action: Added Error: Value 'h' could not be converted. Source: ScopeTextBox2 OriginalSource: ScopeTextBox2",
                               "Action: Added Error: Value 'h' could not be converted. Source: ScopeStackPanel OriginalSource: ScopeStackPanel",
                               "Action: Added Error: Value 'h' could not be converted. Source:  OriginalSource: ",
                               "Action: Added Error: Value 'h' could not be converted. Source:  OriginalSource: ",
                               "Action: Added Error: Value 'h' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            textBox1.Text = "1";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);

            expected.AddRange(new[]
                           {
                               "Action: Removed Error: Value 'g' could not be converted. Source: ScopeTextBox1 OriginalSource: ScopeTextBox1",
                               "Action: Removed Error: Value 'g' could not be converted. Source: ScopeStackPanel OriginalSource: ScopeStackPanel",
                               "Action: Removed Error: Value 'g' could not be converted. Source:  OriginalSource: ",
                               "Action: Removed Error: Value 'g' could not be converted. Source:  OriginalSource: ",
                               "HasError: False",
                               "Empty",
                               "Action: Removed Error: Value 'g' could not be converted. Source: ScopeGroupBox OriginalSource: ScopeGroupBox"
                           });

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Actual: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }
    }
}