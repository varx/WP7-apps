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
using System.Windows.Navigation;

namespace curriculumSchedule
{
    public partial class MainPage : PhoneApplicationPage
    {

        private Boolean isEditMode=false;
        private Boolean isDeleMode=false;
        //private Uri uriNormalEdit = new Uri("/ICON/appbar.edit.rest.png", UriKind.Relative);
        //private Uri uriNormaltDele = new Uri("/ICON/appbar.delete.rest.png", UriKind.Relative);
        //private Uri uriHilightEdit = new Uri("/ICON/appbar.edit.rest.hilight.png", UriKind.Relative);
        //private Uri uriHilightDele = new Uri("/ICON/appbar.delete.rest.hilight.png", UriKind.Relative);
        //private ApplicationBarIconButton iconEdit;
        //private ApplicationBarIconButton iconDele;
        private Curriculum DB;

        private TextBlock selectTextBlock;

        private string DBConnectionString = "Data Source=isostore:/curriculum.sdf";

        // 构造函数
        public MainPage()
        {
            InitializeComponent();
 //           initAppBar();
            initDB();
            initData();
        }

        private void initData()
        {
            var classlist =
                (from s in DB.Ke
                 select s).ToList();
            foreach (var item in classlist)
            {
                var tb = ContentPanel.FindName(item.Tbname) as TextBlock;
                tb.DataContext=item;
            }
        }
        private void initDB()
        {
            DB = new Curriculum(DBConnectionString);
            if (DB.DatabaseExists() == false)
                {
                    // Create the local database.
                    DB.CreateDatabase();
                    //MessageBox.Show("欢迎使用快捷课表！以下是简短使用说明\n点击编辑按钮，出现编辑“编辑模式”字样时，点击单元格可修改内容，再次点击编辑按钮退出编辑模式。\n点击删除按钮，同理可删除单元格内容。");
                    // Prepopulate the categories.
                    //DB.Categories.InsertOnSubmit(new ToDoCategory { Name = "Home" });
                    // Save categories to the database.
                    //DB.SubmitChanges();
                }
            //DB.DeleteDatabase();
            //DB.CreateDatabase();
            App.DB_Context = DB;
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //TODO:保存数据库
            if (App.AddOrEdit=="add")
            {
                DB.Ke.InsertOnSubmit(App.keItem);
                try
                {
                    DB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据保存失败");
                }
                //selectTextBlock.DataContext = App.keItem;
            }
            else
                if (App.AddOrEdit == "edit")
                {
                    //DB.Ke.DeleteOnSubmit(App.keItem);
                    //DB.Ke.InsertOnSubmit(App.keItem);
                    try
                    {
                        DB.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存失败");
                    }
                    //selectTextBlock.DataContext = App.keItem;
                }
        }

        private void Grid_Tap(object sender, GestureEventArgs e)
        {
            if ((!isEditMode)&&(!isDeleMode))
                return;
            selectTextBlock = (TextBlock)sender;
            if (isEditMode)
            {
                if (selectTextBlock.DataContext != null)
                {
                    App.keItem = (Ke)selectTextBlock.DataContext;
                    NavigationService.Navigate(new Uri("/TMPL/Edit.xaml?edit=1", UriKind.Relative));
                }
                else
                {
                    App.keItem = new Ke();
                    selectTextBlock.DataContext = App.keItem;
                    App.keItem.Tbname = selectTextBlock.Name;
                    NavigationService.Navigate(new Uri("/TMPL/Edit.xaml?add=1", UriKind.Relative));
                }
                return;
            }
            if (isDeleMode)
            {
                if (selectTextBlock.DataContext != null)
                {
                    //selectTextBlock.Opacity = 0.5;
                    MessageBoxResult result = MessageBox.Show("确实要删除该课程吗？","确认",MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        DB.Ke.DeleteOnSubmit(selectTextBlock.DataContext as Ke);
                        selectTextBlock.DataContext = null;
                    }
                    DB.SubmitChanges();
                    //selectTextBlock.Opacity = 1;
                }
            }
        }

        private void editModel(object sender, EventArgs e)
        {
            if (isDeleMode)
            {
                isDeleMode = false;
                Textblock_delemode.Visibility = Visibility.Collapsed;
                //iconDele.IconUri = uriNormaltDele;
            }
            if (isEditMode)
            {
                isEditMode = false;
                Textblock_editmode.Visibility = Visibility.Collapsed;
                //iconEdit.IconUri = uriNormalEdit;
            }
            else
            {
                isEditMode = true;
                Textblock_editmode.Visibility = Visibility.Visible;
               // MessageBox.Show(AppEditIco.Text);
                //iconEdit.IconUri = uriHilightEdit;
            }
        }

        private void deleModel(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                isEditMode = false;
                Textblock_editmode.Visibility = Visibility.Collapsed;
                //iconEdit.IconUri = uriNormalEdit;
            }

            if (isDeleMode)
            {
                isDeleMode = false;
                Textblock_delemode.Visibility = Visibility.Collapsed;
                //iconDele.IconUri = uriNormaltDele;
            }
            else
            {
                isDeleMode = true;
                Textblock_delemode.Visibility = Visibility.Visible;
                //iconDele.IconUri = uriHilightDele;
            }
        }

        private void enterHelp(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/TMPL/Help.xaml",UriKind.Relative));
        }
    }

}