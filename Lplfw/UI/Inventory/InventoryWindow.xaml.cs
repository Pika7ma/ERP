using System;
using System.Linq;
using System.Threading;
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
            var _thread = new Thread(new ThreadStart(Refresh));
            _thread.Start();
        }


        #region 权限控制

        private void Refresh()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 2 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
                if (_temp.Mode == "只读")
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        OnlyRead();
                    });
                }
            }
        }
        /// <summary>
        /// 只读
        /// </summary>
        private void OnlyRead()
        {
            btnNewIn.Visibility = Visibility.Hidden;
            btnNewOut.Visibility = Visibility.Hidden;
            btnNewStorage.Visibility = Visibility.Hidden;
            btnEditStorage.Visibility = Visibility.Hidden;
            btnDelStorage.Visibility = Visibility.Hidden;
        }


        #endregion


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
