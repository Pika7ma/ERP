using System.Windows;

namespace Lplfw.UI.Inventory
{
    /// <summary>
    /// InventoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InventoryWindow : Window
    {
        public InventoryWindow()
        {
            InitializeComponent();
        }

        private void NewStorage(object sender, RoutedEventArgs e)
        {
            var _win = new NewStorehouse(isNew: true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void EditStorage(object sender, RoutedEventArgs e)
        {
            var _win = new NewStorehouse(isNew: false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void NewStockIn(object sender, RoutedEventArgs e)
        {
            var _win = new NewStockInOut(isStockIn: true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void NewStockOut(object sender, RoutedEventArgs e)
        {
            var _win = new NewStockInOut(isStockIn: false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }
    }
}
