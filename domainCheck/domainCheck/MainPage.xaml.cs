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
using Microsoft.Phone.Shell;

namespace domainCheck
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // 为 ViewModel 项加载数据
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void search(object sender, EventArgs e)
        {
            int pivotPosition = rootPivot.SelectedIndex;
            if (pivotPosition == 0)
                searchDomain();
            else
                if(pivotPosition==1)
                    searchWhois();
            
        }

        private void searchWhois()
        {
            if (textBoxWho.Text == "")
                return;
            App.domain = textBoxWho.Text;
            NavigationService.Navigate(new Uri("/ResultView/Who.xaml", UriKind.Relative));
        }

        private void searchDomain()
        {
            if (textBoxDomain.Text == "")
                return;
            App.domainExt.Clear();
            foreach (CheckBox item in PanelSuffix1.Children)
            {
                if (item.IsChecked == true)
                    App.domainExt.Add(item.Content.ToString());
            }
            foreach (CheckBox item in PanelSuffix2.Children)
            {
                if (item.IsChecked == true)
                    App.domainExt.Add(item.Content.ToString());
            }
            foreach (CheckBox item in PanelSuffix3.Children)
            {
                if (item.IsChecked == true)
                    App.domainExt.Add(item.Content.ToString());
            }
            App.domain = textBoxDomain.Text;
            NavigationService.Navigate(new Uri("/ResultView/Domain.xaml", UriKind.Relative));
        }
    }
}