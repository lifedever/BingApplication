using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;

namespace BingApplication
{
    /// <summary>
    /// config配置文件工具类
    /// </summary>
    class ConfigUtils
    {
        public const string STORGE_PATH = "storgePath";
        public const string AUTO_WALLPAPER = "autoWallPaper";
        public const string AUTO_STARTUP = "autoStartup";
        public const string AUTO_SAVE = "autoSave";

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

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

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

       

        /// <summary>
        /// 初始化系统参数
        /// </summary>
        internal static void initConfig()
        {
            initStorgePath();
            initAutoWallPaper();
            initAutoStartup();
            initAutoSave();
        }

        private static void initStorgePath()
        {
            if (getStorgePath() == null)
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                dir = dir.EndsWith("/") || dir.EndsWith("\\") ? dir + "bing/" : dir + "/bing/";
                setStorgePath(dir);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }            
        }
        private static void initAutoWallPaper()
        {
            if (getAutoWallPaper() == null)
            {
                setAutoWallPaper("False");
            }
        }

        private static void initAutoStartup()
        {
            if (getAutoStartup() == null)
            {
                setAutoStartup("False");
            }
        }

        private static void initAutoSave()
        {
            if (getAutoSave() == null)
            {
                setAutoSave("False");
            }
        }

        public static KeyValueConfigurationElement getAutoWallPaper()
        {
            return getConfig().AppSettings.Settings[AUTO_WALLPAPER];
        }

        internal static void setAutoWallPaper(string p)
        {
            setProp(p, AUTO_WALLPAPER);
        }

        public static KeyValueConfigurationElement getAutoSave()
        {
            return getConfig().AppSettings.Settings[AUTO_SAVE];
        }

        internal static void setAutoSave(string p)
        {
            setProp(p, AUTO_SAVE);
        }

        public static KeyValueConfigurationElement getAutoStartup()
        {
            return getConfig().AppSettings.Settings[AUTO_STARTUP];
        }
        public static void setAutoStartup(string p)
        {
            setProp(p, AUTO_STARTUP);

            if (Boolean.Parse(p))
            {
                SystemUtils.setAutoStartup();
            }
            else
            {
                SystemUtils.cancleAutoStartup();
            }
        }

        private static void setProp(string p, string key)
        {
            KeyValueConfigurationElement element = ConfigUtils.getElement(key);
            Configuration config = getConfig();
            if (element == null || string.IsNullOrEmpty(element.Value))
            {
                config.AppSettings.Settings.Add(key, p);
            }
            else
            {
                config.AppSettings.Settings[key].Value = p;
            }
            config.Save();
        }

        private static KeyValueConfigurationElement getElement(string key)
        {
            return getConfig().AppSettings.Settings[key];
        }
    }
}
