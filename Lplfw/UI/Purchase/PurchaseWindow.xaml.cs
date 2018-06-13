using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Lplfw.UI.Purchase
{
    /// <summary>
    /// PurchaseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PurchaseWindow : Window
    {
        public PurchaseWindow()
        {
            InitializeComponent();
            new Thread(new ThreadStart(CheckPrivilege)).Start();
        }

        private void TabRouter(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (tabLack.IsSelected)
                {
                    RefreshTvLack();
                    ShowAllLacks(null, null);
                }
                else if (tabPrice.IsSelected)
                {
                    RefreshTvPrice();
                    ShowAllPrices(null, null);

                }
                else if (tabSupplier.IsSelected)
                {
                    RefreshDgSupplier();
                }
                else if (tabPurchase.IsSelected)
                {
                    RefreshDgPurchase();
                }
            }
        }

        #region 缺料浏览
        private void RefreshTvLack()
        {
            tvLack.Items.Clear();
            MaterialClass.SetTreeFromRoot(ref tvLack);
        }

        private void SelectLackClass(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvLack.SelectedItem == null) return;
            var _id = (int)Utils.GetTreeViewSelectedValue(ref tvLack);
            new Thread(new ParameterizedThreadStart(SelectLackClassThread)).Start(_id);
        }
        private void SelectLackClassThread(object id)
        {
            var _id = (int)id;
            var _lacks = MaterialLackView.GetByClassId(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgLack.ItemsSource = _lacks;
            });
        }

        private void ShowAllLacks(object sender, RoutedEventArgs e)
        {
            new Thread(new ParameterizedThreadStart(SelectLackClassThread)).Start(0);
        }

        //TODO: 将缺料生成EXCEL导出
        #endregion

        #region 报价管理
        private void RefreshTvPrice()
        {
            tvPrice.Items.Clear();
            MaterialClass.SetTreeFromRoot(ref tvPrice);
        }

        private void SelectPriceClass(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvPrice.SelectedItem == null) return;
            var _id = (int)Utils.GetTreeViewSelectedValue(ref tvPrice);
            new Thread(new ParameterizedThreadStart(SelectPriceClassThread)).Start(_id);
        }
        private void SelectPriceClassThread(object id)
        {
            var _id = id as int?;
            var _prices = MaterialPriceView.GetByClassId(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgPrice.ItemsSource = _prices;
            });
        }

        private void ShowAllPrices(object sender, RoutedEventArgs e)
        {
            new Thread(new ParameterizedThreadStart(SelectPriceClassThread)).Start(0);
        }

        private void RefreshDgPrice()
        {
            var _id = Utils.GetTreeViewSelectedValue(ref tvPrice);
            new Thread(new ParameterizedThreadStart(SelectPriceClassThread)).Start(_id);
        }

        private void NewPrice(object sender, RoutedEventArgs e)
        {
            var _classId = Utils.GetTreeViewSelectedValue(ref tvPrice);
            var _win = new NewPrice(_classId);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshDgPrice();
            }

        }

        private void EditPrice(object sender, RoutedEventArgs e)
        {
            if (dgPrice.SelectedItem is MaterialPriceView _material)
            {
                var _win = new NewPrice(_material);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    RefreshDgPrice();
                }
            }
        }

        private void DeletePrice(object sender, RoutedEventArgs e)
        {
            if (dgPrice.SelectedItem is MaterialPriceView _material)
            {
                var _confirm = MessageBox.Show("确定要删除报价吗?", null, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (_confirm != MessageBoxResult.OK) return;
                try
                {
                    using (var _db = new ModelContainer())
                    {
                        var _item = _db.MaterialPriceSet.FirstOrDefault(i => i.MaterialId == _material.MaterialId && i.SupplierId == _material.SupplierId);
                        _db.MaterialPriceSet.Remove(_item);
                        _db.SaveChanges();
                    }
                    RefreshDgPrice();
                    MessageBox.Show("删除成功", null, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("删除失败", null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region 供应商管理
        private void RefreshDgSupplier()
        {
            ShowAllSuppliers(null, null);
        }

        private void ShowAllSuppliers(object sender, RoutedEventArgs e)
        {
            new Thread(new ThreadStart(ShowAllSuppliersThread)).Start();
        }
        private void ShowAllSuppliersThread()
        {
            using (var _db = new ModelContainer())
            {
                var _suppliers = _db.SupplierSet.ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgSupplier.ItemsSource = _suppliers;
                });
            }
        }

        private void SetDgSupplierPrice()
        {
            if (dgSupplier.SelectedItem is Supplier _supplier)
            {
                new Thread(new ParameterizedThreadStart(SetDgSupplierPriceThread)).Start(_supplier.Id);
            }
            else
            {
                dgSupplierPrice.ItemsSource = null;
            }
        }
        private void SetDgSupplierPriceThread(object id)
        {
            var _id = (int)id;
            var _prices = MaterialPriceView.GetByClassId(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgSupplierPrice.ItemsSource = _prices;
            });
        }

        private void NewSupplier(object sender, RoutedEventArgs e)
        {
            var _win = new NewSupplier();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshDgSupplier();
            }
        }

        private void EditSupplier(object sender, RoutedEventArgs e)
        {
            if (dgSupplier.SelectedItem is Supplier _supplier)
            {
                var _win = new NewSupplier(_supplier);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    RefreshDgSupplier();
                }
            }
        }

        private void DeleteSupplier(object sender, RoutedEventArgs e)
        {
            if (dgSupplier.SelectedItem is Supplier _supplier)
            {
                var _confirm = MessageBox.Show("确定要删除供货商吗?", null, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (_confirm != MessageBoxResult.OK) return;
                try
                {
                    using (var _db = new ModelContainer())
                    {
                        var _item = _db.SupplierSet.FirstOrDefault(i => i.Id == _supplier.Id);
                        _db.SupplierSet.Remove(_item);
                        _db.SaveChanges();
                    }
                    RefreshDgSupplier();
                    MessageBox.Show("删除成功", null, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("删除失败", null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SelectSupplier(object sender, SelectionChangedEventArgs e)
        {
            SetDgSupplierPrice();
        }

        #endregion

        #region 采购单管理
        private void RefreshDgPurchase()
        {
            new Thread(new ThreadStart(RefreshDgPurchaseThread)).Start();
        }

        private void RefreshDgPurchaseThread()
        {
            var _list = PurchaseView.GetAll();
            Dispatcher.BeginInvoke((Action) delegate ()
            {
                dgPurchase.ItemsSource = _list;
            });
        }

        private void ShowAllPurchase(object sender, RoutedEventArgs e)
        {
            RefreshDgPurchase();
        }

        private void NewPurchase(object sender, RoutedEventArgs e)
        {
            var _win = new NewPurchase();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshDgPurchase();
            }
        }

        private void CancelPurchase(object sender, RoutedEventArgs e)
        {
            var _purchase = dgPurchase.SelectedItem as PurchaseView;
            if (_purchase == null) return;
            if (_purchase.Status == "处理中") {
                _purchase.Status = "已取消";
                dgPurchase.Items.Refresh();
                using (var _db = new ModelContainer())
                {
                    var _item = _db.PurchaseSet.FirstOrDefault(i => i.Id == _purchase.Id);
                    if (_item == null) return;
                    _item.Status = "已取消";
                    _db.SaveChanges();
                }
            }
        }

        private void FinishPurchase(object sender, RoutedEventArgs e)
        {
            var _purchase = dgPurchase.SelectedItem as PurchaseView;
            if (_purchase == null) return;
            if (_purchase.Status == "处理中")
            {
                _purchase.Status = "已完成";
                var _time = DateTime.Now;
                _purchase.FinishedAt = _time;
                dgPurchase.Items.Refresh();
                using (var _db = new ModelContainer())
                {
                    var _item = _db.PurchaseSet.FirstOrDefault(i => i.Id == _purchase.Id);
                    if (_item == null) return;
                    _item.Status = "已完成";
                    _item.FinishedAt = _time;
                    _db.SaveChanges();
                }
            }
        }

        private void NewQuanlity(object sender, RoutedEventArgs e)
        {

        }

        private void ShowQuality(object sender, RoutedEventArgs e)
        {

        }

        private void SelectPurchase(object sender, SelectionChangedEventArgs e)
        {
            var _purchase = dgPurchase.SelectedItem as PurchaseView;
            if (_purchase == null)
            {
                dgPurchaseItem.ItemsSource = null;
                return;
            }
            new Thread(new ParameterizedThreadStart(SelectPurchaseThread)).Start(_purchase.Id);
        }

        private void SelectPurchaseThread(object id)
        {
            var _id = (int)id;
            var _list = PurchaseItemView.GetByPurchaseId(_id);
            Dispatcher.BeginInvoke((Action) delegate ()
            {
                dgPurchaseItem.ItemsSource = _list;
            });
        }
        #endregion

        #region 权限控制
        private void CheckPrivilege()
        {
            using (var _db = new ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 5 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
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
            btnNewPrice.Visibility = Visibility.Hidden;
            btnEditPrice.Visibility = Visibility.Hidden;
            btnDeletePrice.Visibility = Visibility.Hidden;
            btnNewSupplier.Visibility = Visibility.Hidden;
            btnEditSupplier.Visibility = Visibility.Hidden;
            btnDeleteSupplier.Visibility = Visibility.Hidden;
            btnNewOrder.Visibility = Visibility.Hidden;
            btnCancelOrder.Visibility = Visibility.Hidden;
            btnFinishOrder.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
