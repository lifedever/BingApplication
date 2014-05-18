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
    public partial class ConfigUtils
    {
        public const string VERSION = "v0.2.5";

        public const string STORGE_PATH = "storgePath";     //存储路径
        public const string AUTO_WALLPAPER = "autoWallPaper";   //自动设置当前壁纸
        public const string AUTO_STARTUP = "autoStartup";   //自动启动
        public const string AUTO_SAVE = "autoSave"; //自动保存
        public const string WARNING_SAVE = "warningSave";   //是否提示
        public const string WALLPAPER_TIME = "wallpaperTime";   //下载时间
        public const string AUTO_CHANGE_WALLPAPER = "autoChangeWallPaper";  //自动更换壁纸
        public const string AUTO_CHANGE_WALLPAPER_INTERVAL = "autoChangeWallPaperInterval"; //自动更换壁纸间隔
        public const string CHANGE_SOUND = "changeSound";   //更换音效


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
            //initAutoStartup();
            initAutoSave();
            initWarningSave();
            initAutoChangeWallPaper();
            initChangeSound();
        }

        private static void initChangeSound()
        {
            KeyValueConfigurationElement element = getElement(CHANGE_SOUND);
            if (element == null)
            {
                setProp("True", CHANGE_SOUND);
            }
        }

        private static void initAutoChangeWallPaper()
        {
            KeyValueConfigurationElement element = getElement(AUTO_CHANGE_WALLPAPER);
            if (element == null)
            {
                setProp("False", AUTO_CHANGE_WALLPAPER);
            }
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

        private static void initWarningSave()
        {
            if (getWarningSave() == null)
            {
                setWarningSave("Yes");
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

        public static KeyValueConfigurationElement getWarningSave()
        {
            return getConfig().AppSettings.Settings[WARNING_SAVE];
        }

        internal static void setWarningSave(string p)
        {
            setProp(p, WARNING_SAVE);
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

        public static void setProp(string p, string key)
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

        public static KeyValueConfigurationElement getElement(string key)
        {
            return getConfig().AppSettings.Settings[key];
        }
    }
}
