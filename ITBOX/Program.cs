using System;
using System.Diagnostics;
using System.IO;

namespace ITBOX
{
    class Program
    {
        private static void RunWindow()
        {
            using (MainWindow main = new MainWindow())
            {

            }
        }

        private static void RunNotDebugConsole()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Directory.GetCurrentDirectory() + "/POOG.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "0"
            };
            Process.Start(startInfo);
           
        }
        static void Main(string[] args)
        {
#if DEBUG
            RunWindow();
#else
            if (args.Length > 0 && args[0] == "0")
            {
                RunWindow();
                return;
            }
            RunNotDebugConsole();
#endif
        }
    }
}
