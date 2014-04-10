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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Net;
using System.IO;
using System.Threading;

namespace BingApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string imgUrl;
        private string imgName;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadImg();
            }
            catch (Exception ee)
            {
                
                MessageBox.Show("连接失败，请检查您的网络！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
            img.Visibility = System.Windows.Visibility.Visible;
        }

        private void getImg_Click(object sender, RoutedEventArgs e)
        {
            

        }
        private void loadImg()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("http://cn.bing.com/HPImageArchive.aspx?idx=0&n=1");
            XmlNodeList lis = doc.GetElementsByTagName("url");
            String str = lis[0].InnerText;
            string imgUrl = "http://cn.bing.com" + str;
           

            BitmapImage myBitmapImage = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(imgUrl, UriKind.Absolute);
            myBitmapImage.DecodePixelWidth = 2048;
            myBitmapImage.EndInit();
            img.Source = myBitmapImage;

            //如果是HTTP下载文件
            this.imgUrl = imgUrl;
            this.imgName = str.Substring(str.LastIndexOf("/"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            string dir = AppDomain.CurrentDomain.BaseDirectory + "bing";
            if (!Directory.Exists(dir))
            {
                new DirectoryInfo(dir).Create();
            }
            string path = dir + imgName;
            wc.DownloadFile(new Uri(imgUrl, UriKind.Absolute), path);
            MessageBox.Show("下载成功,文件地址为："+path, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
