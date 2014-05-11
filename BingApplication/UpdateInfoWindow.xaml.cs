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

namespace BingApplication
{
    /// <summary>
    /// UpdateInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateInfoWindow : Window
    {
        public UpdateInfoWindow()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            DownloadWindow downloadWin = new DownloadWindow();
            downloadWin.Show();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
