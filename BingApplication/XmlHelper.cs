using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BingApplication
{
    class XmlHelper
    {
        /// <summary>
        /// 将xml反序列化为类
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object XmlDeserailize(String xml, Type type)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            //实例化一个type类型的序列化对象
            XmlSerializer serializer = new XmlSerializer(type);
            //使用stringreader读取xml内容
            StringReader reader = new StringReader(xml);
            //使用stringreader来填充XmltextReader对象
            XmlTextReader xmlReader = new XmlTextReader(reader);
            //将xmlreader中的字符流序列化为对象
            Object obj = serializer.Deserialize(xmlReader);
            //关闭xmlreader
            xmlReader.Close();
            //关闭reader
            reader.Close();
            return obj;
        }

        /// <summary>
        /// 将类序列化为xml文本
        /// </summary>
        /// <param name="type">要被序列化的对象的type</param>
        /// <param name="isDflg">序列化时候是否进行缩进格式化</param>
        /// <returns></returns>
        public static String Xmlserailize(Object obj, bool isIndent)
        {
            if (obj == null) return "";
            //xml命名空间
            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            //此处不需要,所以添加为空
            xmlns.Add(string.Empty, string.Empty);
            //xml序列化器
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            //声明一块内存空间
            MemoryStream ms = new MemoryStream();
            //定义编码方式
            Encoding utf8 = new UTF8Encoding(false);
            //实例化一个XmlTextWriter对象,并且XmlTextWriter对象所关联的内存为ms,编码为Utf8
            //向writer中写入数据相当于向writer所关联的ms中写入;
            XmlTextWriter writer = new XmlTextWriter(ms, utf8);
            string xml = string.Empty;
            writer.Formatting = isIndent ? Formatting.Indented : Formatting.None;
            //将obj对象写入writer对象,使用的命名空间为xmlns
            serializer.Serialize(writer, obj, xmlns);
            //从ms流中读取字符串(已经通过序列化器格式化后的流)
            xml = utf8.GetString(ms.ToArray());
            //刷新缓冲区
            writer.Flush();
            //关闭writer对象
            writer.Close();
            ms.Close();
            return xml;
        }
    }
}
