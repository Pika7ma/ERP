using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lplfw.UI.Produce
{
    /// <summary>
    /// NewRequsition.xaml 的交互逻辑
    /// </summary>
    public partial class NewRequsition : Window
    {
        public NewRequsition(bool isRequsition)
        {
            InitializeComponent();
            if (isRequsition)
            {
                Title = "新建借料单";
            }
            else
            {
                Title = "新建还料单";
            }
        }

        private void NewItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewRequsitionItem(isNew: true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewRequsitionItem(isNew: false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }
    }
}
