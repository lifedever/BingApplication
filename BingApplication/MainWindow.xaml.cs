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
using System.Windows.Forms;
namespace BingApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = null;
        private NotifyIcon notifyIcon;

        public MainWindow()
        {
            
            InitializeComponent();
            this.Title = "每日Bing壁纸" + ConfigUtils.VERSION;

            InitNotify();
        }

        private void InitNotify()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Text = "每日Bing壁纸获取程序";
            this.notifyIcon.Icon = new System.Drawing.Icon("app.ico");
            this.notifyIcon.Visible = true;

            //打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("显示窗口");
            open.Click += new EventHandler((o, e) =>
            {
                this.Show();
            });

            //一键壁纸
            System.Windows.Forms.MenuItem oneKeySetup = new System.Windows.Forms.MenuItem("一键壁纸");
            oneKeySetup.Click += new EventHandler((o, e) => 
            {
                oneKeySetWallpaper();
            });
            //打开目录
            System.Windows.Forms.MenuItem openImageDir = new System.Windows.Forms.MenuItem("打开目录");
            openImageDir.Click += new EventHandler((o,e) => 
            {
                openDir();
            });
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler((o , e) =>
            {
                appClose();
            });

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] 
            { 
                open,
                new System.Windows.Forms.MenuItem("-"), 
                oneKeySetup,
                openImageDir,
                new System.Windows.Forms.MenuItem("-"), 
                exit 
            };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
            
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == MouseButtons.Left) 
                {
                    this.Show();
                };
            });


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
                System.Windows.MessageBox.Show(ee.ToString());
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
            bool autoSave = Boolean.Parse(ConfigUtils.getAutoSave().Value);
            if (autoSave)
            {
                if (!File.Exists(getImageFullPath()))
                {
                    downloadImage();
                    setWallpaper();
                }
                else
                {
                    KeyValueConfigurationElement element = ConfigUtils.getElement(ConfigUtils.WALLPAPER_TIME);

                    if (element == null)
                    {
                        setWallpaper();
                    }
                    else
                    {
                        string shortDateString = element.Value;
                        DateTime changeTime = DateTime.Parse(shortDateString);
                        if (DateTime.Today.CompareTo(changeTime) > 0)
                        {
                            setWallpaper();
                        }
                    }
                }
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
                    WallpaperUtils.setWallpaper(getImageFullPath());
                    notifyTip("恭喜！", "已更换新的桌面壁纸！", 2000);
                    
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.ToString());
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
            notifyTip("提示", "壁纸已下载并保存在"+getImageFullPath(), 2000);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        /**
         * 退出应用程序
         * */
        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            appClose();
        }

        private void appClose()
        {

            this.notifyIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
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

        private void menuItemOpenImage_Click(object sender, RoutedEventArgs e)
        {
            openDir();
        }

        private void openDir()
        {
            System.Diagnostics.Process.Start(@"explorer.exe", System.IO.Path.GetFullPath(ConfigUtils.getStorgePath().Value));

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            notifyTip("提示", "每日bing壁纸获取程序 已最小化到系统托盘", 2000);
            
        }

        private void notifyTip(string title, string text, int timeout)
        {
            this.notifyIcon.BalloonTipTitle = title;
            this.notifyIcon.BalloonTipText = text;
            this.notifyIcon.ShowBalloonTip(timeout);
        }

        private void menuItemSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = "图片"; // Default file name
            //dlg.DefaultExt = ".jpg"; // Default file extension
            dlg.Filter = "图片文件|*.jpg"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                string filename = dlg.FileName;
                File.Copy(getImageFullPath(),filename);
            }
        }

        private void oneKeySetup_Click(object sender, RoutedEventArgs e)
        {
            oneKeySetWallpaper();    
        }

        private void oneKeySetWallpaper()
        {
            if (!File.Exists(getImageFullPath()))
            {
                downloadImage();
            }
            WallpaperUtils.setWallpaper(getImageFullPath());
            notifyTip("恭喜！", "已更换新的桌面壁纸！", 2000);
        }

    }
}
