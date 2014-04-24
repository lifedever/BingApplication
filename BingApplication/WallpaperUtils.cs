using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace BingApplication
{
    
    class WallpaperUtils
    {
        public string strPath = "";
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
            int uAction,
            int uParam,
            string lpvParam,
            int fuWinIni
            );
        public static void setWallpaper(string Path)
        {
            if (File.Exists(Path))
            {
                SystemParametersInfo(20, 0, Path, 0x2); // 0x1 | 0x2 
            }
        }
    }
}
