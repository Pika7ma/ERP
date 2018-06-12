using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Lplfw.UI.Inventory
{
    /// <summary>
    /// InventoryWindow.xaml 的交互逻辑
    /// </summary>
    ///
    public partial class InventoryWindow : Window
    {
        public InventoryWindow()
        {
            InitializeComponent();
            var _thread = new Thread(new ThreadStart(Refresh));
            _thread.Start();
        }

        #region 仓库管理
        private void RefreshDgStorage()
        {
            new Thread(new ThreadStart(RefreshDgStorageThread)).Start();
        }
        private void RefreshDgStorageThread()
        {
            var _list = StorageView.GetAll();
            Dispatcher.Invoke((Action)delegate ()
            {
                dgStorage.ItemsSource = _list;
            });
        }

        private void NewStorage(object sender, RoutedEventArgs e)
        {
            var _win = new NewStorage();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshDgStorage();
            }
        }

        private void EditStorage(object sender, RoutedEventArgs e)
        {
            var _storage = dgStorage.SelectedItem as StorageView;
            if (_storage == null) return;
            var _win = new NewStorage(_storage);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshDgStorage();
            }
        }

        private void DeleteStorage(object sender, RoutedEventArgs e)
        {
            var _storage = dgStorage.SelectedItem as StorageView;
            if (_storage == null) return;
            var _rtn = MessageBox.Show($"确定要删除仓库 {_storage.Name} 吗?", null, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (_rtn == MessageBoxResult.Yes)
            {
                new Thread(new ParameterizedThreadStart(DeleteStorageThread)).Start(_storage.Id);
            }
        }

        private void DeleteStorageThread(object index)
        {
            var _id = (int)index;
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _storage = _db.StorageSet.FirstOrDefault(i => i.Id == _id);
                    _db.StorageSet.Remove(_storage);
                    _db.SaveChanges();
                }
                Dispatcher.Invoke((Action)delegate ()
                {
                    MessageBox.Show("删除仓库成功", null, MessageBoxButton.OK, MessageBoxImage.Information);
                });
                RefreshDgStorage();
            }
            catch (Exception)
            {
                Dispatcher.Invoke((Action)delegate ()
                {
                    MessageBox.Show("删除仓库失败", null, MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
        #endregion

        #region 库存管理
        private void RefreshCbStorageInventory()
        {
            new Thread(new ThreadStart(RefreshCbStorageInventoryThread)).Start();
        }
        private void RefreshCbStorageInventoryThread()
        {
            var _list = Storage.GetAllToComboBox();
            Dispatcher.Invoke((Action)delegate ()
            {
                cbStorageInventory.ItemsSource = _list;
                cbStorageInventory.SelectedValue = 0;
            });
        }

        private void CbStorageChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDgMaterial();
            RefreshDgProduct();
        }

        private void RefreshDgMaterial()
        {
            var _id = 0;
            if (cbStorageInventory.SelectedItem is Storage _storage) _id = _storage.Id;
            new Thread(new ParameterizedThreadStart(RefreshDgMaterialThread)).Start(_id);
        }
        private void RefreshDgMaterialThread(object id)
        {
            var _id = (int)id;
            if (_id == 0)
            {
                var _list = MaterialStockAllView.GetAll();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgMaterial.ItemsSource = _list;
                });
            }
            else
            {
                var _list = MaterialStockView.GetById(_id);
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgMaterial.ItemsSource = _list;
                });
            }
        }

        private void RefreshDgProduct()
        {
            var _id = 0;
            if (cbStorageInventory.SelectedItem is Storage _storage) _id = _storage.Id;
            new Thread(new ParameterizedThreadStart(RefreshDgProductThread)).Start(_id);
        }
        private void RefreshDgProductThread(object id)
        {
            var _id = (int)id;
            if (_id == 0)
            {
                var _list = ProductStockAllView.Get();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgProduct.ItemsSource = _list;
                });
            }
            else
            {
                var _list = ProductStockView.GetById(_id);
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgProduct.ItemsSource = _list;
                });
            }
        }

        private void NewStockIn(object sender, RoutedEventArgs e)
        {
            var _id = cbStorageInventory.SelectedValue as int?;
            var _win = new NewStockInOut(true, _id);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshDgMaterial();
                RefreshDgProduct();
            }
        }

        private void NewStockOut(object sender, RoutedEventArgs e)
        {
            var _id = cbStorageInventory.SelectedValue as int?;
            var _win = new NewStockInOut(false, _id);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshDgMaterial();
                RefreshDgProduct();
            }
        }

        private void ShowCost(object sender, RoutedEventArgs e)
        {
            double _sum = 0;
            if (tabMaterial.IsSelected)
            {
                if (dgMaterial.ItemsSource is List<MaterialStockView> _materials)
                {
                    for (var _i = 0; _i < _materials.Count; _i++)
                    {
                        _sum += _materials[_i].Cost;
                    }
                }
                else if (dgMaterial.ItemsSource is List<MaterialStockAllView> _materialAlls)
                {
                    for (var _i = 0; _i < _materialAlls.Count; _i++)
                    {
                        _sum += _materialAlls[_i].Cost;
                    }
                }
            }
            else if (tabProduct.IsSelected)
            {
                if (dgProduct.ItemsSource is List<ProductStockView> _products)
                {
                    for (var _i = 0; _i < _products.Count; _i++)
                    {
                        _sum += _products[_i].Cost;
                    }
                }
                else if (dgProduct.ItemsSource is List<ProductStockAllView> _productAlls)
                {
                    for (var _i = 0; _i < _productAlls.Count; _i++)
                    {
                        _sum += _productAlls[_i].Cost;
                    }
                }
            }
            new Thread(new ParameterizedThreadStart(ShowCostThread)).Start(_sum.ToString());
        }
        private void ShowCostThread(object currentCost)
        {
            var _currentCost = currentCost as string;
            double _ssum = 0, _psum = 0;
            var _materials = MaterialStockAllView.GetAll();
            if (_materials == null) return;
            var _products = ProductStockAllView.Get();
            if (_products == null) return;
            for (var _i = 0; _i < _materials.Count; _i++)
            {
                _ssum += _materials[_i].Cost;
            }
            for (var _i = 0; _i < _products.Count; _i++)
            {
                _psum += _products[_i].Cost;
            }
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                MessageBox.Show($"总库存成本为{_ssum + _psum}\n总原料成本为{_ssum}\n总产品成本为{_psum}\n当前页面显示项目成本为{_currentCost}", "成本统计");
            });

        }
        #endregion

        #region 出入库单据浏览
        private void RefreshCbStorageStock()
        {
            new Thread(new ThreadStart(RefreshCbStorageStockThread)).Start();
        }
        private void RefreshCbStorageStockThread()
        {
            var _list = Storage.GetAllToComboBox();
            Dispatcher.Invoke((Action)delegate ()
            {
                cbStorageStock.ItemsSource = _list;
                cbStorageStock.SelectedValue = 0;
            });
        }

        private void CbStorageStockChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDgStockIn();
            RefreshDgSrockOut();
        }

        private void RefreshDgStockIn()
        {
            var _id = 0;
            if (cbStorageInventory.SelectedItem is Storage _storage) _id = _storage.Id;
            new Thread(new ParameterizedThreadStart(RefreshDgStockInThread)).Start(_id);
        }
        private void RefreshDgStockInThread(object id)
        {
            var _id = (int)id;
            var _list = StockInView.GetById(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgStockIn.ItemsSource = _list;
            });
        }

        private void RefreshDgSrockOut()
        {
            var _id = 0;
            if (cbStorageInventory.SelectedItem is Storage _storage) _id = _storage.Id;
            new Thread(new ParameterizedThreadStart(RefreshDgStockOutThread)).Start(_id);
        }
        private void RefreshDgStockOutThread(object id)
        {
            var _id = (int)id;
            var _list = StockOutView.GetById(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgStockOut.ItemsSource = _list;
            });
        }

        private void DgStockInChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStockIn.SelectedItem is StockInView _stockin)
            {
                var _id = _stockin.Id;
                dgMaterialItems.ItemsSource = MaterialStockInItemView.GetById(_id);
                dgProductItems.ItemsSource = ProductStockInItemView.GetById(_id);
            }
        }

        private void DgStockOutChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStockOut.SelectedItem is StockOutView _stockout)
            {
                var _id = _stockout.Id;
                dgMaterialItems.ItemsSource = MaterialStockOutItemView.GetById(_id);
                dgProductItems.ItemsSource = ProductStockOutItemView.GetById(_id);
            }
        }

        #endregion

        #region 权限控制
        private void Refresh()
        {
            using (var _db = new ModelContainer())
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
            btnNewStorage.Visibility = Visibility.Hidden;
            btnEditStorage.Visibility = Visibility.Hidden;
            btnDelStorage.Visibility = Visibility.Hidden;
            btnNewIn.Visibility = Visibility.Hidden;
            btnNewOut.Visibility = Visibility.Hidden;
            btnEditSafety.Visibility = Visibility.Hidden;
        }

        #endregion

        private void TabRouter(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl _e)
            {
                if (_e.Name == "tabPage")
                {
                    if (tabStorage.IsSelected)
                    {
                        RefreshDgStorage();
                    }
                    else if (tabInventory.IsSelected)
                    {
                        RefreshCbStorageInventory();
                    }
                    else if (tabStock.IsSelected)
                    {
                        RefreshCbStorageStock();
                    }
                    else if (tabSafeQuantity.IsSelected)
                    {
                        tvMaterialSafe.Items.Clear();
                        MaterialClass.SetTreeFromRoot(ref tvMaterialSafe);
                    }
                }
            }
        }

        private void SelectMaterialClass(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvMaterialSafe.SelectedItem == null) return;
            var _id = (int)Utils.GetTreeViewSelectedValue(ref tvMaterialSafe);
            new Thread(new ParameterizedThreadStart(SelectMaterialClassThread)).Start(_id);
        }

        private void SelectMaterialClassThread(object id)
        {
            var _id = (int)id;
            var _material = MaterialClass.GetSubClassMaterials(_id);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                dgMaterialSafe.ItemsSource = _material;
            });
        }

        private void EditSafeQuantity(object sender, RoutedEventArgs e)
        {
            if (dgMaterialSafe.SelectedItem is Material _material)
            {
                var _win = new EditSafeQuantity(_material);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    if (tvMaterialSafe.SelectedItem == null) return;
                    var _id = (int)Utils.GetTreeViewSelectedValue(ref tvMaterialSafe);
                    new Thread(new ParameterizedThreadStart(SelectMaterialClassThread)).Start(_id);
                }
            }
        }
    }
}