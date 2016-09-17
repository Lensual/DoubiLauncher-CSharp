using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoubiLauncher_CSharp
{
    /// <summary>
    /// AboutPage.xaml 的交互逻辑
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
        }
        private void label1_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://lensual.dreamerstudio.net/blog");
        }
        private void label2_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://www.dreamerstudio.net/doubilauncher");
        }
        private void label3_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://github.com/Lensual/DoubiLauncher-CSharp");
        }

        private void label6_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, "561312630");
            MessageBox.Show("复制成功喵");
        }

    }
}
