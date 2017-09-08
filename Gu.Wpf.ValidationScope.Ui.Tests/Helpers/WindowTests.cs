namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public abstract class WindowTests : IDisposable
    {
        private Application application;
        private bool disposed;

        protected Window window => this.application.MainWindow;

        protected abstract string WindowName { get; }

        public void RestartApplication()
        {
            this.OneTimeTearDown();
            this.OneTimeSetUp();
        }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            this.application?.Dispose();
            this.application = Application.AttachOrLaunch(Info.CreateStartInfo(WindowName));
            ////this.SaveScreenshotToArtifacsDir("start");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ////this.SaveScreenshotToArtifacsDir("finish");
            this.application?.Dispose();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            if (disposing)
            {
                this.application?.Dispose();
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void SaveScreenshotToArtifacsDir(string suffix)
        {
            var fileName = System.IO.Path.Combine(Info.ArtifactsDirectory(), $"{WindowName}_{suffix}.png");
            using (var image = Capture.Screen())
            {
                image.Save(fileName);
            }
        }
    }
}