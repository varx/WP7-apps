using System;
using System.Collections.Generic;
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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq;
using Microsoft.Phone.Data.Linq.Mapping;
using System.ComponentModel;

namespace ningboBus
{
    public partial class MainPage : PhoneApplicationPage
    {

        private BackgroundWorker bw;
        private string msg;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            
            // 将 listbox 控件的数据上下文设置为示例数据
            //DataContext = App.ViewModel;
            //this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            //List<string> names = new List<string>();
            //string namesString = "Fernando Sucre,Scofield,Alexander Mahone,Theodore Bagwell,Sara Tancredi ,Lincoln Burrows,John Abruzzi,Fluorine";
            //foreach (var name in namesString.Split(','))
            //    names.Add(name);
            //this.autoCompleteBox2.ItemsSource = names;
            //this.autoCompleteBox2.ItemFilter += searchStation;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if(App.stationFrom!=null)
                staionFromText.Text = App.stationFrom;
            if(App.stationTo!=null)
                staionToText.Text = App.stationTo;
        }

        //站站查询
        private void stationSearchButton_Click(object sender, RoutedEventArgs e)
        {
            stationSearchButton.Content = "正在搜索";
            stationSearchButton.IsEnabled = false;
            progressBar1.Visibility = Visibility.Visible;

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = false;

            bw.DoWork += new DoWorkEventHandler(SearchLine);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SearchLineComplete);

            string strFrom = staionFromText.Text;
            string strTo = staionToText.Text;
            FromToInfo arg = new FromToInfo() { from=strFrom,to=strTo};
            bw.RunWorkerAsync(arg);
            //return;
            //if (SearchLine(stationFrom, stationTo))
            //{
            //    stationSearchButton.Content = "查找";
            //    stationSearchButton.IsEnabled = true;
            //    //progressBar1.Visibility = Visibility.Collapsed;
            //    //NavigationService.Navigate(new Uri("/ViewResult/LineResult.xaml", UriKind.Relative));
            //}
            //else
            //{
            //    stationSearchButton.Content = "查找";
            //    stationSearchButton.IsEnabled = true;
            //    progressBar1.Visibility = Visibility.Collapsed;
            //}
            
        }

        private void SearchLineComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            stationSearchButton.Content = "查找";
            stationSearchButton.IsEnabled = true;
            progressBar1.Visibility = Visibility.Collapsed;
            if (0==(int)e.Result)
            {
                NavigationService.Navigate(new Uri("/ViewResult/LineResult.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("没找到");
            }
        }

        private void SearchLine(object sender, DoWorkEventArgs e)
        {
            
            FromToInfo arg = (FromToInfo)e.Argument;

            string strFrom = arg.from;
            string strTo = arg.to;
            NingboBus busDB = App.busDB;

            //终点站xid
            var xidInFrom = (from d in busDB.Cnbus where d.Zhan == strFrom select d.Xid).ToList();
            if (xidInFrom.Count == 0)
            {
                e.Result = 1;
                //msg="对不起，未找到该起点站信息";
                return;
            }
            var xidInTo = (from d in busDB.Cnbus where d.Zhan == strTo select d.Xid).ToList();
            if (xidInTo.Count == 0)
            {
                e.Result = 2;
                //msg="对不起，未找到该终点站信息";
                return;
            }
            //包含起点和重点站点路线id
            var idList =
                (from s in busDB.Cnbus
                 where xidInTo.Contains(s.Xid)
                 where xidInFrom.Contains(s.Xid)
                 select s.Xid).ToList();
            if (idList.Count == 0)
            {
                e.Result = 3;
                //msg = "对不起，未找到直达线路";
                return;
            }
            //根据路线id查找路线信息
            var lineList =
                (from s in busDB.Cnbusw
                 where idList.Contains(s.Id)
                 select s).ToList();
            App.lineList = lineList;
            App.stationFrom = strFrom;
            App.stationTo = strTo;
            e.Result = 0;
        }

        //线路查询
        private void buttonFindLine_Click(object sender, RoutedEventArgs e)
        {
            TextChoiceTip.Visibility = Visibility.Collapsed;
            int num;
            try
            {
                num = Convert.ToInt32(textLineNum.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("请输入数字线路名");
                return;
            }
            NingboBus busDB = App.busDB;
            //查找该路线信息
            var lineList =
                (from s in busDB.Cnbusw
                 where s.Shuzi == num
                 select s
                     ).ToList();
            if (lineList.Count == 0)
            {
                MessageBox.Show("找不到该线路。");
                return;
            }
            if (lineList.Count > 1)
            {
                popChoiceLine.ItemsSource = lineList;
                popChoiceLine.Visibility = Visibility.Visible;
                TextChoiceTip.Visibility = Visibility.Visible;
                return;
            }

            App.lineInfo = lineList[0];
            int xid=(int)lineList[0].Id;

            //查找该路线经过的站牌
            var stationList =
            (from s in busDB.Cnbus
             where s.Xid == xid
             where s.Kind == 1
             orderby s.Pm
             select s).ToList();

            App.stationList = stationList;
            NavigationService.Navigate(new Uri("/ViewResult/LineDetail.xaml",UriKind.Relative));
        }

        private void inputFrom(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StationInput/Input.xaml?type=from",UriKind.Relative));
        }

        private void inputTo(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StationInput/Input.xaml?type=to", UriKind.Relative));
        }

        private void choiceItemTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextChoiceTip.Visibility = Visibility.Collapsed;
            popChoiceLine.Visibility = Visibility.Collapsed;

            int xid = Convert.ToInt32(((TextBlock)sender).Tag.ToString());
            NingboBus busDB = App.busDB;
            var lineList =
                (from s in busDB.Cnbusw
                 where s.Id == xid
                 select s
                     ).ToList();
            App.lineInfo = lineList[0];

            //查找该路线经过的站牌
            var stationList =
            (from s in busDB.Cnbus
             where s.Xid == xid
             where s.Kind == 1
             orderby s.Pm
             select s).ToList();
            App.stationList = stationList;
            NavigationService.Navigate(new Uri("/ViewResult/LineDetail.xaml", UriKind.Relative));
        }
    }

    class FromToInfo
    {
        public string from { get; set; }
        public string to { get; set; }
    }
}