// ReSharper disable PossibleMultipleEnumeration
namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class OneWayToSourceBindingsWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "OneWayToSourceBindingsWindow";

        private GroupBox ViewErrorsGroupBox => window.FindGroupBox("ElementName binding");

        private GroupBox ViewModelErrorsGroupBox => window.FindGroupBox("ViewModel binding");

        private TextBox TextBox1 => window.FindTextBox("TextBox1");

        private TextBox TextBox2 => window.FindTextBox("TextBox2");

        [SetUp]
        public void SetUp()
        {
            this.TextBox1.Text = "0";
            this.TextBox2.Text = "0";
            Keyboard.Type(Key.TAB);
        }

        [Test]
        public void Updates()
        {
            var hasError = "HasError: False";
            var errors = Enumerable.Empty<string>();
            AssertErrors(this.ViewErrorsGroupBox, hasError, errors);
            AssertErrors(this.ViewModelErrorsGroupBox, hasError, errors);

            this.TextBox1.Text = "a";
            hasError = "HasError: True";
            errors = new[] { "Value 'a' could not be converted." };
            AssertErrors(this.ViewErrorsGroupBox, hasError, errors);
            AssertErrors(this.ViewModelErrorsGroupBox, hasError, errors);

            this.TextBox2.Text = "b";
            errors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            AssertErrors(this.ViewErrorsGroupBox, hasError, errors);
            AssertErrors(this.ViewModelErrorsGroupBox, hasError, errors);

            this.TextBox1.Text = "1";
            hasError = "HasError: False";
            errors = Enumerable.Empty<string>();
            AssertErrors(this.ViewErrorsGroupBox, hasError, errors);
            AssertErrors(this.ViewModelErrorsGroupBox, hasError, errors);
        }

        private static void AssertErrors(GroupBox groupBox, string hasError, IEnumerable<string> errors)
        {
            Assert.AreEqual(hasError, groupBox.FindTextBlocks()[1].Text);
            CollectionAssert.AreEqual(errors, groupBox.FindGroupBox("Errors").GetErrors());
            CollectionAssert.AreEqual(errors, groupBox.FindGroupBox("Node").GetErrors());
        }
    }
}