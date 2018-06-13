using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Lplfw.DAL;
using System.Linq;
using System;
using System.Data;
using System.Threading;
using Lplfw.BLL;

namespace Lplfw.UI.Produce
{
    /// <summary>
    /// ProduceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProduceWindow : Window
    {
        public ProduceWindow()
        {
            InitializeComponent();
            new Thread(new ThreadStart(CheckPrivilege)).Start();
        }

        private void TabRouter(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (tabConfirm.IsSelected)
                {
                    RefreshDgToDoRequsition();
                }
                else if (tabRequisition.IsSelected)
                {
                    RefreshDgRequisition();
                }
                else if (tabProduce.IsSelected)
                {
                    RefreshDgProduction();
                }
            }
        }

        #region 物料确认
        private void RefreshDgToDoRequsition()
        {
            new Thread(new ThreadStart(RefreshDgToDoRequsitionThread)).Start();
        }

        private void RefreshDgToDoRequsitionThread()
        {
            var _list = RequisitionView.GetUnfinished();
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgToDoRequsitions.ItemsSource = _list;
            });
        }

        private void SelectToDoRequisition(object sender, SelectionChangedEventArgs e)
        {
            var _item = dgToDoRequsitions.SelectedItem as RequisitionView;
            if (_item == null)
            {
                dgToDoItems.ItemsSource = null;
                dgToDoStocks.ItemsSource = null;
                return;
            }
            SetToDoStock(_item.Id);
        }

        private void SetToDoStock(int index)
        {
            var _stocks = RequisitionStockProcedure.GetById(index);
            var _items = RequisitionItemView.GetById(index);
            dgToDoItems.ItemsSource = _items;
            for (var _i = 0; _i < _items.Count; _i++)
            {
                var _stock = _stocks.FirstOrDefault(i => i.MaterialId == _items[_i].MaterialId);
                if (_stock != null)
                {
                    _stock.Quantity -= _items[_i].Quantity;
                }
            }
            dgToDoStocks.ItemsSource = _stocks;
        }

        private bool CheckStock()
        {
            var _list = dgToDoStocks.ItemsSource as List<RequisitionStockProcedure>;
            for (var _i = 0; _i < _list.Count; _i++)
            {
                if (_list[_i].Quantity < 0) return false;
            }
            return true;
        }

        private void SubmitRequsition(object sender, RoutedEventArgs e)
        {
            var _requisition = dgToDoRequsitions.SelectedItem as RequisitionView;
            if (_requisition == null) return;
            if (_requisition.Status != "处理中") return;
            if (CheckStock())
            {
                try
                {
                    using (var _db = new ModelContainer())
                    {
                        var _item = _db.RequisitionSet.FirstOrDefault(i => i.Id == _requisition.Id);
                        _item.Status = "领料中";
                        VirtualUse.AddFromRequisition(_requisition.Id);
                        _db.SaveChanges();
                    }
                    RefreshDgToDoRequsition();
                    MessageBox.Show("已请求领料!");
                }
                catch (Exception)
                {
                    MessageBox.Show("请求领料失败", null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("库存不足!无法领料", null, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ConfirmRequsition(object sender, RoutedEventArgs e) {
            var _requisition = dgToDoRequsitions.SelectedItem as RequisitionView;
            if (_requisition == null) return;
            if (_requisition.Status != "领料中") return;
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _item = _db.RequisitionSet.FirstOrDefault(i => i.Id == _requisition.Id);
                    _item.Status = "已完成";
                    _item.FinishedAt = DateTime.Now;
                    _db.SaveChanges();

                    VirtualUse.RemoveFromRequisition(_requisition.Id);
                }
                RefreshDgToDoRequsition();
                MessageBox.Show("已完成领料!");
            }
            catch (Exception)
            {
                MessageBox.Show("完成领料失败", null, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //TODO: 将物料单导出
        private void ExportRequsition(object sender, RoutedEventArgs e)
        {
            var _requisition = dgToDoRequsitions.SelectedItem as RequisitionView;
            if (_requisition == null) return;
            if (_requisition.Status != "领料中") return;
            var writer = new ExcelWriter();
            var _rtn = writer.WriteRequisition(_requisition, dgToDoItems.ItemsSource as List<RequisitionItemView>);
            if (_rtn == true)
            {
                MessageBox.Show("导出领料单成功");
            }
            else {
                MessageBox.Show("导出领料单失败");
            }
        }
        #endregion

        #region 物料单管理
        private void RefreshDgRequisition()
        {
            new Thread(new ThreadStart(RefreshDgRequisitionThread)).Start();
        }

        private void RefreshDgRequisitionThread()
        {
            var _list = RequisitionView.GetAll();
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgRequisition.ItemsSource = _list;
            });
        }

        private void ShowAllRequistion(object sender, RoutedEventArgs e)
        {
            RefreshDgRequisition();
        }

        private void SelectRequsition(object sender, SelectionChangedEventArgs e)
        {
            var _item = dgRequisition.SelectedItem as RequisitionView;
            if (_item == null)
            {
                dgRequisitionItems.ItemsSource = null;
                return;
            }
            dgRequisitionItems.ItemsSource = RequisitionItemView.GetById(_item.Id);
        }

        #endregion

        #region 生产管理
        private void RefreshDgProduction()
        {
            new Thread(new ThreadStart(RefreshDgProductionThread)).Start();
        }

        private void RefreshDgProductionThread()
        {
            var _list = ProductionView.GetAll();
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgProduction.ItemsSource = _list;
            });
        }

        private void ShowAllProdution(object sender, RoutedEventArgs e)
        {
            RefreshDgProduction();
        }

        private void NewProduction(object sender, RoutedEventArgs e)
        {
            var _win = new NewProduction();
            var _rtn = _win.ShowDialog();
            if (_rtn == true) {
                MessageBox.Show("新建生产记录成功");
                RefreshDgProduction();
            }
            else
            {
                MessageBox.Show("新建生产记录失败");
            }
        }

        private void FinishProduction(object sender, RoutedEventArgs e)
        {
            var _item = dgProduction.SelectedItem as ProductionView;
            if (_item == null) return;
            if (_item.Status == "已完成") return;
            using (var _db = new ModelContainer())
            {
                var _endTime = DateTime.Now;
                var _production = _db.ProductionSet.FirstOrDefault(i => i.Id == _item.Id);
                _production.Status = "已完成";
                _production.FinishedAt = _endTime;
                _db.SaveChanges();
                _item.Status = "已完成";
                _item.FinishedAt = _endTime;
                dgProduction.Items.Refresh();
            }
        }
        #endregion

        #region 权限控制
        private void CheckPrivilege()
        {
            using (var _db = new ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 4 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
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
            btnNewProduction.Visibility = Visibility.Hidden;
            btnFinishProduction.Visibility = Visibility.Hidden;
            btnSubmitRequisition.Visibility = Visibility.Hidden;
            btnConfirmRequisition.Visibility = Visibility.Hidden;
        }
        #endregion

    }
}