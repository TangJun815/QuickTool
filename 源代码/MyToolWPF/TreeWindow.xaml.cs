using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace MyTool
{
    /// <summary>
    /// TreeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TreeWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        public TreeWindow()
        {
            InitializeComponent();
        }
        void CloseWindow()
        {

            this.Dispatcher.Invoke(() =>
            {
                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Close();
                    Application.Current.MainWindow = null;
                }
            });
        }

        #region 树形功能实现


        private void tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tv = sender as TreeView;
            var s = tv.SelectedItem as XmlElement;
            ProcessNode(s);
            if (s != null && this.cbIsCloseWindowAftetClick.IsChecked.HasValue && this.cbIsCloseWindowAftetClick.IsChecked.Value)
                CloseWindow();
        }
        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            var target = sender as TextBlock;
            if (target != null)
            {
                target.Background = Brushes.LightGray;
            }
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            var target = sender as TextBlock;
            if (target != null)
            {
                target.Background = Brushes.Transparent;
            }
        }
        private void ProcessNode(XmlElement node)
        {
            if (node == null)
                return;

            var name = node.HasAttribute("Name") ? node.Attributes["Name"].Value : null;
            var target = node.HasAttribute("Target") ? node.Attributes["Target"].Value : null;
            var type = node.HasAttribute("Type") ? node.Attributes["Type"].Value : null;
            if (target != null && type != null)
            {
                switch (type.ToLower())
                {
                    case "processstart":
                        OpenProcess(target);
                        break;
                    case "explorer":
                        if (this.cbIsNewExplorerProcess.IsChecked.Value)
                            Helper.Explorer.OpenNewExplorer(target);
                        else
                            Helper.Explorer.Explore(target);
                        break;
                    default:
                        break;
                }
            }

            if (node.HasChildNodes)
            {
                foreach (XmlElement item in node.ChildNodes)
                {
                    ProcessNode(item);
                }
            }
        }
        private async void btnCloseAllExplorerWindow_Click(object sender, RoutedEventArgs e)
        {
            this.labStateForCloseAllExplorerWindows.Content = "Windows Closing...";
            await Task.Run(() =>
            {
                Helper.Explorer.CloseAllWindows();
            });
            this.labStateForCloseAllExplorerWindows.Content = "Windows Closed";
        }
        private void OpenProcess(string fileName)
        {
            Process.Start(fileName);
        }
        #endregion

        #region 快速跳转当前系统中特定进程窗口
        private void ShowProcessWindowList()
        {
            var obs = new ObservableCollection<Process>();
            var list = Process.GetProcesses().Where(pp =>
            pp.MainWindowTitle != string.Empty
            &&
            (
            pp.ProcessName.StartsWith("explorer", StringComparison.OrdinalIgnoreCase) || pp.ProcessName.StartsWith("devenv", StringComparison.OrdinalIgnoreCase) || pp.ProcessName.StartsWith("ssms", StringComparison.OrdinalIgnoreCase) || pp.ProcessName.StartsWith("onenote", StringComparison.OrdinalIgnoreCase)
            )).OrderBy(q => q.ProcessName).ThenBy(q => q.MainWindowTitle).ToList();
            foreach (var item in list)
            {
                obs.Add(item);
            }
            //this.lv.ItemsSource = obs;
        }

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var item = this.lv.SelectedItem as Process;
            //if (item != null)
            //{
            //    CloseWindow();
            //    SwitchToThisWindow(item.MainWindowHandle, true);
            //}
        }
        #endregion


        #region 类似QQ的收缩功能，逻辑实现代码
        int QQ_MODE = 0, QQ_T = 3, QQ_XY = 6;//0为不停靠,1为X轴,2为Y轴,3为顶部；QQ_T为显示的像素；QQ_XY为误差
        Timer timer;
        private void StartMonitorWindow()
        {
            this.timer = new Timer();
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Interval = 500;
            this.timer.Start();
        }
        private void StopMonitorWindow()
        {
            this.timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {

                //如果左键按下就不处理当前逻辑[是否收缩]
                //if (System.Windows.Input.MouseButton == System.Windows.Input.MouseButtons.Left)
                //    return;

                //鼠标的位置
                double x = GetMousePosition().X, y = GetMousePosition().Y;
                Debug.WriteLine(@"{0},{1}", x, y);

                //鼠标移动到窗口内，显示
                if (x > (this.Left - QQ_XY)
                    &&
                    x < (this.Left + this.Width + QQ_XY)
                    &&
                    y > (this.Top - QQ_XY)
                    &&
                    y < (this.Top + this.Height + QQ_XY))
                {
                    if (this.QQ_MODE == 1)

                    { this.Left = QQ_T; this.Top = this.Top; }
                    else if (this.QQ_MODE == 2)
                    { this.Left = SystemParameters.WorkArea.Width - this.Width - QQ_T; this.Top = this.Top; }
                    else if (this.QQ_MODE == 3)
                    {
                        this.Left = this.Left; this.Top = QQ_T;
                    }
                }
                else//鼠标移动到窗口外，隐藏
                {
                    if (this.Top <= QQ_T)//上
                    {
                        this.Left = this.Left;
                        this.Top = QQ_T - this.Height;
                        this.QQ_MODE = 3;
                    }
                    else if (this.Left <= QQ_T)//左
                    {
                        this.Left = QQ_T - this.Width;
                        this.Top = this.Top;
                        this.QQ_MODE = 1;
                    }
                    else if (this.Left >= SystemParameters.WorkArea.Width - this.Width - QQ_T)//右
                    {
                        this.Left = SystemParameters.WorkArea.Width - QQ_T;
                        this.Top = this.Top;
                        this.QQ_MODE = 2;
                    }
                    else
                        this.QQ_MODE = 0;
                }
            });
        }


        private void Window_LocationChanged(object sender, EventArgs e)
        {
            WindowMoving();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.StartMonitorWindow();
            ShowProcessWindowList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.StopMonitorWindow();
        }

        //移动窗体时，解决QQ逻辑
        private void WindowMoving()
        {
            this.QQ_MODE = 0;
        }

        #region 获取鼠标位置

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);


        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        #endregion

        #endregion

    }
}
