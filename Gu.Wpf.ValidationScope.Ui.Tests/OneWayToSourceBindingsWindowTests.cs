// ReSharper disable PossibleMultipleEnumeration
namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class OneWayToSourceBindingsWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "OneWayToSourceBindingsWindow";

        public GroupBox ViewErrorsGroupBox => this.Window.GetByText<GroupBox>("ElementName binding");

        public GroupBox ViewModelErrorsGroupBox => this.Window.GetByText<GroupBox>("ViewModel binding");

        public TextBox TextBox1 => this.Window.Get<TextBox>("TextBox1");

        public TextBox TextBox2 => this.Window.Get<TextBox>("TextBox2");

        [SetUp]
        public void SetUp()
        {
            this.TextBox1.Enter("0");
            this.TextBox2.Enter("0");
            this.Window.WaitWhileBusy();
        }

        [Test]
        public void Updates()
        {
            var hasError = "HasError: False";
            var errors = Enumerable.Empty<string>();
            AssertErrors(this.ViewErrorsGroupBox, hasError, errors);
            AssertErrors(this.ViewModelErrorsGroupBox, hasError, errors);

            this.TextBox1.Enter('a');
            hasError = "HasError: True";
            errors = new[] { "Value 'a' could not be converted." };
            AssertErrors(this.ViewErrorsGroupBox, hasError, errors);
            AssertErrors(this.ViewModelErrorsGroupBox, hasError, errors);

            this.TextBox2.Enter('b');
            errors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            AssertErrors(this.ViewErrorsGroupBox, hasError, errors);
            AssertErrors(this.ViewModelErrorsGroupBox, hasError, errors);

            this.TextBox1.Enter('1');
            hasError = "HasError: False";
            errors = Enumerable.Empty<string>();
            AssertErrors(this.ViewErrorsGroupBox, hasError, errors);
            AssertErrors(this.ViewModelErrorsGroupBox, hasError, errors);
        }

        private static void AssertErrors(GroupBox groupBox, string hasError, IEnumerable<string> errors)
        {
            Assert.AreEqual(hasError, groupBox.GetByIndex<Label>(1).Text);
            CollectionAssert.AreEqual(errors, groupBox.GetByText<GroupBox>("Errors").GetErrors());
            CollectionAssert.AreEqual(errors, groupBox.GetByText<GroupBox>("Node").GetErrors());
        }
    }
}