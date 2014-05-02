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
            chkSave.IsChecked = Boolean.Parse(ConfigUtils.getAutoSave().Value);
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
            ConfigUtils.setAutoSave(chkSave.IsChecked.ToString());
        }

        private void chkWall_Click(object sender, RoutedEventArgs e)
        {
            if (chkWall.IsChecked == true)
            {
                chkSave.IsChecked = chkWall.IsChecked;
            }
            if (chkWall.IsChecked == true && ConfigUtils.getElement(ConfigUtils.WARNING_SAVE).Value.Equals("Yes"))
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("您选择了自动设置桌面壁纸选项，程序需要将图片下载到本地才能保证运行正常！\n是否需要继续看到该提醒？", "这是一条提示信息", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                ConfigUtils.setProp(result.ToString(),ConfigUtils.WARNING_SAVE);
            }
        }
    }
}
