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
using System.Runtime.Serialization;
using System.IO;
using System.Text;

namespace domainCheck.ResultView
{
    public partial class who : PhoneApplicationPage
    {

        //http://whois.hichina.com/whois/api_whois?host=varx.info
        public who()
        {
            InitializeComponent();
            PageTitle.Text = App.domain;
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar1.Visibility = Visibility.Visible;
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadJsonCompleted);
            wc.DownloadStringAsync(new Uri("http://whois.hichina.com/whois/api_whois?host=" + App.domain));
        }

        private void DownloadJsonCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            progressBar1.Visibility = Visibility.Collapsed;
            try
            {
                whoisInfo Info;
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(e.Result));
                //MessageBox.Show(e.Result);
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(whoisInfo));
                Info = serializer.ReadObject(ms) as whoisInfo;
                ms.Close();
                //MessageBox.Show(Info.name);
                base_info.DataContext = Info;
                total_info.Text = Info.total_infor;
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
    [DataContract]
    public class whoisInfo
    {
        [DataMember]
        public string exp_date{get;set;}

        [DataMember]
        public string name_server { get; set; }

        [DataMember]
        public string Registrant_Name { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public string registrar { get; set; }

        [DataMember]
        public string reg_date { get; set; }

        [DataMember]
        public string total_infor { get; set; }
    }

}