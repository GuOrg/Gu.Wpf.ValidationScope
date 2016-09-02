namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using NUnit.Framework;

    using TestStack.White;
    using TestStack.White.UIItems.WindowItems;
    using TestStack.White.WindowsAPI;

    public abstract class WindowTests
    {
        private Application application;

        protected Window Window { get; private set; }

        protected abstract string WindowName { get; }

        public void RestartApplication()
        {
            this.OneTimeTearDown();
            this.OneTimeSetUp();
        }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            this.application = Application.AttachOrLaunch(Info.CreateStartInfo(this.WindowName));
            this.Window = this.application.GetWindow(this.WindowName);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.Window?.Keyboard.PressAndLeaveSpecialKey(KeyboardInput.SpecialKeys.CONTROL);
            this.Window?.Keyboard.PressAndLeaveSpecialKey(KeyboardInput.SpecialKeys.SHIFT);
            this.application?.Dispose();
        }
    }
}