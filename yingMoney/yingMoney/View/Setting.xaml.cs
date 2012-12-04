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
using Microsoft.Phone.Shell;
using JasonPopup;

namespace yingMoney.View
{
    public partial class Setting : PhoneApplicationPage
    {
        private ObservableCollection<Main_type> MainTypeList;
        private YingDB APPDB = App.APPDB;
        private bool HasTile=false;
        private bool HasPsw = false;
        public string password;
        public Setting()
        {
            InitializeComponent();
            initUI();
        }

        private void initUI()
        {
            if (App.isoSetting.Contains("password"))
            {
                HasPsw = true;
                toggleSwitchPassword.IsChecked = true;
            }
            ShellTile NowTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("TileID=2"));
            if (NowTile != null)
            {
                HasTile = true;
                toggleSwitchTile.IsChecked = true;
            }
            MainTypeList = new ObservableCollection<Main_type>(
                (from s in APPDB.Main_type
                 where s.Delete==0
                 select s
                     ).ToList()
                );
            listBoxMainType.ItemsSource = MainTypeList;
        }

      
        public void setPassword(string psw)
        {
            if (App.isoSetting.Contains("password"))
            {
                App.isoSetting["password"] = psw;
            }
            else
            {
                App.isoSetting.Add("password",psw);
            }
        }

        public void unsetPassword()
        {
            toggleSwitchPassword.IsChecked = false;
        }
        private void saveSetting()
        {
            App.isoSetting.Save();
        }
        private void toggleSwitchPassword_Checked(object sender, RoutedEventArgs e)
        {
            if (HasPsw)
                return;
            HasPsw = true;
            PopupCotainer pc = new PopupCotainer(this);
            PopupPassword popup = new PopupPassword();
            pc.Show(popup);
            popup.TextBoxPassword.Focus();
            //App.isoSetting["password"] = "123";
            //saveSetting();
            //MessageBox.Show("checked");
        }

        private void toggleSwitchPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            if (HasPsw == false)
                return;
            HasPsw = false;
            App.isoSetting.Remove("password");
            saveSetting();
            //MessageBox.Show("unchecked");
        }

        private void AddMainType(object sender, RoutedEventArgs e)
        {
            if (TextBoxMainType.Text.Length > 0)
            {
                string name = TextBoxMainType.Text;
                Main_type MainItem = new Main_type { Name=name};
                Sub_type defaultSubItem = new Sub_type { Name = name };
                App.APPDB.Main_type.InsertOnSubmit(MainItem);
                try
                {
                    APPDB.SubmitChanges();
                    defaultSubItem.Pid = MainItem.Id;
                    APPDB.Sub_type.InsertOnSubmit(defaultSubItem);
                    APPDB.SubmitChanges();
                    MainTypeList.Add(MainItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据保存失败");
                }
            }
            this.Focus();
        }

        private void MainTypeItemTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            App.MainTypeId = (int)(tb.Tag);
            App.MainTypeName = tb.Text;
            NavigationService.Navigate(new Uri("/View/AddSubType.xaml",UriKind.Relative));
        }

        private void toggleSwitchTile_Checked(object sender, RoutedEventArgs e)
        {

            if (HasTile)
                return;
            StandardTileData TileData = new StandardTileData
            {
                BackgroundImage = new Uri("Background.png", UriKind.Relative),
                Title = "点击记账",
                BackTitle = "颖记账",
                //BackContent = "点击记账",
                BackBackgroundImage = new Uri("/Images/tile.png", UriKind.Relative)
            };
            ShellTile.Create(new Uri("/View/AddRecord.xaml?TileID=2", UriKind.Relative), TileData);
        }

        private void toggleSwitchTile_Unchecked(object sender, RoutedEventArgs e)
        {
            ShellTile NowTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("TileID=2"));
            if (NowTile != null)
            {
                NowTile.Delete();
            }
            HasTile = false;
        }

        private void DeleMainType(object sender, RoutedEventArgs e)
        {
            if (MainTypeList.Count == 1)
            {
                MessageBox.Show("请至少保留一个分类。");
                return;
            }
            Button BT = sender as Button;
            Main_type ItemToDele = (Main_type)BT.Tag;
            ItemToDele.Delete = 1;
            //App.APPDB.Main_type.DeleteOnSubmit(ItemToDele);
            var subTypeToDele = from s in APPDB.Sub_type
                                where s.Pid == ItemToDele.Id
                                select s;
            foreach (var i in subTypeToDele)
                i.Delete = 1;
            //APPDB.Sub_type.DeleteAllOnSubmit(subTypeToDele);
            try
            {
                APPDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                return;
            }
            MainTypeList.Remove(ItemToDele);
            TextBoxMainType.Text = "";
            this.Focus();
        }
    }
}