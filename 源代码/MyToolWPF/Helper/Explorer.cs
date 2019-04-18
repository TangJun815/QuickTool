using SHDocVw;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTool.Helper
{
    //reference
    //https://stackoverflow.com/questions/48151571/close-open-explorer-windows-without-terminating-explorer-exe
    //https://stackoverflow.com/questions/13463639/is-there-a-way-to-close-a-particular-instance-of-explorer-with-c
    class Explorer
    {
        const string ExplorerProcessName = "explorer";
        /// <summary>
        /// through Shell Open path. not create new process.
        /// </summary>
        /// <param name="vDir">path</param>
        public static void Explore(string vDir)
        {
            var shell = new Shell32.Shell();
            shell.Explore(vDir);
        }
        /// <summary>
        /// Close all explorer windows.
        /// </summary>
        public static void CloseAllWindows()
        {
            do
            {
                var _shellWindows = new SHDocVw.ShellWindows();

                if (_shellWindows.Count > 0)
                {
                    foreach (InternetExplorer ie in _shellWindows)
                    {
                        //this parses the name of the process
                        var processName = System.IO.Path.GetFileNameWithoutExtension(ie.FullName).ToLower();
                        Debug.WriteLine($"{processName}:{ie.LocationName},{ie.LocationURL}");
                        //this could also be used for IE windows with processType of "iexplore"
                        //if (processName.Equals(ExplorerProcessName) && ie.LocationURL.Contains(@"C:/Windows"))
                        if (processName.Equals(ExplorerProcessName))
                        {
                            ie.Quit();
                        }
                    }
                }
                else
                {
                    break;
                }
            } while (true);
        }
        /// <summary>
        /// Open Path through New Explorer process.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="windowStyle"></param>
        public static void OpenNewExplorer(string arguments, ProcessWindowStyle windowStyle = ProcessWindowStyle.Maximized)
        {
            Process myProcess = new Process();
            var startInfo = new ProcessStartInfo(@"explorer.exe");
            startInfo.WindowStyle = windowStyle;

            myProcess.StartInfo = startInfo;

            startInfo.Arguments = arguments;
            myProcess.Start();
        }
    }
}
