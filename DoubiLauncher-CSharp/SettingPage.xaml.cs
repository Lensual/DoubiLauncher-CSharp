using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace DoubiLauncher_CSharp
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Page
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SettingPage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 页面卸载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
           (new Action(() => { Properties.Settings.Default.Save(); })).BeginInvoke(null,null);
        }

        private void Btn_SelectJava_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Java|javaw.exe;java.exe";
            if (file.ShowDialog() == DialogResult.OK)
            {
                Txt_CustomJavaPath.Text = file.FileName;
            }
        }


        private void Chk_CustomJavaPath_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)Chk_CustomJavaPath.IsChecked)
            {
                Txt_CustomJavaPath.IsEnabled = true;
                Btn_SelectJava.IsEnabled = true;
            }
            else
            {
                Txt_CustomJavaPath.Text = "javaw.exe";
                Txt_CustomJavaPath.IsEnabled = false;
                Btn_SelectJava.IsEnabled = false;
            }
        }
    }
}
