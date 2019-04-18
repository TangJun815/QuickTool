using EventHook;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            //First Lunch Show MianWindow
            ShowWindow();
            //Application.Current.MainWindow = new MainWindow();
            //Application.Current.MainWindow.Show();

            MoniterKeyboardStart();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            MoniterKeyboardStop();
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }

        #region 键盘快捷键监控
        /*
         * 功能说明：
         * 1、Escape键：关闭窗口
         * 2、F8：显示窗口
         * 3、RightAlt + 0：显示窗口（备注：已取消）
         */
        string monitorWakeUpString = string.Empty;
        void MoniterKeyboardStart()
        {

            KeyboardWatcher.Start();
            KeyboardWatcher.OnKeyInput += (s, e) =>
            {
                Debug.WriteLine(string.Format("Key {0} event of key {1}", e.KeyData.EventType, e.KeyData.Keyname));

                if (e.KeyData.EventType == KeyEvent.down && e.KeyData.Keyname.ToLower() == "Escape".ToLower())
                {
                    CloseWindow();
                }

                #region F8控制方式
                if(e.KeyData.EventType == KeyEvent.down && e.KeyData.Keyname.ToLower() == "F8".ToLower())
                {
                    ShowWindow();
                }
                #endregion

                #region RightAlt+D0控制方式
                /*
                if (e.KeyData.EventType == KeyEvent.down && e.KeyData.Keyname.ToLower() == "RightAlt".ToLower())
                {
                    monitorWakeUpString = e.KeyData.Keyname;
                }
                else if (e.KeyData.EventType == KeyEvent.down && e.KeyData.Keyname.ToLower() == "D0".ToLower())
                {
                    monitorWakeUpString += "+" + e.KeyData.Keyname;
                    if (monitorWakeUpString.ToLower() == "RightAlt+D0".ToLower())
                    {
                        ShowWindow();
                        monitorWakeUpString = string.Empty;
                    }
                }
                */
                #endregion
            };
        }
        void MoniterKeyboardStop()
        {
            KeyboardWatcher.Stop();
        }
        void ShowWindow()
        {
            this.Dispatcher.Invoke(() =>
            {
                if (Application.Current.MainWindow == null)
                {
                    Application.Current.MainWindow = new TreeWindow();
                    Application.Current.MainWindow.Show();
                }
            });

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
        #endregion
    }
}
