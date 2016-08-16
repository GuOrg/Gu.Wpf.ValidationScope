namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using Gu.Wpf.ValidationScope.Demo;
    using NUnit.Framework;

    using TestStack.White.UIItems;
    using TestStack.White.WindowsAPI;

    public class DataGridWindowTests : WindowTests
    {
        protected override string WindowName { get; } = "DataGridWindow";

        [Test]
        public void Updates()
        {
            CollectionAssert.IsEmpty(this.Window.GetErrors());

            var dataGrid = this.Window.Get<ListView>(AutomationIDs.DataGrid);
            var cell = dataGrid.Rows[0].Cells[0];
            cell.Click();
            cell.Enter("a");
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            CollectionAssert.AreEqual(new[] { "Value 'a' could not be converted." }, this.Window.GetErrors());

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.SHIFT, KeyboardInput.SpecialKeys.TAB);
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.BACKSPACE);
            cell.Enter("2");
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            CollectionAssert.IsEmpty(this.Window.GetErrors());
        }
    }
}