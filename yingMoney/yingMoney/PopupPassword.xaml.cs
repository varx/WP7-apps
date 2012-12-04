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

namespace JasonPopup
{
    public partial class PopupPassword : UserControl
    {

        public bool canceled=false;
        public PopupPassword()
        {
            InitializeComponent();
            TextBoxPassword.Focus();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseMeAsPopup();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            canceled = true;
            this.CloseMeAsPopup();
        }
    }
}
