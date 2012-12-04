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
using ningboBus.Model;

namespace ningboBus.StationInput
{
    public partial class Input : PhoneApplicationPage
    {
        private string type;
        public Input()
        {
            InitializeComponent();
            if (App.allStationList == null)
                App.allStationList = new StationList().stations;
            autoCompleteBox1.ItemsSource = App.allStationList;
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            autoCompleteBox1.Focus();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            type = NavigationContext.QueryString["type"];
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if(App.allStationList.Contains(autoCompleteBox1.Text))
            {
                if (type == "from")
                    App.stationFrom = autoCompleteBox1.Text;
                else
                    App.stationTo = autoCompleteBox1.Text;
                NavigationService.Navigate(new Uri("/MainPage.xaml",UriKind.Relative));
            }
            else
                MessageBox.Show("对不起，未找到该站点");
        }
    }
}