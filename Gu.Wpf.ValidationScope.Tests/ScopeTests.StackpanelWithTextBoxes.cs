namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Threading;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using NUnit.Framework;

    public partial class ScopeTests
    {
        [Apartment(ApartmentState.STA)]
        public class StackpanelWithTextBoxes
        {
            [Test]
            public void TextBoxSetSetForInputTypesTextBox()
            {
                var textBox = new TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
                stackPanel.SetForInputTypes(inputTypes);
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                var errorNode = (InputNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
                CollectionAssert.IsEmpty(errorNode.Errors);
            }

            [Test]
            public void TextBoxSetSetForInputTypes()
            {
                var textBox = new TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(Selector), typeof(Slider) };
                stackPanel.SetForInputTypes(inputTypes);
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));
            }

            [Test]
            public void Updates()
            {
                var textBox = new TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
                stackPanel.SetForInputTypes(inputTypes);

                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                var inputNode = (InputNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, inputNode.Source);
                CollectionAssert.IsEmpty(inputNode.Children);
                CollectionAssert.IsEmpty(inputNode.Errors);
                var validationError = TestValidationError.GetFor(textBox, TextBox.TextProperty);
                textBox.SetValidationError(validationError);
                Assert.AreEqual(true, Scope.GetHasError(stackPanel));
                var scopeNode = (ScopeNode)Scope.GetNode(stackPanel);
                Assert.AreEqual(stackPanel, scopeNode.Source);
                Assert.AreEqual(1, scopeNode.Children.Count);
                CollectionAssert.AreEqual(new[] { validationError }, scopeNode.Errors);

                Assert.AreEqual(true, Scope.GetHasError(textBox));
                inputNode = (InputNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, inputNode.Source);
                CollectionAssert.IsEmpty(inputNode.Children);
                CollectionAssert.AreEqual(new[] { validationError }, inputNode.Errors);

                textBox.ClearValidationError(validationError);
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                inputNode = (InputNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, inputNode.Source);
                CollectionAssert.IsEmpty(inputNode.Children);
                CollectionAssert.IsEmpty(inputNode.Errors);
            }
        }
    }
}
