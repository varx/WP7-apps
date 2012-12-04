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
using System.Windows.Navigation;

namespace yingMoney
{
    public partial class Login : PhoneApplicationPage
    {
        private string psw = (string)App.isoSetting["password"];
        private int count=0;
        public Login()
        {
            InitializeComponent();
            //textBoxPassword.Focus();
            this.Loaded += new RoutedEventHandler(Login_Loaded);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if(NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();
            base.OnNavigatedFrom(e);
        }

        void Login_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.isoSetting.Contains("locktime"))
            {
                DateTime now = DateTime.Now;
                DateTime locktime = (DateTime)App.isoSetting["locktime"];
                int min = (now - locktime).Minutes;
                if (min < 10)
                {
                    textBoxPassword.IsEnabled = false;
                    ButtonOK.IsEnabled = false;
                    MessageBox.Show("应用锁定，请"+(10-min)+"分钟后重试");
                }
                else
                {
                    App.isoSetting.Remove("locktime");
                    textBoxPassword.Focus();
                }
            }
            else
            {
                textBoxPassword.Focus();
            }
            
        }


        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (psw == textBoxPassword.Text)
            {
                NavigationService.Navigate(new Uri("MainPage", UriKind.Relative));
            }
            else
            {
                count++;
                if (count == 5)
                {
                    textBoxPassword.IsEnabled = false;
                    ButtonOK.IsEnabled = false;
                    App.isoSetting.Add("locktime",DateTime.Now);
                    MessageBox.Show("密码5次错误，请稍候重试。");
                }
                else
                {
                    MessageBox.Show("密码错误，您还有"+(5-count).ToString()+"次机会");
                }
                textBoxPassword.Focus();
            }
        }
    }
}