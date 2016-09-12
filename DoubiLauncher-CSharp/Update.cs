using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Net;
using Microsoft.VisualBasic.Devices;

namespace DoubiLauncher_CSharp
{
    public static class Update
    {
        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="url">更新服务器地址</param>
        public static void update(string url)
        {
            try
            {
                #region 查询更新脚本
                string response = Network.cURL(url +
                    "?Command=WhatShouldIdo" +
                    "&Build=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build +
                    "&ClientVersion=" + Properties.Settings.Default.ClientVersion +
                    "&exeName=" + Process.GetCurrentProcess().ProcessName + ".exe");
                #endregion
                #region 执行脚本
                string result = runscript(response);
                if (result != null)
                {
                    MessageBox.Show(result, "升级脚本执行错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                #endregion
            }
            catch (Exception)
            {
                //throw;
            }  
        }
        #endregion

        #region 升级脚本解析
        /// <summary>
        /// 运行更新脚本
        /// </summary>
        /// <param name="script">要执行的脚本</param>
        /// <returns>正常返回Null 错误返回错误信息</returns>
        public static string runscript(string script)
        {
            #region 格式化语句
            //去掉回车换行
            script = script.Replace("\n", "");
            //分离出每个语句行
            string[] cmdline;
            cmdline = script.Split(");".ToCharArray());
            #endregion
            #region 按顺序执行命令循环
            for (int i = 0; i < cmdline.Length; i++)
            {
                #region 分隔命令和参数
                string[] tmp = cmdline[i].Split("(".ToCharArray());
                string cmd = tmp[0]; //命令
                string[] parameter = null;
                if (tmp.Length!=1) //如果有参数
                {
                    parameter = tmp[1].Split(",".ToCharArray()); //参数
                }
                #endregion
                try
                {
                    #region 命令switch
                    switch (cmd)
                    {
                        default:
                            break;
                        case "echo":
                            #region echo(内容);
                            MessageBox.Show(parameter[0]);
                            #endregion
                            break;
                        case "ask":
                            #region ask(问题,标识);
                            if (MessageBox.Show(parameter[0], "询问", MessageBoxButton.YesNo, MessageBoxImage.Information) != MessageBoxResult.Yes)
                            {
                                bool notfound = true;
                                for (int j = 0; j < cmdline.Length; j++) //寻找标识循环
                                {
                                    if (cmdline[j] == "sign(" + parameter[1])
                                    {
                                        i = j; //通过修改变量i实现跳转
                                        notfound = false;
                                        break;
                                    }
                                }
                                if (notfound)
                                {
                                    throw new System.Exception("升级脚本错误 找不到跳转标识");
                                }
                            }
                            #endregion
                            break;
                        case "7z":
                            #region 7z(存放路径,文件路径);
                            File.WriteAllBytes(@".\7za.exe", Properties.Resources._7za);
                            Process p_7z = System.Diagnostics.Process.Start(@".\7za.exe", "-mx9 a " + parameter[0] + " " + parameter[1]);
                            p_7z.WaitForExit();
                            File.Delete(@".\7za.exe");
                            if (p_7z.ExitCode != 0)
                            {
                                throw new Exception("压缩时发生错误 7za退出代码" + p_7z.ExitCode);
                            }
                            #endregion
                            break;
                        case "un7z":
                            #region un7z(文件路径,存放路径);
                            File.WriteAllBytes(@".\7za.exe", Properties.Resources._7za);
                            Process p_un7z = System.Diagnostics.Process.Start(@".\7za.exe", "x " + parameter[0] + " -o " + parameter[1]);
                            p_un7z.WaitForExit();
                            File.Delete(@".\7za.exe");
                            if (p_un7z.ExitCode != 0)
                            {
                                throw new Exception("解压时发生错误 7za退出代码" + p_un7z.ExitCode);
                            }
                            #endregion
                            break;
                        case "download":
                            #region download(地址,存放路径);
                            Microsoft.VisualBasic.Devices.Network n = new Microsoft.VisualBasic.Devices.Network();
                            n.DownloadFile(parameter[0], parameter[1], "", "", true, 100, true, UICancelOption.ThrowException);
                            #endregion
                            break;
                        case "start":
                            #region start(路径,参数,等待);
                            Process p_start = System.Diagnostics.Process.Start(parameter[0], parameter[1]);
                            if (parameter[2] == "true") //等待
                            {
                                p_start.WaitForExit();
                            }
                            #endregion
                            break;
                        case "rename": //与move相同 rename(旧名字路径,新名字路径);
                        case "move":
                            #region move(源文件路径,目标路径);
                            if (File.Exists(parameter[0])) //判断源是文件还是文件夹
                            {
                                FileSystem.MoveFile(parameter[0],parameter[1],true);
                            }
                            else if (Directory.Exists(parameter[0]))
                            {
                                FileSystem.MoveDirectory(parameter[0], parameter[1], true);
                            }
                            else
                            {
                                throw new FileNotFoundException("源文件或文件夹不存在");
                            }
                            #endregion
                            break;
                        case "copy":
                            #region copy(源文件路径,目标路径);
                            if (File.Exists(parameter[0])) //判断源是文件还是文件夹
                            {
                                FileSystem.CopyFile(parameter[0], parameter[1], true);
                            }
                            else if (Directory.Exists(parameter[0]))
                            {
                                FileSystem.CopyDirectory(parameter[0], parameter[1], true);
                            }
                            else
                            {
                                throw new FileNotFoundException("源文件或文件夹不存在");
                            }
                            #endregion
                            break;
                        case "mkdir":
                            #region mkdir(文件夹路径);
                            Directory.CreateDirectory(parameter[0]);
                            #endregion
                            break;
                        case "delete":
                            #region delete(文件路径);
                            if (File.Exists(parameter[0])) //判断源是文件还是文件夹
                            {
                                File.Delete(parameter[0]);
                            }
                            else if (Directory.Exists(parameter[0]))
                            {
                                Directory.Delete(parameter[0]);
                            }
                            #endregion
                            break;
                        case "deleteme":
                            #region deleteme();
                            Process p_deleteme = new Process();
                            p_deleteme.StartInfo.CreateNoWindow = true;
                            p_deleteme.StartInfo.FileName = "cmd.exe";
                            p_deleteme.StartInfo.Arguments = "/c taskkill /F /PID " + Process.GetCurrentProcess().Id + " & choice /t 2 /d y /n >nul & del " + Process.GetCurrentProcess().ProcessName + ".exe";
                            p_deleteme.Start();
                            Application.Current.Shutdown();
                            #endregion
                            break;
                        case "end":
                            #region end();
                            return null;
                            #endregion
                            //break;
                        case "goto":
                            #region goto(标识);
                            for (int j = 0; j < cmdline.Length; j++)
                            {
                                if (cmdline[j]=="sign(" + parameter[0])
                                {
                                    i = j; //通过修改变量i实现跳转
                                    break;
                                }
                            }
                            throw new Exception("升级脚本错误 找不到跳转标识");
                        #endregion
                            //break;
                        case "sign":
                            //标识名称
                            break;
                        case "upversion":
                            #region upversion(新版本号);
                            Properties.Settings.Default.ClientVersion = parameter[0];
                            Properties.Settings.Default.Save();
                            #endregion
                            break;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return "升级时发生错误 \n" +
                       "脚本语句：" + cmdline[i] + ");\n" +
                       ex.Message;
                }
            }
            #endregion
            return null;
        }

        #endregion

        #region 获取公告
        /// <summary>
        /// 获取公告
        /// </summary>
        /// <param name="url">公告服务器地址</param>
        public static void GetNotice(string url)
        {
            try
            {
                MainWindow.notice = Network.cURL(url + "?Command=GetNotice");
                ((System.Windows.Controls.TextBlock)Application.Current.Properties["txt_Notice"]).Dispatcher.BeginInvoke(new Action(() =>
                {
                    ((System.Windows.Controls.TextBlock)Application.Current.Properties["txt_Notice"]).Text = MainWindow.notice;
                }));
            }
            catch (Exception)
            {

                //throw;
            }
        }
        #endregion



    }
}
