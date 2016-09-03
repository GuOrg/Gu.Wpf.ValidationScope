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
                IErrorNode errorNode = (ScopeNode)Scope.GetNode(stackPanel);
                Assert.AreEqual(null, errorNode);

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                errorNode = (ErrorNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
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
                Assert.AreEqual(null, Scope.GetNode(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreEqual(null, Scope.GetNode(textBox));
            }

            [Test]
            public void UpdatesError()
            {
                var textBox = new TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
                stackPanel.SetForInputTypes(inputTypes);

                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                IErrorNode errorNode = (ScopeNode)Scope.GetNode(stackPanel);
                Assert.AreEqual(null, errorNode);

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                errorNode = (ErrorNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
                CollectionAssert.IsEmpty(errorNode.Errors);

                textBox.SetValidationError();
                Assert.AreEqual(true, Scope.GetHasError(stackPanel));
                errorNode = (ScopeNode)Scope.GetNode(stackPanel);
                Assert.AreEqual(stackPanel, errorNode.Source);
                Assert.AreEqual(1, errorNode.Children.Count);
                CollectionAssert.AreEqual(new[] { ValidationErrorFactory.GetFor(textBox) }, errorNode.Errors);

                Assert.AreEqual(true, Scope.GetHasError(textBox));
                errorNode = (ErrorNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
                CollectionAssert.AreEqual(new[] { ValidationErrorFactory.GetFor(textBox) }, errorNode.Errors);

                textBox.ClearValidationError();
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                errorNode = (ScopeNode)Scope.GetNode(stackPanel);
                Assert.AreEqual(null, errorNode);

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                errorNode = (ErrorNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
                CollectionAssert.IsEmpty(errorNode.Errors);
            }
        }
    }
}
