using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InputKeeperForms.Scripts
{
    public static class AppFocusClass
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

        // Enum that represent the possible states for the app's window
        private enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDeault = 10, ForceMinimized = 11
        }

        /// <summary>
        /// Method that brings the selected game to the front view
        /// </summary>
        /// <param name="processName">Route to the game in its directory</param>
        public static void BringMainWindowToFront(string processName)
        {
            Debug.WriteLine("Sending app");
            var strings = processName.Split('\\');
            Process bProcess = null;

            // Get the process if it is active -> it only works if the app is in the background
            for (int i = 0; i < strings.Length; i++)
            {
                Debug.WriteLine("Word: " + strings[i]);
                if (strings[i].Contains(".exe"))
                {
                    bProcess = Process.GetProcessesByName(strings[i]).FirstOrDefault();
                    if(bProcess == null)
                        bProcess = Process.GetProcessesByName(strings[i].Replace(".exe", "")).FirstOrDefault();
                }
            }


            // Check if the process is running
            if (bProcess != null)
            {
                Debug.WriteLine("Process not null");
                // Check if the window is hidden / minimized
                if (bProcess.MainWindowHandle == IntPtr.Zero)
                {
                    // The window is hidden so try to restore it before setting focus
                    ShowWindow(bProcess.Handle, ShowWindowEnum.Restore);
                }

                // Set user the focus to the window
                SetForegroundWindow(bProcess.MainWindowHandle);
            }
            else
            {
                Debug.WriteLine("Process null");

                // the process is not running, so start it -> if the game is minimized, the app reopens it
                bProcess = new Process
                {
                    StartInfo =
                        {
                            UseShellExecute = true,
                            FileName = processName,
                            Arguments = "",
                            CreateNoWindow = false
                        }
                };

                bProcess.Start();
            }
        }
    }
}
