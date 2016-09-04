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

    [Apartment(ApartmentState.STA)]
    public partial class ScopeTests
    {
        [Test]
        public void TextBoxSetSetForInputTypesTextBox()
        {
            var textBox = new TextBox();
            var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
            textBox.SetForInputTypes(inputTypes);
            Assert.AreEqual(false, Scope.GetHasError(textBox));
            var errorNode = (InputNode)Scope.GetNode(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
            CollectionAssert.IsEmpty(errorNode.Errors);
        }

        [Test]
        public void TextBoxSetSetForSelectorAndSlider()
        {
            var textBox = new TextBox();
            var inputTypes = new InputTypeCollection { typeof(Selector), typeof(Slider) };
            textBox.SetForInputTypes(inputTypes);
            Assert.AreEqual(false, Scope.GetHasError(textBox));
            Assert.AreEqual(ValidNode.Default, Scope.GetNode(textBox));
        }

        [Test]
        public void UpdatesAndNotifiesError()
        {
            var textBox = new TextBox();
            var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
            textBox.SetForInputTypes(inputTypes);
            Assert.AreEqual(false, Scope.GetHasError(textBox));
            var errorNode = (InputNode)Scope.GetNode(textBox);
            var errorArgs = errorNode.Errors.SubscribeObservableCollectionEvents();
            var nodeArgs = errorNode.SubscribeObservableCollectionEvents();

            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
            CollectionAssert.IsEmpty(errorNode.Errors);
            var validationError = TestValidationError.GetFor(textBox, TextBox.TextProperty);
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
                                       new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,validationError, 0)
                                   };
            CollectionAssert.AreEqual(expectedErrorArgs, errorArgs, ObservableCollectionArgsComparer.Default);
            var expectedNodeArgs = new List<EventArgs>
                                       {
                                           new PropertyChangedEventArgs("Count"),
                                           new PropertyChangedEventArgs("Item[]"),
                                           new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, validationError, 0),
                                           new PropertyChangedEventArgs("HasErrors"),
                                       };
            CollectionAssert.AreEqual(expectedNodeArgs, nodeArgs, ObservableCollectionArgsComparer.Default);

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

            expectedNodeArgs.Add(new PropertyChangedEventArgs("Count"));
            expectedNodeArgs.Add(new PropertyChangedEventArgs("Item[]"));
            expectedNodeArgs.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, validationError, 0));
            expectedNodeArgs.Add(new PropertyChangedEventArgs("HasErrors"));
            CollectionAssert.AreEqual(expectedNodeArgs, nodeArgs, ObservableCollectionArgsComparer.Default);
        }
    }
}
