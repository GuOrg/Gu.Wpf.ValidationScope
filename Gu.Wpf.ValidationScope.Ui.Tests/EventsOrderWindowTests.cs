namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;
    using TestStack.White.WindowsAPI;

    public class EventsOrderWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "EventsOrderWindow";

        [Test]
        public void Validation()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Validation events");
            var expected = new[] { "HasError: False", "Empty" };
            var actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Expected: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox = this.Window.Get<TextBox>("ValidationTextBox");
            textBox.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected = new[]
                               {
                                   "HasError: False",
                                   "Empty",
                                   "ValidationError: Value 'g' could not be converted.",
                                   "HasError: True",
                                   "Action: Added Error: Value 'g' could not be converted."
                               };

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Expected: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeTextBox()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Scope textbox events");
            var expected = new[] { "HasError: False", "Empty" };
            var actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Expected: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox = this.Window.Get<TextBox>("ScopeTextBox");
            textBox.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected = new[]
                               {
                                   "HasError: False",
                                   "Empty",
                                   "ValidationError: Value 'g' could not be converted.",
                                   "HasError: True",
                                   "Action: Added Error: Value 'g' could not be converted."
                               };

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Expected: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }

        [Test]
        public void ScopeGroupBox()
        {
            var groupBox = this.Window.GetByText<GroupBox>("Scope events");
            var expected = new[] { "HasError: False", "Empty" };
            var actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Expected: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");

            var textBox = this.Window.Get<GroupBox>("ScopeGroupBox").Get<TextBox>(SearchCriteria.Indexed(0));
            textBox.EnterSingle('g');
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            expected = new[]
                               {
                                   "HasError: False",
                                   "Empty",
                                   "ValidationError: Value 'g' could not be converted.",
                                   "HasError: True",
                                   "Action: Added Error: Value 'g' could not be converted."
                               };

            actual = groupBox.GetMultiple<Label>("Event").Select(x => x.Text).ToArray();
            CollectionAssert.AreEqual(expected, actual, $"Expected: {string.Join(", ", actual.Select(x => "\"" + x + "\""))}");
        }
    }
}