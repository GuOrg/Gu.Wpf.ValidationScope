namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public static class Info
    {
        public static string ExeFileName { get; } = GetExeFileName();

        public static ProcessStartInfo ProcessStartInfo
        {
            get
            {
                var fileName = GetExeFileName();
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    UseShellExecute = false,
                    ////CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                return processStartInfo;
            }
        }

        internal static ProcessStartInfo CreateStartInfo(string args)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = GetExeFileName(),
                Arguments = args,
                UseShellExecute = false,
                ////CreateNoWindow = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            return processStartInfo;
        }

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

        private static string GetExeFileName()
        {
            //// ReSharper disable once PossibleNullReferenceException
            var fileName = Path.GetFileNameWithoutExtension(TestAssemblyFullFileName()).Replace("Ui.Tests", "Demo");
            //// ReSharper disable once AssignNullToNotNullAttribute
            var fullFileName = Path.Combine(TestAssemblyDirectory(), fileName + ".exe");
            return fullFileName;
        }
    }
}