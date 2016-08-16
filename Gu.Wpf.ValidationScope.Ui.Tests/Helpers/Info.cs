namespace Gu.Wpf.ValidationScope.Ui.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public static class Info
    {
        public static ProcessStartInfo ProcessStartInfo
        {
            get
            {
                var fileName = GetExeFileName();
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    UseShellExecute = false,
                    //CreateNoWindow = true,
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
                //CreateNoWindow = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            return processStartInfo;
        }

        private static string GetExeFileName()
        {
            var uri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var directoryName = Path.GetDirectoryName(uri.LocalPath);
            var fileName = Path.GetFileNameWithoutExtension(uri.LocalPath).Replace("Ui.Tests", "Demo");
            var fullFileName = Path.Combine(directoryName, fileName + ".exe");
            return fullFileName;
        }
    }
}