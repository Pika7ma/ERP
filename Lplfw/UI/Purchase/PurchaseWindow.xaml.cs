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
            var _thread1 = new Thread(new ThreadStart(Refreshtvpurchase));
            _thread1.Start();
            var _thread2 = new Thread(new ThreadStart(Refreshdgmaterialpurchase));
            _thread2.Start();
            var _thread3 = new Thread(new ThreadStart(RefreshdgPurchases));
            _thread3.Start();
            RefreshtvPrice();
            var _thread4 = new Thread(new ThreadStart(Rdpthread));
            _thread4.Start();
            var _thread5 = new Thread(new ThreadStart(Rdsthread));
            _thread5.Start();
            var _thread6 = new Thread(new ThreadStart(Refresh));
            _thread6.Start();
        }

        private void LoadComponent(object sender, RoutedEventArgs e)
        {
            var _thread = new Thread(new ThreadStart(LoadComponentThread));
            _thread.Start();
        }

        private void LoadComponentThread()
        {
            var _sbbp = new List<Utils.KeyValue>
            {
                new Utils.KeyValue {ID=1, Name="材料"},
                new Utils.KeyValue {ID=2, Name="供应商"},
            };

            var _sbbs = new List<Utils.KeyValue>
            {
                new Utils.KeyValue {ID=1, Name="名称"},
                new Utils.KeyValue {ID=2, Name="联系人"},
                new Utils.KeyValue {ID=3, Name="地址"},
                new Utils.KeyValue {ID=4, Name="电话"},
            };

            var _sbbm = new List<Utils.KeyValue>
            {
                new Utils.KeyValue {ID=1, Name="材料"},
                new Utils.KeyValue {ID=2, Name="供应商"},
                new Utils.KeyValue {ID=3, Name="低于线"},
            };

            var _sbbpur = new List<Utils.KeyValue>
            {
                new Utils.KeyValue {ID=1, Name="订单id"},
                new Utils.KeyValue {ID=2, Name="创建时间"},
                new Utils.KeyValue {ID=3, Name="完成时间"},
                new Utils.KeyValue {ID=4, Name="负责人"},
                new Utils.KeyValue {ID=5, Name="供货商"},
            };

            Dispatcher.BeginInvoke((Action)delegate ()
            {
                cbbSearchSupplierClass.ItemsSource = _sbbs;
                cbbSearchPriceClass.ItemsSource = _sbbp;
                cbSearchMaterialClass.ItemsSource = _sbbm;
                cbSearchPurchaseClass.ItemsSource = _sbbpur;
            });
        }

        private void RefreshdgPrice()
        {
            var _thread = new Thread(new ThreadStart(Rdpthread));
            _thread.Start();
        }

        private void Rdpthread()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _dg = MaterialPriceView.GetAllPrice();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgPrice.ItemsSource = _dg;
                });
            }
        }
        /// </summary>
        private void RefreshdgPurchases()
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgPurchases.ItemsSource = PurchaseView.Getall();
            });
        }

        private void Refreshdgmaterialpurchase()
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgmaterialpurchase.ItemsSource = MaterialPurchaseView.Getall();
            });
        }
        private void Refreshtvpurchase()
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                tvpurchase.Items.Clear();
                MaterialClass.SetTreeFromRoot(ref tvpurchase);
            });
        }


        private void RefreshdgSupplier()
        {
            var _thread = new Thread(new ThreadStart(Rdsthread));
            _thread.Start();
        }

        private void Rdsthread()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _dg = _db.SupplierSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgSupplier.ItemsSource = _dg;
                });
            }
        }

        private void BtnShowAll2Click(object sender, RoutedEventArgs e)
        {
            RefreshdgPrice();
        }

        private void BtnShowAll3Click(object sender, RoutedEventArgs e)
        {
            RefreshdgSupplier();
        }

        private void BtnNewPriceClick(object sender, RoutedEventArgs e)
        {
            var _win = new Purchase.NewPriceClass();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshdgPrice();
            }
        }

        private void BtnEditPriceClick(object sender, RoutedEventArgs e)
        {
            if (dgPrice.SelectedItem == null)
            {
                MessageBox.Show("需要选中报价条目");
            }
            else
            {
                int _mid = (int)((MaterialPriceView)dgPrice.SelectedItem).mId;
                int _sid = (int)((MaterialPriceView)dgPrice.SelectedItem).sId;
                var _win = new Purchase.EditPriceClass(_mid, _sid);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    RefreshdgPrice();
                }
            }
        }

        private void BtnNewSupplierClick(object sender, RoutedEventArgs e)
        {
            var _win = new Purchase.NewSupplierClass();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshdgSupplier();
            }
        }

        private void BtnEditSupplierClick(object sender, RoutedEventArgs e)
        {
            if (dgSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中供应商条目");
            }
            else
            {
                int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                var _win = new Purchase.EditSupplierClass(_id);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    RefreshdgSupplier();
                }
            }
        }

        private void BtnDeletePriceClick(object sender, RoutedEventArgs e)
        {
            if (dgPrice.SelectedItem == null)
            {
                MessageBox.Show("需要选中报价条目");
            }
            else
            {

                new Thread(new ParameterizedThreadStart(Bdpthread)).Start(new int[] {
                    (int)((MaterialPriceView)dgPrice.SelectedItem).mId,
                    (int)((MaterialPriceView)dgPrice.SelectedItem).sId
                });
            }
        }

        private void Bdpthread(object li)
        {
            try
            {
                var _li = li as int[];
                var _mid = _li[0];
                var _sid = _li[1];
                using (var _db = new DAL.ModelContainer())
                {
                    var _price = _db.MaterialPriceSet.First(i => i.MaterialId == _mid && i.SupplierId == _sid);
                    _db.MaterialPriceSet.Remove(_price);
                    _db.SaveChanges();
                }
                RefreshdgPrice();
                MessageBox.Show("删除成功");
            }
            catch (Exception)
            {
                MessageBox.Show("删除失败");
            }
        }

        private void BtnDeleteSupplierClick(object sender, RoutedEventArgs e)
        {
            if (dgSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中供应商条目");
            }
            else
            {
                int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                new Thread(new ParameterizedThreadStart(Bdsthread)).Start(_id);
                MessageBox.Show("删除成功");
            }
        }

        private void Bdsthread(object id)
        {
            int _id = (int)id;
            using (var _db = new DAL.ModelContainer())
            {
                if (_db.MaterialPriceSet.Where(i => i.SupplierId == _id).Count() == 0)
                {
                    var _temp = _db.SupplierSet.First(i => i.Id == _id);
                    _db.SupplierSet.Remove(_temp);
                    _db.SaveChanges();
                    RefreshdgSupplier();
                }
                else
                {
                    var _rtn = MessageBox.Show("是否删除其下的所有报价", null, MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (_rtn == MessageBoxResult.OK)
                    {
                        var _temp = _db.SupplierSet.First(i => i.Id == _id);
                        var _temp2 = _db.MaterialPriceSet.Where(i => i.SupplierId == _id).ToList();
                        _db.MaterialPriceSet.RemoveRange(_temp2);
                        _db.SupplierSet.Remove(_temp);
                        _db.SaveChanges();
                        RefreshdgPrice();
                        RefreshdgSupplier();
                    }
                    else
                    {

                    }
                }
            }
        }

        private void RefreshtvPrice()
        {
            tvPrice.Items.Clear();
            MaterialClass.SetTreeFromRoot(ref tvPrice);
        }

        private void Tpthread(object ml)
        {
            var _ml = (List<Material>)ml;
            using (var _db = new DAL.ModelContainer())
            {
                var _dg = MaterialPriceView.GetbyIdList(_ml);
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgPrice.ItemsSource = _dg;
                });
            }
        }

        private void BtnNewPrice2Click(object sender, RoutedEventArgs e)
        {
            if (dgSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中供应商条目");
            }
            else
            {
                int _sid = (int)((Supplier)dgSupplier.SelectedItem).Id;
                var _win = new Purchase.NewPriceWithSupplierClass(_sid);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                    dgMaterialinSupplier.ItemsSource = MaterialPriceView.GetbySupplierId(_id);
                }
            }
        }

        private void BtnEditPrice2Click(object sender, RoutedEventArgs e)
        {
            if (dgMaterialinSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中报价条目");
            }
            else
            {
                int _mid = (int)((MaterialPriceView)dgMaterialinSupplier.SelectedItem).mId;
                int _sid = (int)((Supplier)dgSupplier.SelectedItem).Id;
                var _win = new Purchase.EditPriceClass(_mid, _sid);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                    dgMaterialinSupplier.ItemsSource = MaterialPriceView.GetbySupplierId(_id);
                }
            }
        }

        private void BtnDeletePrice2Click(object sender, RoutedEventArgs e)
        {
            if (dgMaterialinSupplier.SelectedItem == null)
            {
                MessageBox.Show("需要选中报价条目");
            }
            else
            {
                int _mid = (int)((MaterialPriceView)dgMaterialinSupplier.SelectedItem).mId;
                int _sid = (int)((Supplier)dgSupplier.SelectedItem).Id;
                using (var _db = new DAL.ModelContainer())
                {
                    var _price = _db.MaterialPriceSet.First(i => i.MaterialId == _mid && i.SupplierId == _sid);
                    _db.MaterialPriceSet.Remove(_price);
                    _db.SaveChanges();
                    int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                    dgMaterialinSupplier.ItemsSource = MaterialPriceView.GetbySupplierId(_id);
                    MessageBox.Show("删除成功");
                }
            }
        }

        private void BtSearchPriceClassClick(object sender, RoutedEventArgs e)
        {
            switch ((int)cbbSearchPriceClass.SelectedValue)
            {
                case 1: dgPrice.ItemsSource = MaterialPriceView.GetbyMaterialName(searchPrice.Text); break;
                case 2: dgPrice.ItemsSource = MaterialPriceView.GetbySupplierName(searchPrice.Text); break;
            }
        }

        private void BtSearchSupplierClassClick(object sender, RoutedEventArgs e)
        {
            using (var _db = new DAL.ModelContainer())
            {
                switch ((int)cbbSearchSupplierClass.SelectedValue)
                {
                    case 1: dgSupplier.ItemsSource = _db.SupplierSet.Where(i => i.Name == searchSupplier.Text).ToList(); break;
                    case 2: dgSupplier.ItemsSource = _db.SupplierSet.Where(i => i.Contact == searchSupplier.Text).ToList(); break;
                    case 3: dgSupplier.ItemsSource = _db.SupplierSet.Where(i => i.Location == searchSupplier.Text).ToList(); break;
                    case 4: dgSupplier.ItemsSource = _db.SupplierSet.Where(i => i.Tel == searchSupplier.Text).ToList(); break;
                }
            }
        }

        private void TvPriceMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                int _id = (int)((TreeViewItem)tvPrice.SelectedItem).DataContext;
                var _ml = MaterialClass.GetSubClassMaterials(_id);
                new Thread(new ParameterizedThreadStart(Tpthread)).Start(_ml);
            }
            catch (Exception)
            {

            }
        }

        private void DgSupplierMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                int _id = (int)((Supplier)dgSupplier.SelectedItem).Id;
                new Thread(new ParameterizedThreadStart(Dsthread)).Start(_id);
            }
            catch (Exception)
            {

            }
        }

        private void Dsthread(object id)
        {
            int _id = (int)id;
            var _temp = MaterialPriceView.GetbySupplierId(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgMaterialinSupplier.ItemsSource = _temp;
            });
        }



        /// <summary>
        /// 一下为材料浏览页面交互函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowAllClick(object sender, RoutedEventArgs e)
        {
            Refreshdgmaterialpurchase();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearchMaterialClassClick(object sender, RoutedEventArgs e)
        {
            if (cbSearchMaterialClass.Text != "" && txsearchMaterial.Text != "")
            {
                try
                {
                    switch ((int)cbSearchMaterialClass.SelectedValue)
                    {
                        case 1: dgmaterialpurchase.ItemsSource = MaterialPurchaseView.Getallbymaterial(txsearchMaterial.Text); break;
                        case 2: dgmaterialpurchase.ItemsSource = MaterialPurchaseView.Getallbysupplier(txsearchMaterial.Text); break;
                        case 3: dgmaterialpurchase.ItemsSource = MaterialPurchaseView.Getalllow(Convert.ToInt32(txsearchMaterial.Text)); break;
                    }
                }
                catch
                {
                    MessageBox.Show("输入有误");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnpurchaseclick(object sender, RoutedEventArgs e)
        {
            var _createpurchase = new NewPurchase();
            _createpurchase.Show();
        }
        /// <summary>
        /// tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewPurchaseMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                int _id = (int)((TreeViewItem)tvpurchase.SelectedItem).DataContext;
                var _ml = MaterialClass.GetSubClassMaterials(_id);
                new Thread(new ParameterizedThreadStart(Tpthread2)).Start(_ml);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ml"></param>
        private void Tpthread2(object ml)
        {
            var _ml = (List<Material>)ml;
            using (var _db = new DAL.ModelContainer())
            {
                var _dg = MaterialPurchaseView.GetbyIdList(_ml);
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgPurchases.ItemsSource = _dg;
                    dgPurchases.Items.Refresh();
                });
            }
        }

        private void BtnCreatePurchaseClick(object sender, RoutedEventArgs e)
        {
            NewPurchase _createpurchase = new NewPurchase();
            _createpurchase.Show();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChangePurchaseClick(object sender, RoutedEventArgs e)
        {
            var _item = dgPurchases.SelectedItem as PurchaseView;
            if (_item == null)
            {
                MessageBox.Show("请选择条目");
                return;
            }
            if (_item.Status == "完成")
            {
                MessageBox.Show("订单已完成");
                return;
            }
            EditPurchase changepurchase = new EditPurchase(_item.Id);
            changepurchase.Id = _item.Id;
            changepurchase.ShowDialog();
            dgPurchases.Items.Refresh();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteOderClick(object sender, RoutedEventArgs e)
        {
            PurchaseView purchase = (PurchaseView)dgPurchases.SelectedItem;
            if (purchase == null)
            {
                MessageBox.Show("请选择订单");

            }
            else
            {
                using (var _db = new DAL.ModelContainer())
                {
                    DAL.Purchase pc = _db.PurchaseSet.First(i => i.Id == purchase.Id);
                    List<PurchaseItem> purchaseItems = _db.PurchaseItemSet.Where(i => i.PurchaseId == purchase.Id).ToList();
                    _db.PurchaseItemSet.RemoveRange(purchaseItems);
                    _db.PurchaseSet.Remove(pc);
                    _db.SaveChanges();

                }
                MessageBox.Show("删除成功");
                dgPurchases.Items.Refresh();

            }
        }
        /// <summary>
        /// showall
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowAll4Click(object sender, RoutedEventArgs e)
        {
            RefreshdgPurchases();
        }
        /// <summary>
        /// 自检单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewqualityClick(object sender, RoutedEventArgs e)
        {
            var _purchase = (PurchaseView)dgPurchases.SelectedItem;
            if (_purchase == null)
            {
                MessageBox.Show("请选择订单");
            }
            else if (_purchase.Status != "完成")
            {
                MessageBox.Show("此订单尚未完成");
            }
            else
            {
                var newquality = new NewPurchaseQuality(_purchase.Id);
                newquality.Show();

            }
        }

        private void BtnSearchSupplierClassClick(object sender, RoutedEventArgs e)
        {
            if (cbSearchPurchaseClass.Text != "")
            {
                switch ((int)cbSearchPurchaseClass.SelectedValue)
                {
                    case 1: if (txsearchpurchase.Text != "") dgPurchases.ItemsSource = PurchaseView.Getallbyid(Convert.ToInt32(txsearchpurchase.Text)); break;
                    case 2: if (dpsettime.Text != "") dgPurchases.ItemsSource = PurchaseView.Getallbycreateat(dpsettime.Text); break;
                    case 3: if (dpsettime.Text != "") dgPurchases.ItemsSource = PurchaseView.Getallbycfinishedat(dpsettime.Text); break;
                    case 4: if (txsearchpurchase.Text != "") dgPurchases.ItemsSource = PurchaseView.Getallbyuser(txsearchpurchase.Text); break;
                    case 5: if (txsearchpurchase.Text != "") dgPurchases.ItemsSource = PurchaseView.Getallbysupplier(txsearchpurchase.Text); break;
                }
            }

        }

        private void CbbSearchPurchaseClassSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSearchPurchaseClass.SelectedIndex == 1 || cbSearchPurchaseClass.SelectedIndex == 2)
            {
                txsearchpurchase.Visibility = Visibility.Collapsed;
                dpsettime.Visibility = Visibility.Visible;
            }
            else
            {
                txsearchpurchase.Visibility = Visibility.Visible;
                dpsettime.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnShowqualityClick(object sender, RoutedEventArgs e)
        {
            var _showquality = new ShowPurchaseQuality();
            _showquality.Show();
        }

        #region 权限控制

        private void Refresh()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 5 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
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

        }
        #endregion
    }
}
