namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.UIItems.TabItems;

    public class DynamicViewTests
    {
        [Test]
        public void Updates()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIDs.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(AutomationIDs.DynamicScopeTab);
                page.Select();
                var childCountBlock = page.Get<Label>(AutomationIDs.ChildCountTextBlock);
                var typesListBox = page.Get<ListBox>(AutomationIDs.TypeListBox);
                Assert.AreEqual(null, typesListBox.SelectedItem);
                Assert.AreEqual(string.Empty, childCountBlock.Text);
                CollectionAssert.IsEmpty(page.GetErrors());

                var textBox1 = page.Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.EnterSingle('a');
                Assert.AreEqual(string.Empty, childCountBlock.Text);
                CollectionAssert.IsEmpty(page.GetErrors());

                typesListBox.Select(0);
                Assert.AreEqual("Children: 1", childCountBlock.Text);
                CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, page.GetErrors());

                typesListBox.Select(1);
                Assert.AreEqual(string.Empty, childCountBlock.Text);
                CollectionAssert.IsEmpty(page.GetErrors());

                var comboBox1 = page.Get<ComboBox>(AutomationIDs.ComboBox1);
                comboBox1.EnterSingle('b');
                Assert.AreEqual("Children: 1", childCountBlock.Text);
                CollectionAssert.AreEqual(new[] { "Value 'b' could not be converted." }, page.GetErrors());

                //window.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                //typesListBox.Items[0].Click();
                //Assert.AreEqual("Children: 2", childCountBlock.Text);
                //CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted.", "Value '' could not be converted." }, page.GetErrors());
            }
        }
    }
}