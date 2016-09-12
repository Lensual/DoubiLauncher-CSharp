using DoubiLauncher_CSharp.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using MinecraftServerPing_CSharp;
using System.Windows.Threading;

namespace DoubiLauncher_CSharp
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            Application.Current.Properties["txt_Notice"] = txt_Notice;
        }
        #endregion

        
        #region 启动按钮
        private void btn_Launch_Click(object sender,RoutedEventArgs e)
        {
            if (GameList.Items.Count==0)
            {
                MessageBox.Show("未找到游戏");
                return;
            }
            else if (Settings.Default.UserName=="")
            {
                MessageBox.Show("没有设置玩家名");
                return;
            }

            try
            {
                #region 开始启动游戏

                GameInfo[] game = (GameInfo[])MainWindow.game.ToArray(typeof(GameInfo));
                game[GameList.SelectedIndex].libPath = AppDomain.CurrentDomain.BaseDirectory + @".minecraft\libraries";
                game[GameList.SelectedIndex].navPath = AppDomain.CurrentDomain.BaseDirectory + @".minecraft\natives";
                game[GameList.SelectedIndex].UserName = Settings.Default.UserName;
                game[GameList.SelectedIndex].CustomXmx = Settings.Default.CustomXmx.ToString();
                game[GameList.SelectedIndex].CustomXms = Settings.Default.CustomXms.ToString();
                game[GameList.SelectedIndex].gameDirectory = AppDomain.CurrentDomain.BaseDirectory + @".minecraft";
                game[GameList.SelectedIndex].gameAssets = AppDomain.CurrentDomain.BaseDirectory + @".minecraft\assets";

                LaunchHelper.Launch(game[GameList.SelectedIndex]);
                Application.Current.Shutdown();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("在 " + 
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "." + 
                    System.Reflection.MethodBase.GetCurrentMethod().Name + 
                    "发生了\n" + 
                    ex.ToString());
                Application.Current.Shutdown();
                //throw;
            }
        }
        #endregion

        #region 将游戏载入列表
        private void GameList_Loaded(object sender,RoutedEventArgs e )
        {
            //异步获取服务器状态
            GetServerStatus(null,null);
            //向列表添加游戏
            try
            {
                foreach (GameInfo game in MainWindow.game.ToArray())
                {
                    GameList.Items.Add(game.versionName);
                    GameList.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }
        #endregion
        #region 访问Doubi Launcher主页
        private void lbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://www.dreamerstudio.net/doubilauncher");
        }
        #endregion

        public void GetServerStatus(object sender, MouseButtonEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(s =>
            {
                try
                {
                    Dispatcher.Invoke(new Action(() => {
                        lbl_ServerStatus.Content = "服务器状态获取中...";
                    }));
                    MinecraftPingReply reply = new MinecraftPing().getPing(new MinecraftPingOptions().setHostname("zj1.jenetworks.net").setPort(26000));
                    Dispatcher.Invoke(new Action(() => {
                        lbl_ServerStatus.Content = reply.description + "  --  " + reply.getPlayers().getOnline() + "/" + reply.getPlayers().getMax();
                    }));
                }
                catch (Exception)
                {
                    Dispatcher.Invoke(new Action(() => {
                        lbl_ServerStatus.Content = "获取失败";
                    }));
                    //throw;
                }
            });
        }
    }
}

