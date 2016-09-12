using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DoubiLauncher_CSharp
{
    public static class Network
    {
        #region 简易cURL封装
        /// <summary>
        /// 简易cURL封装
        /// </summary>
        /// <param name="URL">URL</param>
        /// <param name="Method">请求方法</param>
        /// <param name="Data">POST数据</param>
        /// <returns></returns>
        public static string cURL(string URL,string Method = "GET",string Data = "")
        {
            //创建HttpWebRequse对象 并进行相关设置
            HttpWebRequest httpReq = (HttpWebRequest)HttpWebRequest.Create(URL);
            httpReq.Method = Method;
            #region 设置POST数据
            if (Method=="POST")
            {
                //以UTF8编码
                byte[] btData = Encoding.UTF8.GetBytes(Data);
                //header设置
                httpReq.ContentType = "application/x-www-form-urlencoded";
                httpReq.ContentLength = btData.Length;
                //写入流
                httpReq.GetRequestStream().Write(btData, 0, btData.Length);
            }
            #endregion
            //取得数据 并返回
            HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse();
            StreamReader reader = new StreamReader(httpRes.GetResponseStream());
            return reader.ReadToEnd();
        }
#endregion

    }
}
