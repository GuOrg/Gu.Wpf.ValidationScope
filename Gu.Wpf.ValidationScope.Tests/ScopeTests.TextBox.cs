namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
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
        public void UpdatesAndNotifiesError()
        {
            var textBox = new TextBox();
            var inputTypes = new InputTypeCollection { typeof(TextBox), typeof(Selector) };
            textBox.SetForInputTypes(inputTypes);
            Assert.AreEqual(false, Scope.GetHasErrors(textBox));
            var errorNode = (ErrorNode)Scope.GetErrors(textBox);
            var errorArgs = new List<EventArgs>();
            var nodeArgs = new List<EventArgs>();
            ((INotifyPropertyChanged)errorNode.Errors).PropertyChanged += (_, e) => errorArgs.Add(e);
            ((INotifyCollectionChanged)errorNode.Errors).CollectionChanged += (_, e) => errorArgs.Add(e);

            ((INotifyPropertyChanged)errorNode).PropertyChanged += (_, e) => nodeArgs.Add(e);
            ((INotifyCollectionChanged)errorNode).CollectionChanged += (_, e) => nodeArgs.Add(e);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
            CollectionAssert.IsEmpty(errorNode.Errors);

            textBox.SetValidationError();
            Assert.AreEqual(true, Scope.GetHasErrors(textBox));
            errorNode = (ErrorNode)Scope.GetErrors(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
            CollectionAssert.AreEqual(new[] { TestValidationError.GetFor(textBox) }, errorNode.Errors);
            var expectedArgs = new List<EventArgs>
                                   {
                                       new PropertyChangedEventArgs("Count"),
                                       new PropertyChangedEventArgs("Item[]"),
                                       new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, TestValidationError.GetFor(textBox), 0)
                                   };

            CollectionAssert.AreEqual(expectedArgs, errorArgs, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(expectedArgs, nodeArgs, ObservableCollectionArgsComparer.Default);

            textBox.ClearValidationError();
            Assert.AreEqual(false, Scope.GetHasErrors(textBox));
            errorNode = (ErrorNode)Scope.GetErrors(textBox);
            Assert.AreEqual(textBox, errorNode.Source);
            CollectionAssert.IsEmpty(errorNode.Children);
            CollectionAssert.IsEmpty(errorNode.Errors);
            expectedArgs.Add(new PropertyChangedEventArgs("Count"));
            expectedArgs.Add(new PropertyChangedEventArgs("Item[]"));
            expectedArgs.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, TestValidationError.GetFor(textBox), 0));
            CollectionAssert.AreEqual(new EventArgs[] { }, errorArgs, ObservableCollectionArgsComparer.Default);
            CollectionAssert.AreEqual(new EventArgs[] { }, nodeArgs, ObservableCollectionArgsComparer.Default);
        }
    }
}
