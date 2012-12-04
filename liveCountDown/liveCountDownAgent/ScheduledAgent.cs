using System.Windows;
using Microsoft.Phone.Scheduler;
using System.IO.IsolatedStorage;
using System;
using System.Linq;
using Microsoft.Phone.Shell;
namespace liveCountDownAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent 构造函数，初始化 UnhandledException 处理程序
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // 订阅托管的异常处理程序
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// 出现未处理的异常时执行的代码
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 出现未处理的异常；强行进入调试器
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// 运行计划任务的代理
        /// </summary>
        /// <param name="task">
        /// 调用的任务
        /// </param>
        /// <remarks>
        /// 调用定期或资源密集型任务时调用此方法
        /// </remarks>
        /// 
        private void updateTile(int days)
        {
            string secondTitle;
            if (days == 0)
            {
                secondTitle = "提醒到期。";
            }
            else
            {
                secondTitle = "提醒";
            }
            ShellTile NowTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("TileID=2"));
            if (NowTile != null)
            {
                StandardTileData NewTileData = new StandardTileData
                {
                    BackTitle = secondTitle,
                    Count = days
                };
                NowTile.Update(NewTileData);
            }
        }

        //private void toast(string msg)
        //{
        //    ShellToast toast = new ShellToast();
        //    toast.Title = "test";
        //    toast.Content = msg;
        //    toast.Show();
        //}
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: 添加用于在后台执行任务的代码


            //××××××××××××××××××××××××××××××××××
            //ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(10));
            //××××××××××××××××××××××××××××××××××××

            IsolatedStorageSettings isoSetting = IsolatedStorageSettings.ApplicationSettings;
            DateTime today = DateTime.Today;
            
            if (isoSetting.Contains("lastUpdate"))
            {
                if ((DateTime)isoSetting["lastUpdate"] != today)
                {
                    isoSetting["lastUpdate"] = today;
                    //toast("update now");
                }
                else
                {
                    //不更新
                    NotifyComplete();
                    return;
                }
            }
            else
            {
                isoSetting.Add("lastUpdate", today);
                //toast("no Last");
            }
            isoSetting.Save();

            DateTime date = (DateTime)isoSetting["date"];
            int days = (date - today).Days;
            if (days < 0)
                days = 0;
            updateTile(days);
            NotifyComplete();
        }
    }
}