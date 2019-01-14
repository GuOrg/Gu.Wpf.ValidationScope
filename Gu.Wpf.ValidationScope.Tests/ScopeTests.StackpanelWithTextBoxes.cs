namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using NUnit.Framework;

    public partial class ScopeTests
    {
        [Apartment(ApartmentState.STA)]
        public class StackpanelWithTextBox
        {
            [Test]
            public void NodesForInputTypesTextBox()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(System.Windows.Controls.TextBox), typeof(Selector) };
                Scope.SetForInputTypes(stackPanel, inputTypes);

                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(stackPanel));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));
                var errorNode = (InputNode)Scope.GetNode(textBox);
                Assert.AreEqual(false, errorNode.HasError);
                Assert.AreNotSame(ErrorCollection.EmptyValidationErrors, errorNode.Errors);
                CollectionAssert.IsEmpty(errorNode.Errors);
                CollectionAssert.IsEmpty(errorNode.Children);

                var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                textBox.SetValidationError(validationError);

                Assert.AreEqual(true, Scope.GetHasError(stackPanel));
                Assert.AreEqual(true, Scope.GetHasError(textBox));
                var expectedErrors = new[] { validationError };
                CollectionAssert.AreEqual(expectedErrors, Scope.GetErrors(stackPanel));
                CollectionAssert.AreEqual(expectedErrors, Scope.GetErrors(textBox));

                var scopeNode = (ScopeNode)Scope.GetNode(stackPanel);
                CollectionAssert.AreEqual(new[] { errorNode }, scopeNode.Children);
                Assert.AreSame(errorNode, Scope.GetNode(textBox));
                Assert.AreEqual(true, scopeNode.HasError);
                Assert.AreEqual(true, errorNode.HasError);
                CollectionAssert.AreEqual(expectedErrors, scopeNode.Errors);
                CollectionAssert.AreEqual(expectedErrors, errorNode.Errors);

                textBox.ClearValidationError(validationError);
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(stackPanel));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));
                Assert.AreSame(errorNode, Scope.GetNode(textBox));
                Assert.AreEqual(false, errorNode.HasError);
                Assert.AreNotSame(ErrorCollection.EmptyValidationErrors, errorNode.Errors);
                CollectionAssert.IsEmpty(errorNode.Errors);
                CollectionAssert.IsEmpty(errorNode.Children);
            }

            [Test]
            public void SetErrorTextBoxBeforeInputTypesThenSetInputTypesToNull()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);

                var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                textBox.SetValidationError(validationError);
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(stackPanel));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));

                var inputTypes = new InputTypeCollection { typeof(System.Windows.Controls.TextBox) };
                Scope.SetForInputTypes(stackPanel, inputTypes);
                Assert.AreEqual(true, Scope.GetHasError(stackPanel));
                Assert.AreEqual(true, Scope.GetHasError(textBox));
                var expectedErrors = new[] { validationError };
                CollectionAssert.AreEqual(expectedErrors, Scope.GetErrors(stackPanel));
                CollectionAssert.AreEqual(expectedErrors, Scope.GetErrors(textBox));

                var scopeNode = (ScopeNode)Scope.GetNode(stackPanel);
                var errorNode = (InputNode)Scope.GetNode(textBox);
                CollectionAssert.AreEqual(new[] { errorNode }, scopeNode.Children);
                Assert.AreSame(errorNode, Scope.GetNode(textBox));
                Assert.AreEqual(true, scopeNode.HasError);
                Assert.AreEqual(true, errorNode.HasError);
                CollectionAssert.AreEqual(expectedErrors, scopeNode.Errors);
                CollectionAssert.AreEqual(expectedErrors, errorNode.Errors);

                Scope.SetForInputTypes(stackPanel, null);
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(stackPanel));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));
            }

            [Test]
            public void SetErrorTextBoxBeforeInputTypesThenSetInputTypesToSlider()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);

                var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                textBox.SetValidationError(validationError);
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(stackPanel));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));

                Scope.SetForInputTypes(stackPanel, new InputTypeCollection { typeof(System.Windows.Controls.TextBox) });
                Assert.AreEqual(true, Scope.GetHasError(stackPanel));
                Assert.AreEqual(true, Scope.GetHasError(textBox));
                var expectedErrors = new[] { validationError };
                CollectionAssert.AreEqual(expectedErrors, Scope.GetErrors(stackPanel));
                CollectionAssert.AreEqual(expectedErrors, Scope.GetErrors(textBox));

                var scopeNode = (ScopeNode)Scope.GetNode(stackPanel);
                var errorNode = (InputNode)Scope.GetNode(textBox);
                CollectionAssert.AreEqual(new[] { errorNode }, scopeNode.Children);
                Assert.AreSame(errorNode, Scope.GetNode(textBox));
                Assert.AreEqual(true, scopeNode.HasError);
                Assert.AreEqual(true, errorNode.HasError);
                CollectionAssert.AreEqual(expectedErrors, scopeNode.Errors);
                CollectionAssert.AreEqual(expectedErrors, errorNode.Errors);

                Scope.SetForInputTypes(stackPanel, new InputTypeCollection { typeof(System.Windows.Controls.Slider) });
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(stackPanel));
                Assert.AreSame(ErrorCollection.EmptyValidationErrors, Scope.GetErrors(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));
            }

            [Test]
            public void NodesForSelectorAndSlider()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(Selector), typeof(Slider) };
                Scope.SetForInputTypes(stackPanel, inputTypes);
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));

                var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                textBox.SetValidationError(validationError);

                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));
            }

            [Test]
            public void UpdatesErrors()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(System.Windows.Controls.TextBox), typeof(Selector) };
                Scope.SetForInputTypes(stackPanel, inputTypes);

                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                var inputNode = (InputNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, inputNode.Source);
                CollectionAssert.IsEmpty(inputNode.Children);
                CollectionAssert.IsEmpty(inputNode.Errors);

                var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                textBox.SetValidationError(validationError);
                Assert.AreEqual(true, Scope.GetHasError(stackPanel));
                var scopeNode = (ScopeNode)Scope.GetNode(stackPanel);
                Assert.AreEqual(stackPanel, scopeNode.Source);
                Assert.AreEqual(1, scopeNode.Children.Count);
                CollectionAssert.AreEqual(new[] { validationError }, scopeNode.Errors);

                Assert.AreEqual(true, Scope.GetHasError(textBox));
                Assert.AreSame(inputNode, Scope.GetNode(textBox));
                Assert.AreEqual(textBox, inputNode.Source);
                CollectionAssert.IsEmpty(inputNode.Children);
                CollectionAssert.AreEqual(new[] { validationError }, inputNode.Errors);

                textBox.ClearValidationError(validationError);
                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreSame(inputNode, Scope.GetNode(textBox));
                Assert.AreEqual(textBox, inputNode.Source);
                CollectionAssert.IsEmpty(inputNode.Children);
                CollectionAssert.IsEmpty(inputNode.Errors);
            }

            [Test]
            public void Notifies()
            {
                var textBox = new System.Windows.Controls.TextBox();
                using (var textBoxEvents = textBox.SubscribeScopeEvents())
                {
                    var stackPanel = new StackPanel();
                    using (var stackPanelEVents = stackPanel.SubscribeScopeEvents())
                    {
                        stackPanel.Children.Add(textBox);
                        var inputTypes = new InputTypeCollection { typeof(System.Windows.Controls.TextBox), typeof(Selector) };
                        Scope.SetForInputTypes(stackPanel, inputTypes);
                        var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                        textBox.SetValidationError(validationError);
                        var expectedEvents = new List<ScopeValidationErrorEventArgs>
                        {
                            new ScopeValidationErrorEventArgs(validationError, ValidationErrorEventAction.Added),
                        };
                        CollectionAssert.AreEqual(expectedEvents, textBoxEvents, ScopeValidationErrorEventArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedEvents, stackPanelEVents, ScopeValidationErrorEventArgsComparer.Default);

                        textBox.ClearValidationError(validationError);
                        expectedEvents.Add(new ScopeValidationErrorEventArgs(validationError, ValidationErrorEventAction.Removed));
                        CollectionAssert.AreEqual(expectedEvents, textBoxEvents, ScopeValidationErrorEventArgsComparer.Default);
                        CollectionAssert.AreEqual(expectedEvents, stackPanelEVents, ScopeValidationErrorEventArgsComparer.Default);
                    }
                }
            }
        }
    }
}
