namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using TestStack.White.InputDevices;
    using TestStack.White.WindowsAPI;

    public static class KeyboardExt
    {
        public static void PressSpecialKey(this AttachedKeyboard keyboard, KeyboardInput.SpecialKeys holdKey, KeyboardInput.SpecialKeys specialKey)
        {
            keyboard.HoldKey(holdKey);
            keyboard.PressSpecialKey(specialKey);
            keyboard.LeaveKey(holdKey);
        }

        public static void PressAndLeaveSpecialKey(this AttachedKeyboard keyboard, KeyboardInput.SpecialKeys specialKey)
        {
            keyboard.HoldKey(specialKey);
            keyboard.LeaveKey(specialKey);
        }
    }
}
