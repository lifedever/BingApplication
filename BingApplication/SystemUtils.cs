using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace BingApplication
{
    class SystemUtils
    {
        private const string REG_KEY = "bing.wincn.net";

        /// <summary>
        /// 设置开机自启
        /// </summary>
        public static void setAutoStartup()
        {
            //获得文件的当前路径  
            string appName = Process.GetCurrentProcess().MainModule.FileName;
            //获取Run键
            RegistryKey reg = Registry.LocalMachine;
            RegistryKey run = reg.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            //在Run键中写入一个新的键值  
            run.SetValue(REG_KEY, "\"" + appName + "\"");
            reg.Close();
        }

        /// <summary>
        /// 取消开机自启
        /// </summary>
        public static void cancleAutoStartup()
        {
            RegistryKey reg = Registry.LocalMachine;
            RegistryKey run = reg.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            //在Run键中写入一个新的键值  
            if (run.GetValue(REG_KEY) != null)
            {
                run.DeleteValue(REG_KEY);
            }
            reg.Close();
        }
    }
}
