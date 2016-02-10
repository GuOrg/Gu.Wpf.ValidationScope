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
                var keyboard = window.Keyboard;
                var page = window.Get<TabPage>(AutomationIDs.DynamicScopeTab);
                page.Select();
                var typesListBox = page.Get<ListBox>(AutomationIDs.TypeListBox);
                Assert.AreEqual(null, typesListBox.SelectedItem);
                CollectionAssert.IsEmpty(page.GetErrors());
                var textBox1 = page.Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.EnterSingle('a');
                CollectionAssert.IsEmpty(page.GetErrors());
                typesListBox.Select(0);
                CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, page.GetErrors());
                typesListBox.Select(1);
                CollectionAssert.IsEmpty(page.GetErrors());
            }
        }
    }
}