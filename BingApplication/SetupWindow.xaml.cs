using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Configuration;

namespace BingApplication
{
    /// <summary>
    /// SetupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetupWindow : Window
    {
        public const string STORGE_PATH = "storgePath";
        public const string AUTO_WALLPAPER = "autoWallPaper";
        public SetupWindow()
        {
            InitializeComponent();
        }

        private void selectPath_Click(object sender, RoutedEventArgs e)
        {
            selectPathOpera();
        }

        private void textPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectPathOpera();
        }

        private void selectPathOpera()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderDialog.SelectedPath;
                textPath.Text = path;

                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfa.AppSettings.Settings[STORGE_PATH].Value = path + "\\";
                cfa.Save();
            }
        }

        private void chkWall_Click(object sender, RoutedEventArgs e)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (cfg.AppSettings.Settings[AUTO_WALLPAPER] == null || string.IsNullOrEmpty(cfg.AppSettings.Settings[AUTO_WALLPAPER].Value))
            {
                cfg.AppSettings.Settings.Add(AUTO_WALLPAPER, chkWall.IsChecked.ToString());
                
            }
            else
            {
                cfg.AppSettings.Settings[AUTO_WALLPAPER].Value = chkWall.IsChecked.ToString();
            }
            cfg.Save();
        }
    }
}
