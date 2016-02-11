namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;

    public class DataGridViewTests
    {
        [Test]
        public void Updates()
        {
            using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
            {
                var window = app.GetWindow(AutomationIDs.MainWindow, InitializeOption.NoCache);
                var page = window.Get<TabPage>(AutomationIDs.DataGridScopeTab);
                page.Select();
                CollectionAssert.IsEmpty(page.GetErrors());

                var dataGrid = page.Get<ListView>(AutomationIDs.DataGrid);
                var cell = dataGrid.Rows[0].Cells[0];
                cell.Click();
                cell.Enter("a");
                page.Get<Button>(AutomationIDs.LoseFocusButton).Click();
                CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, page.GetErrors());

                Assert.Inconclusive("Could not get below to work");
                cell.Click();
                cell.Enter("2");
                page.Get<Button>(AutomationIDs.LoseFocusButton).Click();
                CollectionAssert.IsEmpty(page.GetErrors());
            }
        }
    }
}