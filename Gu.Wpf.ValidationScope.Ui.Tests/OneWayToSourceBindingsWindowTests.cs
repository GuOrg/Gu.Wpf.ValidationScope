// ReSharper disable PossibleMultipleEnumeration
namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using TestStack.White.UIItems;

    [Explicit("don't thinks we want this mess")]
    public class OneWayToSourceBindingsWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "OneWayToSourceBindingsWindow";

        [Test]
        public void Updates()
        {
            var viewErrorsGroupBox = this.Window.GetByText<GroupBox>("ElementName binding");
            var viewModelErrorsGroupBox = this.Window.GetByText<GroupBox>("ViewModel binding");
            var hasError = "HasError: False";
            var errors = Enumerable.Empty<string>();
            AssertErrors(viewErrorsGroupBox, hasError, errors);
            AssertErrors(viewModelErrorsGroupBox, hasError, errors);

            var textBox1 = this.Window.Get<TextBox>("TextBox1");
            textBox1.Enter('a');
            hasError = "HasError: True";
            errors = new[] { "Value 'a' could not be converted." };
            AssertErrors(viewErrorsGroupBox, hasError, errors);
            AssertErrors(viewModelErrorsGroupBox, hasError, errors);

            var textBox2 = this.Window.Get<TextBox>("TextBox2");
            textBox2.Enter('b');
            errors = new[] { "Value 'a' could not be converted.", "Value 'b' could not be converted." };
            AssertErrors(viewErrorsGroupBox, hasError, errors);
            AssertErrors(viewModelErrorsGroupBox, hasError, errors);

            textBox1.Enter('1');
            hasError = "HasError: False";
            errors = Enumerable.Empty<string>();
            AssertErrors(viewErrorsGroupBox, hasError, errors);
            AssertErrors(viewModelErrorsGroupBox, hasError, errors);
        }

        private static void AssertErrors(GroupBox groupBox, string hasError, IEnumerable<string> errors)
        {
            Assert.AreEqual(hasError, groupBox.GetByIndex<Label>(1).Text);
            CollectionAssert.AreEqual(errors, groupBox.GetByText<GroupBox>("Errors").GetErrors());
            CollectionAssert.AreEqual(errors, groupBox.GetByText<GroupBox>("Node").GetErrors());
        }
    }
}