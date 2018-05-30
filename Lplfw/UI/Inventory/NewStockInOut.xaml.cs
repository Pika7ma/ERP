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

namespace Lplfw.UI.Inventory
{
    /// <summary>
    /// NewStockInOut.xaml 的交互逻辑
    /// </summary>
    public partial class NewStockInOut : Window
    {
        public NewStockInOut(bool isStockIn)
        {
            InitializeComponent();
            if (isStockIn)
            {
                Title = "新建入库单据";
            }
            else
            {
                Title = "新建出库单据";
            }
        }

        private void NewMaterialItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewStockItem(isNew: true, isMaterial: true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true) {

            }
        }

        private void EditMaterialItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewStockItem(isNew: false, isMaterial: true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void NewProductItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewStockItem(isNew: true, isMaterial: false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void EditProductItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewStockItem(isNew: false, isMaterial: false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }
    }
}
