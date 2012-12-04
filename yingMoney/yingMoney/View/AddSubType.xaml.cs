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
using System.Collections.ObjectModel;

namespace yingMoney.View
{
    public partial class AddSubType : PhoneApplicationPage
    {
        private YingDB APPDB = App.APPDB;
        private ObservableCollection<Sub_type> SubTypeList;
        private int pid=App.MainTypeId;

        public AddSubType()
        {
            InitializeComponent();
            //this.Loaded += new RoutedEventHandler(AddSubType_Loaded);
            initData();
        }

        private void initData()
        {
            PageTitle.Text = App.MainTypeName;
            SubTypeList = new ObservableCollection<Sub_type>(
                (from s in APPDB.Sub_type
                 where s.Pid == pid
                 where s.Delete==0
                 select s
                     ).ToList()
                );
            ListBoxSubType.ItemsSource = SubTypeList;
        }

        private void AddSubTypeClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxSubType.Text.Length > 0)
            {
                string name = TextBoxSubType.Text;
                Sub_type SubTypeItem = new Sub_type{Name = name,Pid=pid};
                APPDB.Sub_type.InsertOnSubmit(SubTypeItem);
                try
                {
                    APPDB.SubmitChanges();
                    SubTypeList.Add(SubTypeItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库错误，请尝试重新安装应用。");
                }
                TextBoxSubType.Text = "";
                this.Focus();
            }
        }

        private void DeleSubType(object sender, RoutedEventArgs e)
        {
            Sub_type subTypeItem = (Sub_type)(sender as Button).Tag;
            if (SubTypeList.Count == 1)
            {
                subTypeItem.Name = "默认" + App.MainTypeName;
            }
            else
            {
                //APPDB.Sub_type.DeleteOnSubmit(subTypeItem);
                subTypeItem.Delete = 1;
                SubTypeList.Remove(subTypeItem);
            }
            APPDB.SubmitChanges();
            this.Focus();
        }
    }
}