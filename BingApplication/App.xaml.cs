using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace BingApplication
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main(string[] args)
        {
            SingleInstanceApplicationWrapper wrapper = new SingleInstanceApplicationWrapper();
            wrapper.Run(args);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow w = new MainWindow();
            w.Show();
        }
        public void activate()
        {
            MainWindow.Visibility = Visibility.Visible;
            MainWindow.Activate();
        }
        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {

        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
