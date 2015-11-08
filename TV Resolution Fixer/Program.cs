using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace TV_Resolution_Fixer
{
    class Program
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
           SetLastError = true,
           CharSet = CharSet.Unicode, ExactSpelling = true,
           CallingConvention = CallingConvention.StdCall)]
        
        private static extern int WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFilename);

        const string iniPath = "TV_CD_DVD.ini";
        const string windowsClientSection = "WinDrv.WindowsClient";
        const string kResX = "FullscreenViewportX";
        const string kResY = "FullscreenViewportY";

        static void Main(string[] args)
        {
            bool shouldWait = true;

            foreach (string arg in args)
            {
                switch (arg.Substring(0, 2).ToLower())
                {
                    case "/q":
                        shouldWait = false;
                        break;

                    default:
                        break;
                }
            }

            if (File.Exists(iniPath))
            {
                string fullIniPath = Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath).ToString(), iniPath);

                Rectangle resolution = Screen.PrimaryScreen.Bounds;

                string resX = resolution.Width.ToString();
                string resY = resolution.Height.ToString();
                
                int r = WritePrivateProfileString(windowsClientSection, kResX, resX, fullIniPath);
                r &=    WritePrivateProfileString(windowsClientSection, kResY, resY, fullIniPath);

                if (r == 0)
                {
                    Console.WriteLine("Something went wrong, not sure what happened. Change the " + kResX + " and " + kResY + " values under the " + windowsClientSection + " section in " + iniPath + " to the main resolutions yourself.");
                }
                else
                {
                    Console.WriteLine("Resolution set to " + resX + "x" + resY);
                }
            }
            else
            {
                Console.WriteLine(iniPath + " not found. Please move this program in to the Bin folder of the Tribes Installation");
            }

            if (shouldWait)
            {
                System.Threading.Thread.Sleep(15 * 1000);
            }
        }
    }
}
