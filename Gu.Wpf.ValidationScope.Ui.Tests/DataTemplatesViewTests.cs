namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System.Linq;
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;

    public class DataTemplatesViewTests
    {
        [Test]
        public void Updates()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIDs.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(AutomationIDs.OneLevelScopeTab);
                page.Select();
                CollectionAssert.IsEmpty(page.GetErrors());
                var textBox1 = page.Get<TextBox>(AutomationIDs.TextBox1);
                textBox1.EnterSingle('a');
                CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, page.GetErrors());

                var textBox2 = page.Get<TextBox>(AutomationIDs.TextBox2);
                textBox2.EnterSingle('b');
                var expectedErrors = new[]
                {
                    "Value 'a' could not be converted." ,
                    "Value 'b' could not be converted."
                };
                CollectionAssert.AreEqual(expectedErrors, page.GetErrors());
                textBox1.EnterSingle('1');
                CollectionAssert.IsEmpty(page.GetErrors());
            }
        }
    }
}