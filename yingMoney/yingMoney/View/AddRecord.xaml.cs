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

namespace yingMoney.View
{
    public partial class AddRecord : PhoneApplicationPage
    {
        private YingDB APPDB = App.APPDB;
        private List<Main_type> MainTypeList;
        private List<Sub_type> SubTypeList;
        private int mTypeId;
        private int sTypeId;
        private bool hasLoaded = false;
        private Home_info homeInfo;
        public AddRecord()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AddRecord_Loaded);
        }

        void AddRecord_Loaded(object sender, RoutedEventArgs e)
        {
            if (hasLoaded)
            {
                return;
            }
            if (App.APPDB == null)
                APPDB = new YingDB("Data Source = isostore:/yingDB.sdf");
            else
                APPDB = App.APPDB;
            if (App.homeinfo == null)
                homeInfo = APPDB.Home_info.FirstOrDefault();
            else
                homeInfo = App.homeinfo;
            MainTypeList =
                (from s in APPDB.Main_type
                 where s.Delete==0
                 select s
                     ).ToList();
            listPickerMainType.ItemsSource = MainTypeList;
            TextBoxMoneyInt.Focus();
            hasLoaded = true;
        }

        private void setSubList()
        {
            SubTypeList =
                (from s in APPDB.Sub_type
                 where s.Pid == mTypeId
                 where s.Delete==0
                 select s
                     ).ToList();
                listPickerSubType.ItemsSource = SubTypeList;
        }

        private void PreDay(object sender, RoutedEventArgs e)
        {
            datePicker.Value = datePicker.Value.Value.AddDays(-1);
        }

        private void NextDay(object sender, RoutedEventArgs e)
        {
            datePicker.Value = datePicker.Value.Value.AddDays(1);
        }

        private void listPickerMainType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listPickerMainType.ItemsSource == null)
                return;
            Main_type item = listPickerMainType.SelectedItem as Main_type;
            mTypeId = item.Id;
            setSubList();
        }

        private void AddNewRecord(object sender, EventArgs e)
        {
            if (TextBoxMoneyInt.Text.Length == 0)
            {
                MessageBox.Show("请填写金额。");
                return;
            }

            if (TextBoxMoneyPoint.Text.Length == 1)
                TextBoxMoneyPoint.Text = TextBoxMoneyPoint.Text + "0";

            int money;
            try
            {
                money = int.Parse(TextBoxMoneyInt.Text) * 100 + int.Parse(TextBoxMoneyPoint.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请正确填写金额。");
                return;
            }
            DateTime today = DateTime.Today;
            DateTime choiceDate = datePicker.Value.Value;
            Outgoing record = new Outgoing { Money = money, Main_id = mTypeId, Sub_id = sTypeId, Time = choiceDate };
            APPDB.Outgoing.InsertOnSubmit(record);


            //清除过期信息
            if (homeInfo.Date != today)
                homeInfo.Daysum = 0;
            //下一周
            if ((today - homeInfo.Weekstart).Days > 7)
            {
                homeInfo.Weeksum = 0;
                homeInfo.Weekstart = today.AddDays(-(int)today.DayOfWeek);
            }
            if (homeInfo.Date.Month != today.Month && homeInfo.Date.Year != today.Year)
            {
                homeInfo.Mouthsum = 0;
            }
            homeInfo.Date = today;

            if (homeInfo.Date.Year == choiceDate.Year)
            {
                if (homeInfo.Date.Month == choiceDate.Month)
                    homeInfo.Mouthsum += money;
                int weekDays=choiceDate.DayOfYear - homeInfo.Weekstart.DayOfYear;
                if (weekDays < 8&&weekDays>0)
                    homeInfo.Weeksum += money;
                if (choiceDate == homeInfo.Date)
                    homeInfo.Daysum += money;
            }
            
            APPDB.SubmitChanges();
            NavigationService.GoBack();
        }

        private void CancelRecord(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void TextBoxMoneyPoint_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxMoneyPoint.SelectAll();
        }

        private void listPickerSubType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listPickerSubType.ItemsSource == null)
                return;
            Sub_type item = listPickerSubType.SelectedItem as Sub_type;
            sTypeId = item.Id;
        }

        private void TextBoxMoneyInt_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            TextBoxMoneyPoint.Focus();
        }

        private void TextBoxMoneyInt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxMoneyInt.Text.Length > 7)
            {
                TextBoxMoneyInt.Text = TextBoxMoneyInt.Text.Substring(0, 7);
                TextBoxMoneyInt.SelectionStart = 7;
            }
        }

        private void TextBoxMoneyPoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxMoneyPoint.Text.Length > 2)
            {
                TextBoxMoneyPoint.Text = TextBoxMoneyPoint.Text.Substring(0, 2);
                TextBoxMoneyPoint.SelectionStart = 2;
            }
        }

        private void TextBoxMoneyPoint_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

    }
}