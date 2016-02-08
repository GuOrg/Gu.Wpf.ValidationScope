namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using NUnit.Framework;

    public partial class ScopeTests
    {
        [RequiresSTA]
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
                Assert.AreEqual(false, Scope.GetHasErrors(stackPanel));
                IErrorNode errorNode = (ScopeNode)Scope.GetErrors(stackPanel);
                Assert.AreEqual(null, errorNode);

                Assert.AreEqual(false, Scope.GetHasErrors(textBox));
                errorNode = (ErrorNode)Scope.GetErrors(textBox);
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
                Assert.AreEqual(false, Scope.GetHasErrors(stackPanel));
                Assert.AreEqual(null, Scope.GetErrors(stackPanel));
                Assert.AreEqual(false, Scope.GetHasErrors(textBox));
                Assert.AreEqual(null, Scope.GetErrors(textBox));
            }

            [Test]
            public void UpdatesError()
            {
                var textBox = new TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
                stackPanel.SetForInputTypes(inputTypes);

                Assert.AreEqual(false, Scope.GetHasErrors(stackPanel));
                IErrorNode errorNode = (ScopeNode)Scope.GetErrors(stackPanel);
                Assert.AreEqual(null, errorNode);

                Assert.AreEqual(false, Scope.GetHasErrors(textBox));
                errorNode = (ErrorNode)Scope.GetErrors(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);

                textBox.SetValidationError();
                Assert.AreEqual(true, Scope.GetHasErrors(stackPanel));
                errorNode = (ScopeNode)Scope.GetErrors(stackPanel);
                Assert.AreEqual(stackPanel, errorNode.Source);
                Assert.AreEqual(1, errorNode.Children.Count);

                Assert.AreEqual(true, Scope.GetHasErrors(textBox));
                errorNode = (ErrorNode)Scope.GetErrors(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);

                textBox.ClearValidationError();
                Assert.AreEqual(false, Scope.GetHasErrors(stackPanel));
                errorNode = (ScopeNode)Scope.GetErrors(stackPanel);
                Assert.AreEqual(null, errorNode);

                Assert.AreEqual(false, Scope.GetHasErrors(textBox));
                errorNode = (ErrorNode)Scope.GetErrors(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
            }

            [Test]
            public void Errors()
            {
                var textBox = new TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
                stackPanel.SetForInputTypes(inputTypes);

                Assert.AreEqual(false, Scope.GetHasErrors(stackPanel));
                IErrorNode errorNode = (ScopeNode)Scope.GetErrors(stackPanel);
                Assert.AreEqual(null, errorNode);

                Assert.AreEqual(false, Scope.GetHasErrors(textBox));
                errorNode = (ErrorNode)Scope.GetErrors(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
                CollectionAssert.IsEmpty(errorNode.Errors);

                textBox.SetValidationError();
                Assert.AreEqual(true, Scope.GetHasErrors(stackPanel));
                errorNode = (ScopeNode)Scope.GetErrors(stackPanel);
                Assert.AreEqual(stackPanel, errorNode.Source);
                Assert.AreEqual(1, errorNode.Children.Count);
                CollectionAssert.AreEqual(new[] { TestValidationError.GetFor(textBox) }, errorNode.Errors);

                Assert.AreEqual(true, Scope.GetHasErrors(textBox));
                errorNode = (ErrorNode)Scope.GetErrors(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
                CollectionAssert.AreEqual(new[] { TestValidationError.GetFor(textBox) }, errorNode.Errors);

                textBox.ClearValidationError();
                Assert.AreEqual(false, Scope.GetHasErrors(stackPanel));
                errorNode = (ScopeNode)Scope.GetErrors(stackPanel);
                Assert.AreEqual(null, errorNode);

                Assert.AreEqual(false, Scope.GetHasErrors(textBox));
                errorNode = (ErrorNode)Scope.GetErrors(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
                CollectionAssert.IsEmpty(errorNode.Errors);
            }
        }
    }
}
