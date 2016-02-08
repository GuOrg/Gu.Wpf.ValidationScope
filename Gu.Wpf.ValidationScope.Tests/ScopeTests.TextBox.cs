namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using NUnit.Framework;

    [RequiresSTA]
    public partial class ScopeTests
    {
        [Test]
        public void TextBoxSetSetForInputTypesTextBox()
        {
            var textBox = new TextBox();
            var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
            textBox.SetForInputTypes(inputTypes);
            Assert.AreEqual(false, Scope.GetHasErrors(textBox));
            var errorNode = (ErrorNode)Scope.GetErrors(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
        }

        [Test]
        public void TextBoxSetSetForInputTypes()
        {
            var textBox = new TextBox();
            var inputTypes = new InputTypeCollection { typeof(Selector), typeof(Slider) };
            textBox.SetForInputTypes(inputTypes);
            Assert.AreEqual(false, Scope.GetHasErrors(textBox));
            Assert.AreEqual(null, Scope.GetErrors(textBox));
        }

        [Test]
        public void UpdatesError()
        {
            var textBox = new TextBox();
            var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
            textBox.SetForInputTypes(inputTypes);
            Assert.AreEqual(false, Scope.GetHasErrors(textBox));
            var errorNode = (ErrorNode)Scope.GetErrors(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);

            textBox.SetValidationError();
            Assert.AreEqual(true, Scope.GetHasErrors(textBox));
            errorNode = (ErrorNode)Scope.GetErrors(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);

            textBox.ClearValidationError();
            Assert.AreEqual(false, Scope.GetHasErrors(textBox));
            errorNode = (ErrorNode)Scope.GetErrors(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
        }

        [Test]
        public void Reminder()
        {
            var textBox = new TextBox();
            var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
            textBox.SetForInputTypes(inputTypes);
            Assert.AreEqual(false, Scope.GetHasErrors(textBox));
            var errorNode = (ErrorNode)Scope.GetErrors(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
            Assert.Fail("Implement");
            // CollectionAssert.IsEmpty(errorNode.Errors);

            textBox.SetValidationError();
            Assert.AreEqual(true, Scope.GetHasErrors(textBox));
            errorNode = (ErrorNode)Scope.GetErrors(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
            // CollectionAssert.AreEqual(new[] {TestValidationError.GetFor(textBox)} ,errorNode.Errors);

            textBox.ClearValidationError();
            Assert.AreEqual(false, Scope.GetHasErrors(textBox));
            errorNode = (ErrorNode)Scope.GetErrors(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
            // CollectionAssert.IsEmpty(errorNode.Errors);
        }
    }
}
