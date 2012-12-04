using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Data;
using AmCharts.Windows.QuickCharts;
using System.ComponentModel;

namespace yingMoney
{
    public partial class MainPage : PhoneApplicationPage
    {
        private IsolatedStorageSettings isoSetting = App.isoSetting;
        private YingDB APPDB;
        private ObservableCollection<PData> _pieData;
        private ObservableCollection<LData> _LineData;
        private ObservableCollection<HData> _HistoryData;
        private ObservableCollection<HData> _RecentData;

        private bool DBLOCK = false;

        private Home_info homeInfo;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            
            // 将 listbox 控件的数据上下文设置为示例数据
            //DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            //MainPara.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(MainPara_SelectionChanged);
        }
        public ObservableCollection<PData> PieData { get { return _pieData; } }
        public ObservableCollection<LData> LineData { get { return _LineData; } }

        void MainPara_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPara.SelectedIndex == 1)
            {
                if(ListBoxHistory.ItemsSource==null)
                    setHistoryData(DateTime.Today);
                if (_pieData == null)
                {
                    setPieData(7);
                    pieChart.DataContext = this;
                }
            }
            if (MainPara.SelectedIndex == 2)
            {
                if (_LineData == null)
                {
                    //初始一月
                    setLineData(30);
                    LineChart.DataContext = this;
                }
            }
        }

        private void setLineData(int days)
        {
            DateTime start = DateTime.Today.AddDays(-days);

            _LineData = new ObservableCollection<LData>(
                (from p in APPDB.Outgoing
                 where p.Time > start
                 group p by p.Time into g
                 select new LData
                 {
                    // time = g.Key.ToShortDateString(),
                    time="",
                     value = g.Sum(p => p.Money)/100
                 }
                 )
              );
            if (_LineData.Count != 0)
            {
                _LineData[0].time = start.ToShortDateString();
                _LineData.LastOrDefault().time = DateTime.Today.ToShortDateString();
            }
            //int count = _LineData.Count;
            //if (count < 3)
            //    return;
            //int step = count / 3;
            //int index = 0;
            //foreach (var i in _LineData)
            //{
            //    if (index % step != 0)
            //        i.time = "";
            //    index++;
            //}
            
        }

        private void setPieData(int days)
        {
            DateTime start = DateTime.Today.AddDays(-days);
            _pieData = new ObservableCollection<PData>(
                (from p in APPDB.Outgoing
                 where p.Time>start
                 group p by p.Main_id into g
                 join m in APPDB.Main_type on g.Key equals m.Id
                 select new PData { title=m.Name,value=g.Sum(p=>p.Money)/100}
                     )
                );
            //_pieData.Add(new PData { title = "吃的", value = 9 });
            //_pieData.Add(new PData { title = "穿的", value = 9 });
            
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if(APPDB==null)
                APPDB = new YingDB("Data Source = isostore:/yingDB.sdf");
            if (!APPDB.DatabaseExists())
            {
                copyDbFile();
            }
            App.APPDB = APPDB;

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = false;
            bw.DoWork += new DoWorkEventHandler(initHomePage);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InithomeCompleted);
            DBLOCK = true;
            bw.RunWorkerAsync();
            //initHomePage();
        }

        void InithomeCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            initResult result = (initResult)e.Result;

            homeInfo = result.HomeInfo;
            App.homeinfo = result.HomeInfo;
            _RecentData = result.RecentData;
            _pieData = result.pieData;
            _LineData = result.LineData;

            PanelHomeInfo.DataContext = homeInfo;
            pieChart.DataContext = this;
            LineChart.DataContext = this;
            ListBoxRecent.ItemsSource = _RecentData;
            setHistoryData(datePicker.Value.Value);
            progressBar1.Visibility = Visibility.Collapsed;
            DBLOCK = false;
        }

        //protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    if (APPDB != null)
        //    {
        //        APPDB.SubmitChanges();
        //        APPDB.Dispose();
        //    }
        //    base.OnNavigatedFrom(e);
        //}

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (APPDB != null)
            {
                APPDB.SubmitChanges();
                APPDB.Dispose();
            }
            base.OnBackKeyPress(e);
        }

        /**
         *  返回_pieData,_lineData,_RecentData,homeinfo
         */
        private void initHomePage(object sender, DoWorkEventArgs e)
        {
            YingDB DB = App.APPDB;
            Home_info HomeInfo = DB.Home_info.FirstOrDefault();
            DateTime today = DateTime.Today;
            initResult result = new initResult();
            //获取首页信息
            if (HomeInfo.Date.Year == today.Year)
            {
                if (HomeInfo.Date.Month != today.Month)
                {
                    HomeInfo.Mouthsum = 0;
                    HomeInfo.Daysum = 0;
                }
                else
                {
                    if (HomeInfo.Date != today)
                        HomeInfo.Daysum = 0;
                }
            }
            else 
            {
                HomeInfo.Weeksum = 0;
                HomeInfo.Daysum = 0;
            }
            HomeInfo.Date = today;
            if ((today - HomeInfo.Weekstart).Days > 7)
            {
                HomeInfo.Weeksum = 0;
                HomeInfo.Weekstart = today.AddDays(-(int)today.DayOfWeek);
            }
            result.HomeInfo = HomeInfo;

            //最近数据
            ObservableCollection<HData> recentData = new ObservableCollection<HData>(
                (
                from p in APPDB.Outgoing
                join g in APPDB.Sub_type on p.Sub_id equals g.Id
                orderby p.Id descending
                select new HData { Name=g.Name,value=p.Money}
                ).Take(7)
                );
            result.RecentData = recentData;

            //7天饼图数据
            DateTime start = DateTime.Today.AddDays(-7);
            ObservableCollection<PData> PieData = new ObservableCollection<PData>(
                (from p in APPDB.Outgoing
                 where p.Time > start
                 group p by p.Main_id into g
                 join m in APPDB.Main_type on g.Key equals m.Id
                 select new PData { title = m.Name, value = g.Sum(p => p.Money) / 100 }
                     )
                );
            result.pieData = PieData;

            //30天线图数据

            start = DateTime.Today.AddDays(-30);

            ObservableCollection<LData> LineData = new ObservableCollection<LData>(
                (from p in APPDB.Outgoing
                 where p.Time > start
                 group p by p.Time into g
                 select new LData
                 {
                     // time = g.Key.ToShortDateString(),
                     time = "",
                     value = g.Sum(p => p.Money) / 100
                 }
                 )
              );
            if (LineData.Count != 0)
            {
                LineData[0].time = start.ToShortDateString();
                LineData.LastOrDefault().time = DateTime.Today.ToShortDateString();
            }
            result.LineData = LineData;
            e.Result = result;
            
        }

        private void copyDbFile()
        {
            // Obtain the virtual store for the application.
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

            // Create a stream for the file in the installation folder.
            using (Stream input = Application.GetResourceStream(new Uri("yingDB.sdf", UriKind.Relative)).Stream)
            {
                // Create a stream for the new file in isolated storage.
                using (IsolatedStorageFileStream output = iso.CreateFile("yingDB.sdf"))
                {
                    // Initialize the buffer.
                    byte[] readBuffer = new byte[4096];
                    int bytesRead = -1;

                    // Copy the file from the installation folder to isolated storage. 
                    while ((bytesRead = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        output.Write(readBuffer, 0, bytesRead);
                    }
                }
            }
        }

        private void setHistoryData(DateTime date)
        {

            _HistoryData = new ObservableCollection<HData>(
                from p in APPDB.Outgoing
                join g in APPDB.Sub_type on p.Sub_id equals g.Id
                where p.Time == date
                select new HData { Name=g.Name, value=p.Money}
                );
            ListBoxHistory.ItemsSource = _HistoryData;
            long daySum=0;
            foreach (var i in _HistoryData)
            {
                daySum += i.value;
            }
            HDaySum.DataContext = daySum;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (DBLOCK)
                return;
            NavigationService.Navigate(new Uri("/View/AddRecord.xaml",UriKind.Relative));
        }

        private void gotoSetting(object sender, EventArgs e)
        {
            if (DBLOCK)
                return;
            NavigationService.Navigate(new Uri("/View/Setting.xaml", UriKind.Relative));
        }

        private void line_pre(object sender, RoutedEventArgs e)
        {
            if (DBLOCK)
                return;
            string now = (string)TextBlockLineTitle.Tag;
            if (now == "mouth")
            {
                TextBlockLineTitle.Text = "半年";
                TextBlockLineTitle.Tag = "year";
                LineChart.DataContext = null;
                setLineData(180);
                LineChart.DataContext = this;
            }
            else
            {
                if (now == "year")
                {
                    TextBlockLineTitle.Text = "一个月";
                    TextBlockLineTitle.Tag = "mouth";
                    LineChart.DataContext = null;
                    setLineData(30);
                    LineChart.DataContext = this;
                }
            }
        }

        private void History_pre(object sender, RoutedEventArgs e)
        {
            if (DBLOCK)
                return;
            datePicker.Value = datePicker.Value.Value.AddDays(-1);
            //setHistoryData(date);

        }

        private void History_next(object sender, RoutedEventArgs e)
        {
            if (DBLOCK)
                return;
            datePicker.Value = datePicker.Value.Value.AddDays(1);
        }

        private void changeHistoryData(object sender, DateTimeValueChangedEventArgs e)
        {
            setHistoryData(datePicker.Value.Value);
        }

        private void pie_pre(object sender, RoutedEventArgs e)
        {
            if (DBLOCK)
                return;
            pieChart.DataContext = null;
            string tag = (string)TextBlockPieTitle.Tag;
            if (tag == "week")
            {
                TextBlockPieTitle.Text = "一月";
                TextBlockPieTitle.Tag = "month";
                setPieData(30);
            }
            else {
                if (tag == "month")
                {
                    TextBlockPieTitle.Text = "半年";
                    TextBlockPieTitle.Tag = "year";
                    setPieData(180);
                }
                else
                {
                    TextBlockPieTitle.Text = "一周";
                    TextBlockPieTitle.Tag = "week";
                    setPieData(7);
                }
            }
            pieChart.DataContext = this;
        }

        private void pie_next(object sender, RoutedEventArgs e)
        {
            if (DBLOCK)
                return;
            pieChart.DataContext = null;
            string tag = (string)TextBlockPieTitle.Tag;
            if (tag == "week")
            {
                TextBlockPieTitle.Text = "半年";
                TextBlockPieTitle.Tag = "year";
                setPieData(180);
            }
            else
            {
                if (tag == "month")
                {
                    TextBlockPieTitle.Text = "一周";
                    TextBlockPieTitle.Tag = "week";
                    setPieData(7);
                }
                else
                {
                    TextBlockPieTitle.Text = "一月";
                    TextBlockPieTitle.Tag = "month";
                    setPieData(30);
                }
            }
            pieChart.DataContext = this;
        }

        private void IconAdd_Click(object sender, EventArgs e)
        {
            if (DBLOCK)
                return;
            NavigationService.Navigate(new Uri("/View/AddRecord.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DBLOCK)
                return;
            string now = (string)TextBlockLineTitle.Tag;
            if (now == "mouth")
            {
                TextBlockLineTitle.Text = "半年";
                TextBlockLineTitle.Tag = "year";
                LineChart.DataContext = null;
                setLineData(180);
                LineChart.DataContext = this;
            }
            else
            {
                if (now == "year")
                {
                    TextBlockLineTitle.Text = "一个月";
                    TextBlockLineTitle.Tag = "mouth";
                    LineChart.DataContext = null;
                    setLineData(30);
                    LineChart.DataContext = this;
                }
            }
        }

    }
    public class PData
    {
        public string title { get; set; }
        public double value { get; set; }
    }
    public class LData
    {
        public string time { get; set; }
        public double value { get; set; }
    }

    public class HData
    {
        public string Name { get; set; }
        public int value { get; set; }
    }


    public class MoneyFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            string money = value.ToString();
            if (money.Length == 0)
                return "0.00";
            if (money.Length == 2)
                return "0." + money;
            if (money.Length == 1)
                return "0.0" + money;
            return money.Insert(money.Length-2,".");
        }

        public object ConvertBack(object value, Type targetType, 
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class initResult
    {
        public Home_info HomeInfo;
        public ObservableCollection<PData> pieData;
        public ObservableCollection<LData> LineData;
        //public ObservableCollection<HData> HistoryData;
        public ObservableCollection<HData> RecentData;
    }
}