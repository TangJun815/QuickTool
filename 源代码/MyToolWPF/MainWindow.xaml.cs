using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDesktop1_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                //Process.Start(@"");
                Process.Start(@"D:/唐军/网盘/OneDrive/唐军/工作/公司/西网通信/项目处理/日常-操作/平台导航.html");
                Process.Start(@"C:\Program Files (x86)\Microsoft Office\root\Office16\ONENOTE.EXE");

                Process myProcess = new Process();
                var startInfo = new ProcessStartInfo(@"explorer.exe");
                startInfo.WindowStyle = ProcessWindowStyle.Maximized;

                myProcess.StartInfo = startInfo;

                startInfo.Arguments = @"C:\Users\tangj\Desktop\桌面临时文件夹";
                myProcess.Start();

                startInfo.Arguments = @"D:\唐军\持续处理\临时文件夹\下载";
                myProcess.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFootball_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"D:\唐军\网盘\微云同步盘\945045\相关人\唐军\工作\公司\Other\项目文档\球队管理\数据库\备份");
            Process.Start(@"C:\Users\tangj\Desktop\桌面临时文件夹");
            Process.Start(new ProcessStartInfo()
            {
                FileName = @"D:\Program Files\FileZilla FTP Client\filezilla.exe",
                WindowStyle = ProcessWindowStyle.Maximized,
            }
            );
            
            Process.Start(@"D:\唐军\网盘\OneDrive\唐军\工作\公司\西网通信\项目处理\日常-操作\RDP\119.6.254.28.RDP");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var startInfo = new ProcessStartInfo("explorer.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            startInfo.Arguments = @"D:\唐军\网盘\OneDrive\唐军\工作\公司\西网通信\项目处理\日常-操作\RDP";
            Process.Start(startInfo);

            startInfo.Arguments = @"D:\唐军\网盘\OneDrive\唐军\工作\公司\西网通信\项目处理\日常-操作";
            Process.Start(startInfo);

            Process.Start(@"D:\唐军\网盘\OneDrive\唐军\工作\公司\西网通信\项目处理\日常-工具\西网实用小工具\西网业务支撑系统实用工具.exe");

            Process.Start(@"C:\Program Files (x86)\Microsoft SQL Server\120\Tools\Binn\ManagementStudio\Ssms.exe");
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var commandParameter = ((Button)sender).CommandParameter.ToString().ToLower();
            switch (commandParameter)
            {
                case "vs":
                    Process.Start(@"D:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe");
                    break;
                case "sql":
                    Process.Start(@"C:\Program Files (x86)\Microsoft SQL Server\120\Tools\Binn\ManagementStudio\Ssms.exe");
                    break;
                case "folder_desktoptemp":
                    OpenExplorer(@"C:\Users\tangj\Desktop\桌面临时文件夹");
                    break;
                case "rdp":
                    OpenExplorer(@"D:\唐军\网盘\OneDrive\唐军\工作\公司\西网通信\项目处理\日常-操作\RDP");
                    break;
                case "folder_download":
                    OpenExplorer(@"D:\唐军\持续处理\临时文件夹\下载");
                    break;
                case "folder_tangguoer":
                    OpenExplorer(@"D:\唐军\网盘\微云同步盘\945045\相关人\唐果儿\成长记录");
                    break;
                case "":
                    break;

                default:
                    MessageBox.Show("按钮没有传递参数，不能执行未定义操作。");
                    break;
            }
        }

        
        private void OpenExplorer(string arguments, ProcessWindowStyle windowStyle = ProcessWindowStyle.Maximized)
        { 
            Process myProcess = new Process();
            var startInfo = new ProcessStartInfo(@"explorer.exe");
            startInfo.WindowStyle = windowStyle;

            myProcess.StartInfo = startInfo;

            startInfo.Arguments = arguments; 
            myProcess.Start();
        }
    }
}
