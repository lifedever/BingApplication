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
using System.Diagnostics;
namespace BingApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = null;

        private DispatcherTimer changeTimer = new DispatcherTimer();

        private NotifyIcon notifyIcon;

        private Thread updateThread;

        private Version versionObj = null;
        public Version VersionObj
        {
            get { return versionObj; }
            set { versionObj = value; }
        }

        public const string remote = "http://git.oschina.net/gefangshuai/BingApplication/raw/master/BingApplication/bin/setup/";
        private const string VERSION_FILE = "version.xml";
        public MainWindow()
        {

            InitializeComponent();
            this.Title = "每日Bing壁纸" + ConfigUtils.VERSION;

            InitNotify();

            updateCheck(true, null);


        }

        private void updateCheck(bool auto, Window win)
        {
            WebClient client = new WebClient();
            updateThread = new Thread(() =>
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                {
                    // 获取版本信息
                    string versionInfo = Encoding.UTF8.GetString(client.DownloadData(remote + VERSION_FILE));

                    // 将信息转换为对象 
                    versionObj = XmlHelper.XmlDeserailize(versionInfo, typeof(Version)) as Version;

                    if (!versionObj.AppVersion.Equals(ConfigUtils.VERSION))
                    {
                        // 需要更新
                        if (!auto)
                        {
                            win.Close();
                        }
                        UpdateInfoWindow updateWin = new UpdateInfoWindow();
                        updateWin.Show();
                        List<string> infos = versionObj.AppInfo;
                        updateWin.textUpdateInfo.Text = string.Format("发现{0}项可用更新！版本号：{1}", infos.Count.ToString(), versionObj.AppVersion);
                        string content = "";
                        foreach (string item in infos)
                        {
                            if (infos.IndexOf(item) == infos.Count-1)
                            {
                                content += item;
                            }
                            else
                            {
                                content += item + "\n";
                            }
                        }
                        updateWin.groupBoxInfo.Content = content;


                    }
                    else 
                    {
                        if (!auto)
                        {
                            win.Close();
                            System.Windows.MessageBox.Show("当前没有可用更新!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    
                });
            });
            updateThread.Start();
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
            openImageDir.Click += new EventHandler((o, e) =>
            {
                openDir();
            });

            //设置菜单项
            System.Windows.Forms.MenuItem setup = new System.Windows.Forms.MenuItem("选项");
            setup.Click += new EventHandler((o, e) =>
            {
                setupClick();
            });

            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler((o, e) =>
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
                setup,
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
                initializeChangeTimer();
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.ToString());
            }

        }

        private void initializeChangeTimer()
        {
            changeTimer.Stop();
            if (Boolean.Parse(ConfigUtils.getElement(ConfigUtils.AUTO_CHANGE_WALLPAPER).Value))
            {
                int interval = int.Parse(ConfigUtils.getElement(ConfigUtils.AUTO_CHANGE_WALLPAPER_INTERVAL).Value);
                changeTimer.Interval = new TimeSpan(0, interval, 0);
                changeTimer.Tick += new EventHandler(changeTimer_tick);
                changeTimer.Start();
            }
            else
            {
                changeTimer.Stop();
            }
        }

        private void changeTimer_tick(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(ConfigUtils.getStorgePath().Value);
            Random ran = new Random();

            int n = ran.Next(0, files.Length - 1);
            setWallpaper(files[n]);
        }

        /// <summary>
        /// 定时器
        /// </summary>
        private void initializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += new EventHandler(timer_tick);

            if (Boolean.Parse(ConfigUtils.getAutoSave().Value))
            {
                timer.Start();
            }

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
            if (autoSave)   //自动保存
            {
                if (!File.Exists(getImageFullPath()))
                {
                    downloadImage();
                }
                if (Boolean.Parse(ConfigUtils.getAutoWallPaper().Value) && File.Exists(getImageFullPath()))
                {
                    KeyValueConfigurationElement element = ConfigUtils.getElement(ConfigUtils.WALLPAPER_TIME);

                    if (element == null)
                    {
                        setWallpaper(getImageFullPath());
                    }
                    else
                    {
                        string shortDateString = element.Value;
                        DateTime changeTime = DateTime.Parse(shortDateString);
                        if (DateTime.Today.CompareTo(changeTime) > 0)
                        {
                            setWallpaper(getImageFullPath());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置桌面壁纸
        /// </summary>
        private void setWallpaper(string filePath)
        {
            WallpaperUtils.setWallpaper(filePath);
            notifyTip("恭喜！", "已更换新的桌面壁纸！", 2000);
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
            notifyTip("提示", "壁纸已下载并保存在" + getImageFullPath(), 2000);
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
            setupClick();
        }

        private void setupClick()
        {
            SetupWindow setupWin = new SetupWindow();
            setupWin.Owner = this;
            setupWin.ShowDialog();

            updateConfig(setupWin);

            if (!Boolean.Parse(ConfigUtils.getAutoSave().Value))
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }

            initializeChangeTimer();
        }

        /// <summary>
        /// 更新配置信息
        /// </summary>
        /// <param name="setupWin"></param>
        private void updateConfig(SetupWindow setupWin)
        {
            ConfigUtils.setStorgePath(setupWin.textPath.Text);
            ConfigUtils.setAutoWallPaper(setupWin.chkWall.IsChecked.ToString());
            //ConfigUtils.setAutoStartup(chkAutoStart.IsChecked.ToString());
            ConfigUtils.setAutoSave(setupWin.chkSave.IsChecked.ToString());
            ConfigUtils.setProp(setupWin.autoChange.IsChecked.ToString(), ConfigUtils.AUTO_CHANGE_WALLPAPER);
            ComboInterval interval = setupWin.combobox.SelectedValue as ComboInterval;
            ConfigUtils.setProp(interval.Interval.ToString(), ConfigUtils.AUTO_CHANGE_WALLPAPER_INTERVAL);
            ConfigUtils.setProp(setupWin.checkBoxSound.IsChecked.ToString(), ConfigUtils.CHANGE_SOUND);
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
                File.Copy(getImageFullPath(), filename);
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

        private void autoChangeMenu_Click(object sender, RoutedEventArgs e)
        {
        }

        private void checkUpdate_Click(object sender, RoutedEventArgs e)
        {
            CheckUpdateWindow updateWindow = new CheckUpdateWindow();
            updateWindow.Show();
            updateCheck(false, updateWindow);
        }


    }
}
