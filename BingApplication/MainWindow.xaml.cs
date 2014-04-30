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
using System.Configuration;
using System.Windows.Threading;
namespace BingApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string imgUrl;
        private string imgName;
        private bool isSet = true;

        private DispatcherTimer timer = null;

        public static string path;
        public string dir;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadImg();
                img.Visibility = System.Windows.Visibility.Visible;
                loadConfig();
                initializeTimer();
            }
            catch (Exception ee)
            {
                MessageBox.Show("连接失败，请检查您的网络！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        /// <summary>
        /// 定时器
        /// </summary>
        private void initializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

        /// <summary>
        /// 定时器执行的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_tick(object sender, EventArgs e)
        {
            dir = ConfigUtils.getStorgePath().Value;
            path = @dir + DateTime.Today.ToLongDateString() + ".jpg";

            if (!File.Exists(path))
            {
                downloadImage();
                if (isSet)
                {
                    setWallpaper();
                }
            }
            else if(isSet)
            {
                setWallpaper();
            }
        }

        /// <summary>
        /// 设置桌面壁纸
        /// </summary>
        private void setWallpaper()
        {
            try
            {
                KeyValueConfigurationElement autoWallpaper = ConfigUtils.getAutoWallPaper();
                if (autoWallpaper != null && autoWallpaper.Value == "True")
                {
                    WallpaperUtils.setWallpaper(path);
                    message.Text = DateTime.Today.ToLongDateString() + " 的壁纸设置成功！";
                    //isSet = false;
                }
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.ToString());
            }
            
            
        }

        /**
         * 创建配置文件
         * */
        private void loadConfig()
        {
            ConfigUtils.initConfig();
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


        /// <summary>
        /// 下载图片保存到本地
        /// </summary>
        private void downloadImage()
        {
            WebClient wc = new WebClient();
            if (!Directory.Exists(dir))
            {
                new DirectoryInfo(@dir).Create();
            }
            wc.DownloadFile(new Uri(imgUrl, UriKind.Absolute), path);
            //MessageBox.Show("下载成功,文件地址为：" + path, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            isSet = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        /**
         * 退出应用程序
         * */
        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void menuItemSetup_Click(object sender, RoutedEventArgs e)
        {
            SetupWindow setupWin = new SetupWindow();
            setupWin.ShowDialog();
        }

        private void menuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        private void hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(img.Source.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
