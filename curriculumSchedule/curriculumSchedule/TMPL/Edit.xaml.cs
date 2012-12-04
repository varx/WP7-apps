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

namespace curriculumSchedule.TMPL
{
    public partial class Edit : PhoneApplicationPage
    {
        private Boolean editMode;
        public Edit()
        {
            InitializeComponent();
        }

        private void init(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("edit"))
            {
                ContentPanel.DataContext = App.keItem;
                editMode = true;
            }
            else
                editMode = false;
        }

        private void AppBarOk_Click(object sender, EventArgs e)
        {
            if (TextBoxName.Text.Length ==0)
            {
                MessageBox.Show("请填写课程名称");
                return;
            }

            App.keItem.Name = TextBoxName.Text;
            if (TextBoxTeacher.Text.Length > 0)
                App.keItem.Teacher = TextBoxTeacher.Text;
            if (TextBoxRoom.Text.Length > 0)
                App.keItem.Room = TextBoxRoom.Text;
            if (TextBoxComment.Text.Length > 0)
                App.keItem.Comment = TextBoxComment.Text;
            if (editMode)
            {
                App.AddOrEdit = "edit";
                NavigationService.GoBack();
            }
            else
            {
                App.AddOrEdit = "add";
                NavigationService.GoBack();
                //NavigationService.Navigate(new Uri("/MainPage.xaml?add=1", UriKind.Relative));
            }
            //Ke ToInsertKe=new Ke{Name=name};
        }

        private void AppBarCancel_Click(object sender, EventArgs e)
        {
            App.AddOrEdit = null;
            NavigationService.GoBack();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.AddOrEdit = null;
        }

    }
}