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
        public SetupWindow()
        {
            InitializeComponent();
        }

        private void selectPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderDialog.SelectedPath;
                textPath.Text = path;

                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfa.AppSettings.Settings[STORGE_PATH].Value = path;
                
                cfa.Save();
            }
        }
    }
}
