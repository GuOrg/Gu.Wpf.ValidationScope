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

    public static partial class ScopeTests
    {
        [Apartment(ApartmentState.STA)]
        public static class TextBox
        {
            [Test]
            public static void TextBoxSetSetForInputTypesTextBox()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var inputTypes = new InputTypeCollection { typeof(System.Windows.Controls.TextBox), typeof(Selector) };
                Scope.SetForInputTypes(textBox, inputTypes);
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                CollectionAssert.IsEmpty(Scope.GetErrors(textBox));

                var errorNode = (InputNode)Scope.GetNode(textBox);
                Assert.AreEqual(textBox, errorNode.Source);
                Assert.AreEqual(false, errorNode.HasError);
                CollectionAssert.IsEmpty(errorNode.Children);
                CollectionAssert.IsEmpty(errorNode.Errors);

                var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                textBox.SetValidationError(validationError);
                var expectedErrors = new[] { validationError };
                Assert.AreEqual(true, Scope.GetHasError(textBox));
                CollectionAssert.AreEqual(expectedErrors, Scope.GetErrors(textBox));
                Assert.AreSame(errorNode, Scope.GetNode(textBox));

                Assert.AreEqual(true, errorNode.HasError);
                Assert.AreEqual(textBox, errorNode.Source);
                CollectionAssert.IsEmpty(errorNode.Children);
                CollectionAssert.AreEqual(expectedErrors, errorNode.Errors);
            }

            [Test]
            public static void TextBoxSetSetForSelectorAndSlider()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var inputTypes = new InputTypeCollection { typeof(Selector), typeof(Slider) };
                Scope.SetForInputTypes(textBox, inputTypes);
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));

                var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                textBox.SetValidationError(validationError);

                Assert.AreEqual(false, Scope.GetHasError(textBox));
                Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));
            }

            [Test]
            public static void UpdatesAndNotifiesError()
            {
                var textBox = new System.Windows.Controls.TextBox();
                var inputTypes = new InputTypeCollection { typeof(System.Windows.Controls.TextBox), typeof(Selector) };
                Scope.SetForInputTypes(textBox, inputTypes);
                Assert.AreEqual(false, Scope.GetHasError(textBox));
                var errorNode = (InputNode)Scope.GetNode(textBox);
                using (var errorArgs = errorNode.Errors.SubscribeObservableCollectionEvents())
                {
                    using (var nodeArgs = errorNode.SubscribePropertyChangedEvents())
                    {
                        Assert.AreEqual(textBox, errorNode.Source);
                        CollectionAssert.IsEmpty(errorNode.Children);
                        CollectionAssert.IsEmpty(errorNode.Errors);
                        var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                        textBox.SetValidationError(validationError);
                        Assert.AreEqual(true, Scope.GetHasError(textBox));
                        errorNode = (InputNode)Scope.GetNode(textBox);
                        Assert.AreEqual(textBox, errorNode.Source);
                        CollectionAssert.IsEmpty(errorNode.Children);
                        CollectionAssert.AreEqual(new[] { validationError }, errorNode.Errors);

                        var expectedErrorArgs = new List<EventArgs>
                        {
                            new PropertyChangedEventArgs("Count"),
                            new PropertyChangedEventArgs("Item[]"),
                            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, validationError, 0),
                        };
                        CollectionAssert.AreEqual(expectedErrorArgs, errorArgs, ObservableCollectionArgsComparer.Default);
                        CollectionAssert.AreEqual(new[] { new PropertyChangedEventArgs(nameof(Node.HasError)) }, nodeArgs, PropertyChangedEventArgsComparer.Default);

                        textBox.ClearValidationError(validationError);
                        Assert.AreEqual(false, Scope.GetHasError(textBox));
                        Assert.AreSame(errorNode, Scope.GetNode(textBox));
                        Assert.AreEqual(textBox, errorNode.Source);
                        CollectionAssert.IsEmpty(errorNode.Children);
                        CollectionAssert.IsEmpty(errorNode.Errors);

                        expectedErrorArgs.Add(new PropertyChangedEventArgs("Count"));
                        expectedErrorArgs.Add(new PropertyChangedEventArgs("Item[]"));
                        expectedErrorArgs.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, validationError, 0));
                        CollectionAssert.AreEqual(expectedErrorArgs, errorArgs, ObservableCollectionArgsComparer.Default);

                        CollectionAssert.AreEqual(new[] { new PropertyChangedEventArgs(nameof(Node.HasError)), new PropertyChangedEventArgs(nameof(Node.HasError)) }, nodeArgs, PropertyChangedEventArgsComparer.Default);
                    }
                }
            }

            [Test]
            public static void Notifies()
            {
                var textBox = new System.Windows.Controls.TextBox();
                using (var textBoxEvents = textBox.SubscribeScopeEvents())
                {
                    Scope.SetForInputTypes(textBox, new InputTypeCollection { typeof(System.Windows.Controls.TextBox), typeof(Selector) });
                    var validationError = TestValidationError.GetFor(textBox, System.Windows.Controls.TextBox.TextProperty);
                    textBox.SetValidationError(validationError);
                    var expectedEvents = new List<ScopeValidationErrorEventArgs>
                    {
                        new ScopeValidationErrorEventArgs(validationError, ValidationErrorEventAction.Added),
                    };
                    CollectionAssert.AreEqual(expectedEvents, textBoxEvents, ScopeValidationErrorEventArgsComparer.Default);

                    textBox.ClearValidationError(validationError);
                    expectedEvents.Add(new ScopeValidationErrorEventArgs(validationError, ValidationErrorEventAction.Removed));
                    CollectionAssert.AreEqual(expectedEvents, textBoxEvents, ScopeValidationErrorEventArgsComparer.Default);
                }
            }
        }
    }
}
