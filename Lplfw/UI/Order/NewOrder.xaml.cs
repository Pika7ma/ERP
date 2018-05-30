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

namespace Lplfw.UI.Order
{
    /// <summary>
    /// NewOrder.xaml 的交互逻辑
    /// </summary>
    public partial class NewOrder : Window
    {
        public NewOrder()
        {
            InitializeComponent();
        }

        private void NewSalesItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewOrderItem(isNew:true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void EditSalesItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewOrderItem(isNew: false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }
    }
}
