using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubiLauncher_CSharp
{
    public struct GameInfo
    {
        /// <summary>
        /// 游戏jar完整路径
        /// </summary>
        public string jarPath;
        /// <summary>
        /// 游戏jar文件名
        /// </summary>
        public string jarName;
        /// <summary>
        /// 游戏版本名字
        /// </summary>
        public string versionName;
        /// <summary>
        /// 游戏version路径
        /// </summary>
        public string versionPath;

        public string libPath;
        public string navPath;
        public string UserName;
        public string CustomXmx;
        public string CustomXms;

        public string jsonName;
        public string jsonPath;
        public string json;

        public string gameDirectory;
        public string gameAssets;

        public MCJsonStruct.LaunchArguments la;

        /// <summary>
        /// GameInfo构造函数
        /// </summary>
        /// <param name="path">游戏version完整路径</param>
        public GameInfo(string path)
        {
            versionPath = path;
            string[] tmp = path.Split(@"\".ToCharArray());
            versionName = tmp.Last();
            jarName = versionName + ".jar";
            jarPath = versionPath + @"\" + jarName;
            jsonName = versionName + ".json";
            jsonPath = versionPath + @"\" + jsonName;
            json = "";
            libPath = @".minecraft\libraries";
            navPath = @".minecraft\natives";
            UserName = "UserName";
            CustomXms = "1024";
            CustomXmx = "1024";

            gameDirectory = ".minecraft";
            gameAssets = @".minecraft\assets";

            la = null;
            
        }

    }
}
