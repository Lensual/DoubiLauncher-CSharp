using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DoubiLauncher_CSharp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 公告变量
        /// </summary>
        public static string notice { get; set; } = "正在获取公告";



        #region 构造函数
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region 窗口移动
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion
        #region 菜单和Frame

        /// <summary>
        /// 正在切换页面
        /// </summary>
        private bool Switching = false;
        private void MenuLabel_MouseDown(object sender, RoutedEventArgs e)
        {
            if (!Switching) //正在切换则忽略
            {
                Switching = true; //声明动画开始运行
                double time = 300; //设定动画持续时间

                //菜单滑动效果
                ThicknessAnimation ta = new ThicknessAnimation();
                ta.From = Menu_Decoration.Margin;   //起始值
                ta.To = new Thickness(((ContentControl)sender).Margin.Left,   //结束值
                    Menu_Decoration.Margin.Top, 
                    Menu_Decoration.Margin.Right,
                    Menu_Decoration.Margin.Bottom);
                ta.Duration = TimeSpan.FromMilliseconds(time);   //动画持续时间
                ta.Completed += delegate (object obj, EventArgs a) //注册动画完成事件的回调
                {
                    Switching = false; //声明动画结束
                };
                //Frame淡出
                DoubleAnimation da = new DoubleAnimation();
                da.From = 1;
                da.To = 0;
                da.Duration = TimeSpan.FromMilliseconds(time / 2);
                da.Completed += delegate (object obj, EventArgs a)
                {
                    //Frame切换
                    switch (((ContentControl)sender).Name)
                    {
                        case "Label_MainPage":
                            myFrame.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
                            break;
                        case "Label_SettingPage":
                            myFrame.Navigate(new Uri("SettingPage.xaml", UriKind.Relative));
                            break;
                        case "Label_AboutPage":
                            myFrame.Navigate(new Uri("AboutPage.xaml", UriKind.Relative));
                            break;
                    }
                    //Frame淡入
                    da = new DoubleAnimation();
                    da.From = 0;
                    da.To = 1;
                    da.Duration = TimeSpan.FromMilliseconds(time / 2);
                    myFrame.BeginAnimation(UIElement.OpacityProperty, da);
                };

                //开始动画
                Menu_Decoration.BeginAnimation(Rectangle.MarginProperty, ta);
                myFrame.BeginAnimation(UIElement.OpacityProperty, da);
            }
        }

        #endregion
        #region 退出按钮
        private bool closing = false; //防止多次点击反复运行动画
        private void btn_Exit_Click(Object sender, RoutedEventArgs e)
        {
            if (!closing)
            {
                double time = 300; //设定动画持续时间
                closing = true;
                DoubleAnimation da = new DoubleAnimation();
                da.From = 1;
                da.To = 0;
                da.Duration = TimeSpan.FromMilliseconds(time / 2);
                da.Completed += delegate (object obj, EventArgs a)
                {
                    Application.Current.Shutdown();
                };
                Application.Current.MainWindow.BeginAnimation(UIElement.OpacityProperty, da);
            }
        }

        #endregion

        #region 窗口加载完毕

        /// <summary>
        /// 游戏列表
        /// </summary>
        public static ArrayList game = new ArrayList();
        /// <summary>
        /// 检查更新委托
        /// <param name="url">更新服务器url</param>
        /// </summary>
        delegate void DelegateUpdate(string url);
        /// <summary>
        /// 获取公告委托
        /// </summary>
        /// <param name="url">公告服务器url</param>
        /// <returns></returns>
        delegate void DelegateGetNotice(string url);
        private void MainWindow_Loaded(object sender,RoutedEventArgs e)
        {
            #region 调用委托检查更新
            DelegateUpdate update = new DelegateUpdate(Update.update);
            update.BeginInvoke(Properties.Settings.Default.updateUrl, null, null);
            #endregion

            #region 调用委托获取公告
            DelegateGetNotice GetNotice = new DelegateGetNotice(Update.GetNotice);
            GetNotice.BeginInvoke(Properties.Settings.Default.updateUrl, null, null);
            #endregion

            #region 查找游戏
            try
            {
            #endregion

                string[] versions = System.IO.Directory.GetDirectories(
                    AppDomain.CurrentDomain.BaseDirectory + @".minecraft\versions");
                foreach (string path in versions)
                {
                    game.Add(new GameInfo(path));
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }
        #endregion

    }
}
