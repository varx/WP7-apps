using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Scheduler;

namespace liveCountDown
{
    public partial class MainPage : PhoneApplicationPage
    {

        private string periodicTaskName = "liveCountDown";
        private IsolatedStorageSettings isoSetting = IsolatedStorageSettings.ApplicationSettings;
        private Boolean isFirstTime;
        private Boolean pickAlert=true;
        private Boolean isActived = false;
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            initSetting();
            updateAgent();
        }

        //！！！！
        //第一次启动后没有设置倒计时但启动了后台！！

        //更新后台
        private void updateAgent()
        {
            if (!isActived)
                return;
            PeriodicTask periodicTask = new PeriodicTask(periodicTaskName);

            periodicTask.Description = "liveCountDown Agent";
            periodicTask.ExpirationTime = System.DateTime.Now.AddDays(14);

            // If the agent is already registered with the system,
            if (ScheduledActionService.Find(periodicTaskName) != null)
            {
                ScheduledActionService.Remove("liveCountDown");
            }
                ScheduledActionService.Add(periodicTask);


                //调试用
                //ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(10));
        }

        //确认修改
        private void IconButton_Click(object sender, EventArgs e)
        {
            textBoxDay_LostFocus(null, null);
            string msg = textBoxMsg.Text;
            int days = Convert.ToInt32(textBoxDay.Text);
            if (days < 1)
                return;
            //if (days < 1)
            //{
            //    MessageBox.Show("请设置一个大于今天的日期");
            //    return;
            //}
            isActived = true;
            DateTime date = datePickerDate.Value.Value;
            updateIsolate(msg, date);
            updateAgent();
            if (!isoSetting.Contains("isActived"))
                isoSetting.Add("isActived", true);
            updateTile(msg, days);
            if(!isFirstTime)
                MessageBox.Show("磁贴已设置");
        }

        //读取设置
        private void initSetting()
        {
            if (isoSetting.Contains("msg"))
            {
                string msg = (string)isoSetting["msg"];
                DateTime date = (DateTime)isoSetting["date"];
                int days = (date - DateTime.Today).Days;

                textBoxDay.Text = days.ToString();
                textBoxMsg.Text = msg;
                datePickerDate.Value = date;
            }
            if (isoSetting.Contains("isActived"))
            {
                isActived = true;
            }
        }


        //更新设置
        private void updateIsolate(string msg,DateTime date)
        {
            isoSetting.Remove("msg");
            isoSetting.Remove("date");
            isoSetting.Add("msg",msg);
            isoSetting.Add("date", date);
        }

        //更新磁贴
        private void updateTile(string msg,int days)
        {
            ShellTile NowTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("TileID=2"));
            StandardTileData TileData = new StandardTileData
            {
                BackgroundImage = new Uri("background.jpg", UriKind.Relative),
                Title = "live倒计时",
                Count = days,
                BackTitle = "提醒",
                BackContent = msg,
                //BackBackgroundImage = new Uri("Blue.jpg", UriKind.Relative)
            };
            if (NowTile == null)
            {
                isFirstTime = true;
                ShellTile.Create(new Uri("/MainPage.xaml?TileID=2", UriKind.Relative), TileData);
            }
            else
            {
                isFirstTime = false;
                NowTile.Update(TileData);
            }
            //更新日期
            if (isoSetting.Contains("lastUpdate"))
            {
                isoSetting["lastUpdate"] = DateTime.Today;
            }
            else
            {
                isoSetting.Add("lastUpdate",DateTime.Today);
            }
            isoSetting.Save();
        }

        //更改天数
        private void pickerChange(object sender, DateTimeValueChangedEventArgs e)
        {
            if (!pickAlert)
            {
                pickAlert = true;
                return;
            }
            DateTime date = datePickerDate.Value.Value;
            DateTime today = DateTime.Today;
            int days = (date - today).Days;
            if (days < 1)
            {
                days = 0;
                pickAlert = false;
                datePickerDate.Value = today;
                MessageBox.Show("请设置一个大于明天的日期。");
            }
            if (days > 99)
            {
                days = 0;
                pickAlert = false;
                datePickerDate.Value = today;
                MessageBox.Show("倒计时上限99天。");
            }
            textBoxDay.Text = days.ToString();
        }

        //更改日期
        private void textBoxDay_LostFocus(object sender, RoutedEventArgs e)
        {
            int days=0;
            try
            {
                days = Convert.ToInt32(textBoxDay.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入整数日期");
                pickAlert = false;
                textBoxDay.Text = "0";
                textBoxDay.SelectAll();
                datePickerDate.Value = DateTime.Today;
                
                return;
            }
            if (days < 1)
            {
                MessageBox.Show("请设置一个大于0的天数");
                pickAlert = false;
                textBoxDay.SelectAll();
                datePickerDate.Value = DateTime.Today;
                
                return;
            }
            DateTime date = DateTime.Today.AddDays(days);
            datePickerDate.Value = date;
        }


        private void IconDelete_Click(object sender, EventArgs e)
        {
            
            MessageBoxResult result= MessageBox.Show("确实要取消桌面倒计时并清除磁贴吗？","确认",MessageBoxButton.OKCancel);
            if (MessageBoxResult.OK == result)
            {
                isActived = false;
                isoSetting.Remove("isActived");
                string msg = textBoxMsg.Text;
                DateTime date = datePickerDate.Value.Value;
                //int days = Convert.ToInt32(textBoxDay.Text);
                updateIsolate(msg, date);
                isoSetting.Remove("lastUpdate");
                ShellTile NowTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("TileID=2"));
                if (NowTile != null)
                    NowTile.Delete();
                if (ScheduledActionService.Find(periodicTaskName) != null)
                {
                    ScheduledActionService.Remove(periodicTaskName);
                }
            }
            else
            {
                return;
            }
        }

        private void IconHelp_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/help.xaml", UriKind.Relative));
        }

        private void textBoxDay_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxDay.SelectAll();
        }

        private void datePickerDate_LostFocus(object sender, RoutedEventArgs e)
        {

        }

    }
}