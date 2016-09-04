namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
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
                stackPanel.SetForInputTypes(inputTypes);

                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                var errorNode = (InputNode)Scope.GetNode(textBox);

                var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                textBox.SetValidationError(validationError);

                Assert.AreEqual(true, Scope.GetHasError(stackPanel));
                Assert.IsInstanceOf<ScopeNode>(Scope.GetNode(stackPanel));

                Assert.AreEqual(true, Scope.GetHasError(textBox));
                Assert.AreSame(errorNode, Scope.GetNode(textBox));

                textBox.ClearValidationError(validationError);

                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreSame(errorNode, Scope.GetNode(textBox));
            }

            [Test]
            public void NodesForSelectorAndSlider()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(Selector), typeof(Slider) };
                stackPanel.SetForInputTypes(inputTypes);
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
                stackPanel.SetForInputTypes(inputTypes);

                Assert.AreEqual(false, Scope.GetHasError(stackPanel));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(stackPanel));

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                var inputNode = (InputNode)Scope.GetNode(textBox);
                var events = inputNode.SubscribeObservableCollectionEvents();
                var expectedEvents = new List<EventArgs>();
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
                expectedEvents.AddRange(
                    new EventArgs[]
                        {
                            new PropertyChangedEventArgs("Count"),
                            new PropertyChangedEventArgs("Item[]"),
                            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, validationError, 0),
                            new PropertyChangedEventArgs("HasErrors"),
                        });
                CollectionAssert.AreEqual(expectedEvents, events, ObservableCollectionArgsComparer.Default);

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
                expectedEvents.AddRange(
                    new EventArgs[]
                        {
                            new PropertyChangedEventArgs("Count"),
                            new PropertyChangedEventArgs("Item[]"),
                            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, validationError, 0),
                            new PropertyChangedEventArgs("HasErrors"),
                        });
                CollectionAssert.AreEqual(expectedEvents, events, ObservableCollectionArgsComparer.Default);
            }

            [Test]
            public void Notifies()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var textBoxEvents = textBox.SubscribeScopeEvents();
                var stackPanel = new StackPanel();
                var stackPanelEVents = stackPanel.SubscribeScopeEvents();
                stackPanel.Children.Add(textBox);
                var inputTypes = new InputTypeCollection { typeof(System.Windows.Controls.TextBox), typeof(Selector) };
                stackPanel.SetForInputTypes(inputTypes);
                var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                textBox.SetValidationError(validationError);
                var expectedEvents = new List<ScopeValidationErrorEventArgs>
                                         {
                                             new ScopeValidationErrorEventArgs(validationError, ValidationErrorEventAction.Added)
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
