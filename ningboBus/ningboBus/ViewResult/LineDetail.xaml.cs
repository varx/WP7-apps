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
    public partial class LindeDetail : PhoneApplicationPage
    {
        public LindeDetail()
        {
            InitializeComponent();
            if (listBoxLineDetail.ItemsSource == null)
                listBoxLineDetail.ItemsSource = App.stationList;
            PageTitle.Text = App.lineInfo.Busw;
        }
    }
}