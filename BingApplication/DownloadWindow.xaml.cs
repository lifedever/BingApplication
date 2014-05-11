using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BingApplication
{
    /// <summary>
    /// DownloadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadWindow : Window
    {
        private Thread downloadThread;
        private Version obj;

        public DownloadWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            doAction();
            MainWindow mainWin = Application.Current.MainWindow as MainWindow;
            obj = mainWin.VersionObj;
        }

        private void doAction()
        {
            downloadThread = new Thread(download);
            downloadThread.Start();
        }

        private void download()
        {
            WebClient wc = new WebClient();
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                    {
                        foreach (string item in obj.Files)
                        {
                            textBlockFileInfo.Text = string.Format("需要下载文件总数：{0}, 正在下载第{1}个文件", obj.Files.Count, obj.Files.IndexOf(item) + 1);
                            wc.DownloadFileAsync(new Uri(MainWindow.remote + item), item);
                            wc.DownloadProgressChanged += client_DownloadProgressChanged;
                            wc.DownloadFileCompleted += client_DownloadFileCompleted;
                        }
                    });
        }

        private void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            textBlockFileInfo.Text = "下载完成！";
            textBlockSizeInfo.Visibility = Visibility.Hidden;
            buttonUpdate.Visibility = Visibility.Visible;
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            textBlockSizeInfo.Visibility = Visibility.Visible;
            long iTotalSize = e.TotalBytesToReceive;
            long iSize = e.BytesReceived;
            textBlockSizeInfo.Text = string.Format("文件大小总共 {1} KB, 当前已接收 {0} KB", (iSize / 1024), (iTotalSize / 1024));

            probar.Value = Convert.ToDouble(iSize) / Convert.ToDouble(iTotalSize) * 100;
        }

        private void updateProgressBar()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                    {

                    });
            }
        }


        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "setup.exe";
            process.Start();
            System.Environment.Exit(0);
        }

    }
}
