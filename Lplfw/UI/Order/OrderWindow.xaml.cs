using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Lplfw.UI.Order
{
    /// <summary>
    /// OrderWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
            cbSearchSales.ItemsSource = SalesFields;
            cbSearchSales.SelectedValue = 0;
            var _thread = new Thread(new ThreadStart(Refresh));
            _thread.Start();

        }

        #region 权限控制

        private void Refresh()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 3 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
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
            btnNewOrder.Visibility = Visibility.Hidden;
            btnFinishOrder.Visibility = Visibility.Hidden;
            btnCancelOrder.Visibility = Visibility.Hidden;
            btnDelayOrder.Visibility = Visibility.Hidden;
            btnSeperateOrder.Visibility = Visibility.Hidden;
        }


        #endregion


        static public List<Utils.KeyValue> SalesFields = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { ID=0, Name="状态" },
            new Utils.KeyValue { ID=1, Name="订单号" },
            new Utils.KeyValue { ID=2, Name="客户" },
            new Utils.KeyValue { ID=3, Name="订货时间" },
            new Utils.KeyValue { ID=4, Name="应付时间" },
            new Utils.KeyValue { ID=5, Name="完成时间" },
            new Utils.KeyValue { ID=6, Name="优先级" },
            new Utils.KeyValue { ID=7, Name="负责人" },
        };

        private void NewSales(object sender, RoutedEventArgs e)
        {
            var _win = new NewOrder();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }
    }
}
