using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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
            new Thread(new ThreadStart(CheckPrivilege)).Start();
            RefreshDgSales();
        }

        private void RefreshDgSales()
        {
            new Thread(new ThreadStart(RefreshDgSalesThread)).Start();
        }

        private void RefreshDgSalesThread()
        {
            var _list = SalesView.GetAll();
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgSales.ItemsSource = _list;
            });
        }

        private void ShowAllSales(object sender, RoutedEventArgs e)
        {
            RefreshDgSales();
        }

        private void NewSales(object sender, RoutedEventArgs e)
        {
            var _win = new NewOrder();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshDgSales();
            }
        }

        private void FinishSales(object sender, RoutedEventArgs e)
        {
            var _item = dgSales.SelectedItem as SalesView;
            if (_item == null) return;
            using (var _db = new ModelContainer())
            {
                var _sales = _db.SalesSet.FirstOrDefault(i => i.Id == _item.Id);
                if (_sales == null) return;

                if (SalesStatus.CanFinish(_sales.Status))
                {
                    _sales.Status = SalesStatus.Finished;
                    _db.SaveChanges();
                    _item.Status = SalesStatus.Finished;
                    dgSales.Items.Refresh();
                }
            }
        }

        private void CancelSales(object sender, RoutedEventArgs e)
        {
            var _item = dgSales.SelectedItem as SalesView;
            if (_item == null) return;
            using (var _db = new ModelContainer())
            {
                var _sales = _db.SalesSet.FirstOrDefault(i => i.Id == _item.Id);
                if (_sales == null) return;

                if (SalesStatus.CanCancel(_sales.Status))
                {
                    _sales.Status = SalesStatus.Canceled;
                    _db.SaveChanges();
                    _item.Status = SalesStatus.Canceled;
                    dgSales.Items.Refresh();
                }
            }
        }

        private void DelaySales(object sender, RoutedEventArgs e)
        {
            var _item = dgSales.SelectedItem as SalesView;
            if (_item == null) return;
            using (var _db = new ModelContainer())
            {
                var _sales = _db.SalesSet.FirstOrDefault(i => i.Id == _item.Id);
                if (_sales == null) return;

                if (SalesStatus.CanDelay(_sales.Status))
                {
                    _sales.Status = SalesStatus.Delayed;
                    _db.SaveChanges();
                    _item.Status = SalesStatus.Delayed;
                    dgSales.Items.Refresh();
                }
            }
        }

        private void SelectSales(object sender, SelectionChangedEventArgs e)
        {
            var _item = dgSales.SelectedItem as SalesView;
            if (_item == null) return;
            dgSalesItem.ItemsSource = SalesItemView.GetById(_item.Id);
        }

        private void GenerateRequsition(object sender, RoutedEventArgs e)
        {
            var _item = dgSales.SelectedItem as SalesView;
            if (_item == null) return;
            if (_item.RequsitionStatus == true)
            {
                MessageBox.Show("该订单已生成领料单", null, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var _win = new GenerateRequsition(_item.Id);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    _item.RequsitionStatus = true;
                    MessageBox.Show("物料分解成功");
                }
                else
                {
                    MessageBox.Show("物料分解失败");
                }
            }
        }

        #region 权限控制
        private void CheckPrivilege()
        {
            using (var _db = new ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 3 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
                if (_temp.Mode == 1)
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
            btnNewSales.Visibility = Visibility.Hidden;
            btnFinishSales.Visibility = Visibility.Hidden;
            btnCancelSales.Visibility = Visibility.Hidden;
            btnDelaySales.Visibility = Visibility.Hidden;
            btnGenerateRequsition.Visibility = Visibility.Hidden;
        }

        #endregion


    }

    static public class SalesStatus
    {
        static public string Processing
        {
            get { return "处理中"; }
        }

        static public string Delayed
        {
            get { return "延期中"; }
        }

        static public string Canceled
        {
            get { return "已取消"; }
        }

        static public string Finished
        {
            get { return "已完成"; }
        }

        static public bool CanFinish(string status)
        {
            if (status == Processing) return true;
            if (status == Delayed) return true;
            return false;
        }

        static public bool CanCancel(string status)
        {
            if (status == Processing) return true;
            if (status == Delayed) return true;
            return false;
        }

        static public bool CanDelay(string status)
        {
            if (status == Processing) return true;
            return false;
        }
    }
}
