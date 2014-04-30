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
        
        public SetupWindow()
        {
            InitializeComponent();
            IniConfig();
        }

        /// <summary>
        /// 初始化配置参数
        /// </summary>
        private void IniConfig()
        {
            textPath.Text = ConfigUtils.getStorgePath().Value;
            chkWall.IsChecked = Boolean.Parse(ConfigUtils.getAutoWallPaper().Value);
            chkAutoStart.IsChecked = Boolean.Parse(ConfigUtils.getAutoStartup().Value);
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
                //ConfigUtils.setStorgePath(path);                
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ConfigUtils.setStorgePath(textPath.Text);
            ConfigUtils.setAutoWallPaper(chkWall.IsChecked.ToString());
            ConfigUtils.setAutoStartup(chkAutoStart.IsChecked.ToString());
        }
    }
}
