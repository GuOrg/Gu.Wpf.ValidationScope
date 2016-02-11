namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.UIItems.TabItems;

    public class ParentChildScopesViewTests
    {
        [Test]
        public void Updates()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIDs.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(AutomationIDs.ParentChildScopesTab);
                page.Select();
                CollectionAssert.IsEmpty(page.GetErrors());
                var textBox1 = page.Get<GroupBox>(AutomationIDs.TextBoxScope).Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.EnterSingle('a');
                CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, page.GetErrors());

                var textBox2 = page.Get<GroupBox>(AutomationIDs.TextBoxScope).Get<TextBox>(AutomationIDs.TextBox2);
                textBox2.EnterSingle('b');
                var expectedErrors = new[]
                {
                    "Value 'a' could not be converted.",
                    "Value 'b' could not be converted."
                };
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());

                var comboBox1 = page.Get<GroupBox>(AutomationIDs.TextBoxScope).Get<ComboBox>(AutomationIDs.ComboBox1);
                comboBox1.EnterSingle('c');
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());

                var comboBox2 = page.Get<GroupBox>(AutomationIDs.ComboBoxScope).Get<ComboBox>(AutomationIDs.ComboBox1);
                comboBox2.EnterSingle('d');
                expectedErrors = new[]
                {
                    "Value 'a' could not be converted.",
                    "Value 'b' could not be converted.",
                    "Value 'd' could not be converted."
                };
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());

                var textBox3 = page.Get<GroupBox>(AutomationIDs.ComboBoxScope).Get<TextBox>(AutomationIDs.TextBox1);
                textBox3.EnterSingle('e');
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());

                textBox1.EnterSingle('1');
                CollectionAssert.IsEmpty(page.GetErrors());

                var textBox4 = page.Get<GroupBox>(AutomationIDs.NoScope).Get<TextBox>(AutomationIDs.TextBox1);
                textBox4.EnterSingle('e');
                CollectionAssert.IsEmpty(page.GetErrors());

                var comboBox3 = page.Get<GroupBox>(AutomationIDs.NoScope).Get<ComboBox>(AutomationIDs.ComboBox1);
                comboBox3.EnterSingle('f');
                CollectionAssert.IsEmpty(page.GetErrors());
            }
        }
    }
}
