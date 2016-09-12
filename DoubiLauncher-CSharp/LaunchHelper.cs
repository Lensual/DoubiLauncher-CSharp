using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;

namespace DoubiLauncher_CSharp
{

    /// <summary>
    /// 启动助手模块
    /// </summary>
    public static class LaunchHelper
    {
        #region 搜索jar
        /// <summary>
        /// 搜索jar包
        /// </summary>
        /// <param name="path">搜索路径</param>
        /// <returns>返回分号分隔的完整路径</returns>
        public static string FindJAR(string path)
        {
            string[] files;
            files = System.IO.Directory.GetFiles(path, "*.jar", System.IO.SearchOption.AllDirectories);
            string r = "";
            foreach (string f in files)
            {
                r += f + ";";
            }
            return r.Remove(r.Length-1); //去掉最后一个分号并返回
        }
        #endregion
        #region 生成CP参数
        /// <summary>
        /// 生成JVM启动的参数CP
        /// </summary>
        /// <param name="libPath">libraries文件夹路径</param>
        /// <param name="jarPath">游戏jar路径</param>
        /// <param name="la">LaunchArguments JSON的实例</param>
        /// <returns>返回CP 路径由分号分隔</returns>
        public static string MakeCP(string libPath, string jarPath, MCJsonStruct.LaunchArguments la)
        {
            string cp = "";
            foreach (MCJsonStruct.libraries lib in la.libraries)
            {
                try
                {
                    if (lib.natives == null)
                    {
                        #region 根据json生成路径
                        string[] front; //路径前面部分
                        string[] back; //路径后面部分
                        string fileName = ""; //文件名
                        string path = libPath + @"\"; //完整路径
                        back = lib.name.Split(":".ToCharArray());
                        front = back[0].Split(".".ToCharArray());
                        //链接前部字符串
                        foreach (string str in front)
                        {
                            path += str + @"\";
                        }
                        //链接后部字符串 和 文件名字符串
                        for (int i = 1; i < back.Length; i++)
                        {
                            path += back[i] + @"\";
                            fileName += back[i] + "-";
                        }
                        //链接成完整路径
                        path += fileName.TrimEnd("-".ToCharArray()) + ".jar";
                        #endregion
                        #region 验证文件是否存在 添加到cp
                        if (File.Exists(path))
                        {
                            cp += path + ";";
                        }
                    }
                    #endregion
                }
                catch (Exception)
                {

                    //throw;
                }

            }
            return cp + jarPath;

        }

        #endregion

        #region 分析JSON
        public static GameInfo AnalyseJSON(GameInfo gi)
        {
            //读js并反序列化
            string js = File.ReadAllText(gi.jsonPath);
            MCJsonStruct.LaunchArguments la = (MCJsonStruct.LaunchArguments)JSONSerizer.Serizer_Read(js, typeof(MCJsonStruct.LaunchArguments)); //实例化的json数据
            if (la.inheritsFrom != null)
            {
                //递归分析inheritsFrom
                GameInfo giSwap = gi;
                MCJsonStruct.libraries[] lib = la.libraries;
                giSwap.jsonPath = 
                    giSwap.versionPath.Remove(giSwap.versionPath.Length - giSwap.versionName.Length, giSwap.versionName.Length) + 
                    la.inheritsFrom + @"\" + la.inheritsFrom + ".json";
                giSwap = AnalyseJSON(giSwap);
                la.libraries = la.libraries.Concat(giSwap.la.libraries).ToArray();
                gi.jarPath = giSwap.jsonPath.Replace(".json", ".jar");
                gi.jarName = giSwap.jarPath.Split(@"\".ToCharArray()).Last();
            }
            gi.la = la;
            return gi;
        }
        #endregion

        #region 启动游戏

        /// <summary>
        /// 启动游戏
        /// </summary>
        ///// <param name="jarPath">游戏jar包路径</param>
        ///// <param name="libPath">libraries包搜索路径</param>
        ///// <param name="playerName">玩家名字</param>
        ///// <param name="Xmx">JVM参数 设置JVM最大可用内存, 不含单位(m)</param>
        ///// <param name="Xms">JVM参数 设置JVM初始内存, 不含单位(m)</param>
        /// <param name="gi">GameInfo 游戏信息对象</param>
        /// <returns>返回游戏Process对象</returns>
        public static Process Launch(GameInfo gi)
        {
            //json数据分析
            gi = AnalyseJSON(gi);

            //生成cp参数
            string cp = MakeCP(gi.libPath, gi.jarPath, gi.la); //cp参数 libraies和游戏包路径 JVM启动参数之一
            #region 给启动参数赋值
            gi.la.minecraftArguments = gi.la.minecraftArguments.Replace("${auth_player_name}", gi.UserName); //玩家名称;
            gi.la.minecraftArguments = gi.la.minecraftArguments.Replace("${version_name}", gi.versionName); //版本名字
            gi.la.minecraftArguments = gi.la.minecraftArguments.Replace("${game_directory}", gi.gameDirectory); //.minecraft文件夹路径
            gi.la.minecraftArguments = gi.la.minecraftArguments.Replace("${game_assets}",gi.gameAssets); //assets路径
            gi.la.minecraftArguments = gi.la.minecraftArguments.Replace("${assets_root}", gi.gameAssets); //assets路径
            gi.la.minecraftArguments = gi.la.minecraftArguments.Replace("${assets_index_name}", gi.la.assets); //assets index
            gi.la.minecraftArguments = gi.la.minecraftArguments.Replace("${user_properties}", "{}"); //留空



            //正版登陆未完成
            //--uuid ${auth_uuid}
            //--accessToken ${auth_access_token}

            #endregion

            //启动前相关设置
            Process p = new Process();
            p.StartInfo.Arguments =
                "-Xmx" + gi.CustomXmx + "m " +
                "-Xms" + gi.CustomXms + "m " +
                "-Dfml.ignoreInvalidMinecraftCertificates=true " + //忽略无效的MC证书
                "-Dfml.ignorePatchDiscrepancies=true " + //忽略补丁差异
                "-Djava.library.path=\"" + gi.navPath + "\" " + //Natives路径
                "-cp \"" + cp + "\"   " + //libraies和游戏jar
                gi.la.mainClass + " " + //启动主类
                gi.la.minecraftArguments; //给启动主类的参数
            p.StartInfo.FileName = Properties.Settings.Default.CustomJavaPath; //自定义java路径
            p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory; //设定工作目录为当前目录
            p.StartInfo.UseShellExecute = false; //不使用系统外壳启动
            //启动游戏
            p.Start();
            //返回
            //System.IO.File.WriteAllText("test.bat", "java.exe " + p.StartInfo.Arguments);
            return p;




        }
        #endregion


    }
}
