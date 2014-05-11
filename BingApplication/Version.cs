using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BingApplication
{
    /// <summary>
    /// 版本信息
    /// </summary>
    /// 
    [Serializable]
    [XmlRoot("app")]
    public class Version
    {
        private List<string> files; //  文件清单

        [XmlElement("files", typeof(List<string>))]
        public List<string> Files
        {
            get { return files; }
            set { files = value; }
        }
        private string appVersion;  //  程序版本号

        [XmlElement("version")]
        public string AppVersion
        {
            get { return appVersion; }
            set { appVersion = value; }
        }
        private List<String> appInfo; //  程序更新信息

        [XmlElement("info", typeof(List<string>))]
        public List<String> AppInfo
        {
            get { return appInfo; }
            set { appInfo = value; }
        }



        private int size;   //  更新大小

        [XmlElement("size")]
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

    }
}
