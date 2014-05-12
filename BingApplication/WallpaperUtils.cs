using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace BingApplication
{
    
    class WallpaperUtils
    {
        private static SoundPlayer player = new SoundPlayer();

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
                KeyValueConfigurationElement element = ConfigUtils.getElement(ConfigUtils.CHANGE_SOUND);
                if (element != null && Boolean.Parse(element.Value))
                {
                    SoundPlayer player = new SoundPlayer();
                    player.Stream = Properties.Resources.ding;
                    player.Play();
                }
                SystemParametersInfo(20, 0, Path, 0x2); // 0x1 | 0x2 
                ConfigUtils.setProp(DateTime.Today.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo), ConfigUtils.WALLPAPER_TIME);
            }
        }
    }
}
