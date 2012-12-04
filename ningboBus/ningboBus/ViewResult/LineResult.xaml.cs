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

namespace ningboBus.ViewResult
{
    public partial class LineResult : PhoneApplicationPage
    {
        public LineResult()
        {
            InitializeComponent();
            if(listBox1.ItemsSource==null)
                listBox1.ItemsSource = App.lineList;
            PageTitle.Text = App.stationFrom+"-"+App.stationTo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button item = (Button)sender;
            int lineNum = Convert.ToInt32(item.Tag.ToString());
            NingboBus busDB = App.busDB;
            var stationList =
            (from s in busDB.Cnbus
             where s.Xid == lineNum
             where s.Kind == 1
             orderby s.Pm
             select s).ToList();
            var lineInfo =
                (from s in busDB.Cnbusw
                 where s.Id == lineNum
                 select s
                ).ToList()[0];
            App.stationList = stationList;
            App.lineInfo = lineInfo;
            NavigationService.Navigate(new Uri("/ViewResult/LineDetail.xaml", UriKind.Relative));
        }
    }
}