namespace Gu.Wpf.ValidationScope.UiTests
{
    using System;
    using System.IO;
    using System.Reflection;
    using Gu.Wpf.UiAutomation;

    public static class Info
    {
        public static string ExeFileName { get; } = Application.FindExe("Gu.Wpf.ValidationScope.Demo.exe");

        internal static string TestAssemblyFullFileName()
        {
           return new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
        }

        internal static string TestAssemblyDirectory() => Path.GetDirectoryName(TestAssemblyFullFileName());

        internal static string ArtifactsDirectory()
        {
            //// ReSharper disable PossibleNullReferenceException
            var root = new DirectoryInfo(TestAssemblyFullFileName()).Parent.Parent.Parent.Parent.FullName;
            //// ReSharper restore PossibleNullReferenceException
            var artifacts = Path.Combine(root, "artifacts");
            Directory.CreateDirectory(artifacts);
            return artifacts;
        }
    }
}