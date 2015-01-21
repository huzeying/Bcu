using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace WebSite.DataProvider
{
    public static class JsonSerializer
    {
        /// <summary>
        /// 将对象序列化Json
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Serialize(this object o)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式 
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            //JsonConvert.SerializeObject(dt, new Newtonsoft.Json.Converters.JsonDa());
            return JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.None, timeConverter);
        }

        public static string SerializeHTML(this object o)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式 
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            //JsonConvert.SerializeObject(dt, new Newtonsoft.Json.Converters.JsonDa());
            return JsonConvert.SerializeObject(o, timeConverter);
        }

        /// <summary>
        /// 将Json反序列化为指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 将Json反序列化为指定类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeSerialize(this string str, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(str, type);
            }
            catch
            {
                if (!str.StartsWith("\""))
                {
                    str = "\"" + str;
                }
                if (!str.EndsWith("\""))
                {
                    str = str + "\"";
                }
                return JsonConvert.DeserializeObject(str, type);
            }
        }

        /// <summary>
        /// 将可序列化的对象转换为XML字符串
        /// </summary>
        /// <typeparam name="T">指定对象的类型</typeparam>
        /// <param name="obj">可序列化的对象</param>
        /// <returns>XML字符串</returns>
        public static string XmlSerialize<T>(T obj)
        {
            string xmlString = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xmlString;
        }

        /// <summary>  
        /// XML String 反序列化成对象  
        /// </summary>  
        public static T XmlDeserialize<T>(string xmlString)
        {
            T t = default(T);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                {
                    Object obj = xmlSerializer.Deserialize(xmlReader);
                    t = (T)obj;
                }
            }
            return t;
        }

        /// <summary>
        /// 深度Clone一个可序列化的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}