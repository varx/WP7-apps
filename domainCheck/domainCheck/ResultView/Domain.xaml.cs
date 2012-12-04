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
using System.ComponentModel;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace domainCheck.ResultView
{
    public partial class Domain : PhoneApplicationPage
    {
        private string domain;
        private ObservableCollection<domainItem> list =new ObservableCollection<domainItem>();
        
        public Domain()
        {
            InitializeComponent();
            domain = App.domain;
            PageTitle.Text = domain;
            //list = new ObservableCollection<domainItem>();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar1.Visibility = Visibility.Visible;
            foreach (string ext in App.domainExt)
            {
                checkAvailable(ext);
                //list.Add(new domainItem() { ext = ext, available = true });
            }
            checkList.ItemsSource = list;
        }
        private void checkAvailable(string ext)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadXmlCompleted);
            wc.DownloadStringAsync(new Uri("http://panda.www.net.cn/cgi-bin/check.cgi?area_domain="+domain+ext));
        }

        private void DownloadXmlCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            progressBar1.Visibility = Visibility.Collapsed;
            try
            {
                //MessageBox.Show(e.Result);
                XElement doc = XElement.Parse(e.Result);
                string domain = doc.Element("key").Value;
                string original = doc.Element("original").Value;
                bool availavle = original.StartsWith("210") ? true : false;
                list.Add(new domainItem { ext = domain, available = availavle });
            }
            catch (WebException we)
            {
                MessageBox.Show("网络连接异常");
            }
            catch (Exception ex)
            {
                MessageBox.Show("暂时无法获取信息");
            }
        }
    }
    public class domainItem
    {
        public string ext
        {
            get;
            set;
        }
        public Nullable<Boolean> available
        {
            get;
            set;
        }

    }
}