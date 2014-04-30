using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BingApplication
{
    /// <summary>
    /// config配置文件工具类
    /// </summary>
    class ConfigUtils
    {
        public const string STORGE_PATH = "storgePath";
        public const string AUTO_WALLPAPER = "autoWallPaper";

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns>配置信息</returns>
        private static Configuration getConfig()
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return config;
        }


        public static KeyValueConfigurationElement getStorgePath()
        {
            return getConfig().AppSettings.Settings[STORGE_PATH];
        }

        /// <summary>
        /// 保存选择的存储路径
        /// </summary>
        /// <param name="path"></param>
        public static void setStorgePath(string path)
        {
            path = path.EndsWith("/") || path.EndsWith("\\") ? path : path + "/";

            KeyValueConfigurationElement element = ConfigUtils.getStorgePath();
            Configuration config = getConfig();
            if (element == null || string.IsNullOrEmpty(element.Value))
            {
                config.AppSettings.Settings.Add(STORGE_PATH, path);
            }
            else
            {
                config.AppSettings.Settings[STORGE_PATH].Value = path;
            }
            config.Save();
        }

        public static KeyValueConfigurationElement getAutoWallPaper()
        {
            return getConfig().AppSettings.Settings[AUTO_WALLPAPER];
        }

        /// <summary>
        /// 初始化系统参数
        /// </summary>
        internal static void initConfig()
        {
            initStorgePath();
            initAutoWallPaper();
        }


        private static void initAutoWallPaper()
        {
            setAutoWallPaper("False");
        }

        /// <summary>
        /// 初始化存储参数
        /// </summary>
        /// <param name="config"></param>
        private static void initStorgePath()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            dir = dir.EndsWith("/") || dir.EndsWith("\\") ? dir + "bing/" : dir + "/bing/";
            setStorgePath(dir);
        }


        internal static void setAutoWallPaper(string p)
        {
            KeyValueConfigurationElement element = ConfigUtils.getAutoWallPaper();
            Configuration config = getConfig();
            if (element == null || string.IsNullOrEmpty(element.Value))
            {
                config.AppSettings.Settings.Add(AUTO_WALLPAPER, p);
            }
            else
            {
                config.AppSettings.Settings[AUTO_WALLPAPER].Value = p;
            }
            config.Save();
        }
    }
}
