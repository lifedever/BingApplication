﻿using System;
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
using System.Security.Principal;
namespace BingApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = null;

        public MainWindow()
        {
            InitializeComponent();
            // 当程序没有以管理员权限启动时
            if (!IsAdmin())
            {
                SendMessage(this.Handle, 0x160C, 0, 0xFFFFFFFF); //　向按钮发送消息使之加上 UAC 盾牌标志
            }
        }

        /// <summary>
      /// 判断当前是否具有管理员权限
        /// </summary>
       private bool IsAdmin()
       {
          return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadConfig();
                loadImg();
                img.Visibility = System.Windows.Visibility.Visible;
                initializeTimer();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                //MessageBox.Show("连接失败，请检查您的网络！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        /// <summary>
        /// 定时器
        /// </summary>
        private void initializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

        private string getImageFullPath()
        {
            return ConfigUtils.getStorgePath().Value + DateTime.Today.ToLongDateString() + ".jpg";
        }

        /// <summary>
        /// 定时器执行的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_tick(object sender, EventArgs e)
        {
            if (!File.Exists(getImageFullPath()))
            {
                downloadImage();
            }
            setWallpaper();
           
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
                    WallpaperUtils.setWallpaper(getImageFullPath());
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

        private string getImageUrl()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("http://cn.bing.com/HPImageArchive.aspx?idx=0&n=1");
            XmlNodeList lis = doc.GetElementsByTagName("url");
            String str = lis[0].InnerText;
            string imgUrl = "http://cn.bing.com" + str;
            return imgUrl;
        }

        private void loadImg()
        {
            if (!File.Exists(getImageFullPath()))
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(getImageUrl(), UriKind.Absolute);
                myBitmapImage.DecodePixelWidth = 2048;
                myBitmapImage.EndInit();
                img.Source = myBitmapImage;
            }
            else
            {
                img.Source = new BitmapImage(new Uri(getImageFullPath(), UriKind.RelativeOrAbsolute));
            }
        }


        /// <summary>
        /// 下载图片保存到本地
        /// </summary>
        private void downloadImage()
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(new Uri(getImageUrl(), UriKind.Absolute), getImageFullPath());
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

    }
}
