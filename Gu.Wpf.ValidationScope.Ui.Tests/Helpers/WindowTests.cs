namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using NUnit.Framework;

    using TestStack.White;
    using TestStack.White.UIItems.WindowItems;
    using TestStack.White.WindowsAPI;

    public abstract class WindowTests
    {
        private Application application;

        public static Window StaticWindow { get; private set; }

        protected Window Window => StaticWindow;

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
            StaticWindow = this.application.GetWindow(this.WindowName);
            //this.SaveScreenshotToArtifacsDir("start");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.Window?.Keyboard.PressAndLeaveSpecialKey(KeyboardInput.SpecialKeys.CONTROL);
            this.Window?.Keyboard.PressAndLeaveSpecialKey(KeyboardInput.SpecialKeys.SHIFT);
            //this.SaveScreenshotToArtifacsDir("finish");
            this.application?.Dispose();
            StaticWindow = null;
        }

        protected void PressTab()
        {
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
        }

        // ReSharper disable once UnusedMember.Local
        private void SaveScreenshotToArtifacsDir(string suffix)
        {
            var fileName = System.IO.Path.Combine(Info.ArtifactsDirectory(), $"{this.WindowName}_{suffix}.png");
            using (var image = new TestStack.White.ScreenCapture().CaptureDesktop())
            {
                image.Save(fileName);
            }
        }
    }
}