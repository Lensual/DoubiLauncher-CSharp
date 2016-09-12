using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;

namespace DoubiLauncher_CSharp
{

    /// <summary>
    /// 用于序列化和反序列化JSON
    /// </summary>
    public static class JSONSerizer
    {
        #region 序列化JSON
        public static string Serizer_Write(object obj)
        {
            //声明定义 内存流 和 Serializer
            MemoryStream MemStream = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
            //将序列化后的json写到内存流
            ser.WriteObject(MemStream, obj);
            //读内存流并返回string
            MemStream.Position = 0;
            StreamReader MemStreamReader = new StreamReader(MemStream);
            return MemStreamReader.ReadToEnd();
        }
    #endregion
        #region 反序列化JSON
        public static object Serizer_Read(string json,Type jsonType)
        {
            //声明定义 内存流 和 Serializer
            MemoryStream MemStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(jsonType);
            //反序列化内存流 并返回
            return ser.ReadObject(MemStream);
        }
#endregion
    }
}
