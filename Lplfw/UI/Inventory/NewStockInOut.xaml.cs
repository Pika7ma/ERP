using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading;

namespace Lplfw.UI.Inventory
{
    /// <summary>
    /// NewStockInOut.xaml 的交互逻辑
    /// </summary>
    public partial class NewStockInOut : Window
    {
        public List<Material> Materials { get; set; }
        public List<Product> Products { get; set; }

        private bool isStockIn;
        private int? storageId;
        private StockInViewModel stockIn;
        private StockOutViewModel stockOut;
        private List<MaterialStockInItem> materialStockInItems;
        private List<MaterialStockOutItem> materialStockOutItems;
        private List<ProductStockInItem> productStockInItems;
        private List<ProductStockOutItem> productStockOutItems;

        public NewStockInOut(bool isStockIn, int? storageId)
        {
            InitializeComponent();
            DataContext = this;
            using (var _db = new ModelContainer())
            {
                Materials = _db.MaterialSet.ToList();
                Products = _db.ProductSet.ToList();
            }
            Title = isStockIn ? "新建入库单据" : "新建出库单据";
            this.isStockIn = isStockIn;
            this.storageId = storageId;
            SetControls();
            if (isStockIn)
            {
                stockIn = new StockInViewModel();
                cbStorage.Binding(stockIn, "CbStorage");
                dpTime.Binding(stockIn, "DpTime");
                cbUser.Binding(stockIn, "CbUser");
                txtDescription.Binding(stockIn, "TxtDescription");
                materialStockInItems = new List<MaterialStockInItem>();
                productStockInItems = new List<ProductStockInItem>();
                dgMaterialItems.ItemsSource = materialStockInItems;
                dgProductItems.ItemsSource = productStockInItems;
            }
            else
            {
                stockOut = new StockOutViewModel();
                cbStorage.Binding(stockOut, "CbStorage");
                dpTime.Binding(stockOut, "DpTime");
                cbUser.Binding(stockOut, "CbUser");
                txtDescription.Binding(stockOut, "TxtDescription");
                materialStockOutItems = new List<MaterialStockOutItem>();
                productStockOutItems = new List<ProductStockOutItem>();
            }
            dpTime.SelectedDate = DateTime.Now;
        }

        private void SetControls()
        {
            new Thread(new ThreadStart(SetControlsThread)).Start();
        }
        private void SetControlsThread()
        {
            using (var _db = new ModelContainer())
            {
                var _storages = _db.StorageSet.ToList();
                var _users = _db.UserSet.ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbStorage.ItemsSource = _storages;
                    cbStorage.SelectedValue = storageId;
                    cbUser.ItemsSource = _users;
                    cbUser.SelectedValue = Utils.CurrentUser.Id;
                });
            }
        }

        #region 原料
        private void NewMaterialItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewStockItem(true, isStockIn);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                if (isStockIn)
                {
                    var _item = Utils.TempObject as MaterialStockInItem;
                    Utils.TempObject = null;
                    var _old = materialStockInItems.FirstOrDefault(i => i.MaterialId == _item.MaterialId);
                    if (_old == null) materialStockInItems.Add(_item);
                    else
                    {
                        _old.Quantity += _item.Quantity;
                        _old.Location = $"{_old.Location},{_item.Location}";
                    }
                    dgMaterialItems.Items.Refresh();
                }
                else
                {
                    var _item = Utils.TempObject as MaterialStockOutItem;
                    Utils.TempObject = null;
                    var _old = materialStockOutItems.FirstOrDefault(i => i.MaterialId == _item.MaterialId);
                    if (_old == null) materialStockOutItems.Add(_item);
                    else
                    {
                        _old.Quantity += _item.Quantity;
                        _old.Location = $"{_old.Location},{_item.Location}";
                    }
                    dgMaterialItems.Items.Refresh();
                }
            }
        }

        private void EditMaterialItem(object sender, RoutedEventArgs e)
        {
            if (isStockIn)
            {
                var item = dgMaterialItems.SelectedItem as MaterialStockInItem;
                if (item == null) return;
                var _win = new NewStockItem(item, true, isStockIn);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    var _item = Utils.TempObject as MaterialStockInItem;
                    Utils.TempObject = null;
                    var _old = materialStockInItems.FirstOrDefault(i => i.MaterialId == _item.MaterialId);
                    if (_old == null) return;
                    _old.MaterialId = _item.MaterialId;
                    _old.Quantity = _item.Quantity;
                    _old.Location = _item.Location;
                    dgMaterialItems.Items.Refresh();
                }

            }
            else
            {
                var item = dgMaterialItems.SelectedItem as MaterialStockOutItem;
                if (item == null) return;
                var _win = new NewStockItem(item, true, isStockIn);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    var _item = Utils.TempObject as MaterialStockOutItem;
                    Utils.TempObject = null;
                    var _old = materialStockOutItems.FirstOrDefault(i => i.MaterialId == _item.MaterialId);
                    if (_old == null) return;
                    _old.MaterialId = _item.MaterialId;
                    _old.Quantity = _item.Quantity;
                    _old.Location = _item.Location;
                    dgMaterialItems.Items.Refresh();
                }
            }
        }

        private void DeleteMaterialItem(object sender, RoutedEventArgs e)
        {
            if (isStockIn)
            {
                var _item = dgMaterialItems.SelectedItem as MaterialStockInItem;
                if (_item == null) return;
                materialStockInItems.Remove(_item);
                dgMaterialItems.Items.Refresh();
            }
            else
            {
                var _item = dgMaterialItems.SelectedItem as MaterialStockOutItem;
                if (_item == null) return;
                materialStockOutItems.Remove(_item);
                dgMaterialItems.Items.Refresh();
            }
        }
        #endregion

        #region 产品
        private void NewProductItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewStockItem(false, isStockIn);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                if (isStockIn)
                {
                    var _item = Utils.TempObject as ProductStockInItem;
                    Utils.TempObject = null;
                    var _old = productStockInItems.FirstOrDefault(i => i.ProductId == _item.ProductId);
                    if (_old == null) productStockInItems.Add(_item);
                    else
                    {
                        _old.Quantity += _item.Quantity;
                        _old.Location = $"{_old.Location},{_item.Location}";
                    }
                    dgProductItems.Items.Refresh();
                }
                else
                {
                    var _item = Utils.TempObject as ProductStockOutItem;
                    Utils.TempObject = null;
                    var _old = productStockOutItems.FirstOrDefault(i => i.ProductId == _item.ProductId);
                    if (_old == null) productStockOutItems.Add(_item);
                    else
                    {
                        _old.Quantity += _item.Quantity;
                        _old.Location = $"{_old.Location},{_item.Location}";
                    }
                    dgProductItems.Items.Refresh();
                }
            }
        }

        private void EditProductItem(object sender, RoutedEventArgs e)
        {
            if (isStockIn)
            {
                var item = dgProductItems.SelectedItem as ProductStockInItem;
                if (item == null) return;
                var _win = new NewStockItem(item, false, isStockIn);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    var _item = Utils.TempObject as ProductStockInItem;
                    Utils.TempObject = null;
                    var _old = productStockInItems.FirstOrDefault(i => i.ProductId == _item.ProductId);
                    if (_old == null) return;
                    _old.ProductId = _item.ProductId;
                    _old.Quantity = _item.Quantity;
                    _old.Location = _item.Location;
                    dgProductItems.Items.Refresh();
                }

            }
            else
            {
                var item = dgProductItems.SelectedItem as ProductStockOutItem;
                if (item == null) return;
                var _win = new NewStockItem(item, false, isStockIn);
                var _rtn = _win.ShowDialog();
                if (_rtn == true)
                {
                    var _item = Utils.TempObject as ProductStockOutItem;
                    Utils.TempObject = null;
                    var _old = productStockOutItems.FirstOrDefault(i => i.ProductId == _item.ProductId);
                    if (_old == null) return;
                    _old.ProductId = _item.ProductId;
                    _old.Quantity = _item.Quantity;
                    _old.Location = _item.Location;
                    dgProductItems.Items.Refresh();
                }
            }
        }

        private void DeleteProductItem(object sender, RoutedEventArgs e)
        {
            if (isStockIn)
            {
                var _item = dgProductItems.SelectedItem as ProductStockInItem;
                if (_item == null) return;
                productStockInItems.Remove(_item);
                dgProductItems.Items.Refresh();
            }
            else
            {
                var _item = dgProductItems.SelectedItem as ProductStockOutItem;
                if (_item == null) return;
                productStockOutItems.Remove(_item);
                dgProductItems.Items.Refresh();
            }
        }
        #endregion

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            //TODO: 入库出库的虚拟库存, 为0时自动删除
            try {
                if (isStockIn)
                {
                    if (stockIn.CanSubmit)
                    {
                        // 新建入库
                        using (var _db = new ModelContainer())
                        {
                            var _stockIn = stockIn.Object;
                            _db.StockInSet.Add(_stockIn);
                            _db.SaveChanges();
                            for (var _i = 0; _i < materialStockInItems.Count; _i++)
                            {
                                var _item = materialStockInItems[_i];
                                _item.StockInId = _stockIn.Id;
                                var _stock = _db.MaterialStockSet.FirstOrDefault(i => i.StorageId == _stockIn.StorageId
                                    && i.MaterialId == _item.MaterialId);
                                if (_stock == null)
                                {
                                    _db.MaterialStockSet.Add(new MaterialStock
                                    {
                                        StorageId = _stockIn.StorageId,
                                        MaterialId = _item.MaterialId,
                                        Quantity = _item.Quantity,
                                        Location = _item.Location
                                    });
                                }
                                else
                                {
                                    _stock.Quantity += _item.Quantity;
                                    var _locationList = new List<string>(_stock.Location.Split(',')).Union(new List<string>(_item.Location.Split(',')));
                                    _stock.Location = String.Join(",", _locationList.Distinct().ToArray());
                                }
                            }
                            for (var _j = 0; _j < productStockInItems.Count; _j++)
                            {
                                var _item = productStockInItems[_j];
                                _item.StockInId = _stockIn.Id;
                                var _stock = _db.ProductStockSet.FirstOrDefault(i => i.StorageId == _stockIn.StorageId
                                    && i.ProductId == _item.ProductId);
                                if (_stock == null)
                                {
                                    _db.ProductStockSet.Add(new ProductStock
                                    {
                                        StorageId = _stockIn.StorageId,
                                        ProductId = _item.ProductId,
                                        Quantity = _item.Quantity,
                                        Location = _item.Location
                                    });
                                }
                                else
                                {
                                    _stock.Quantity += _item.Quantity;
                                    var _locationList = new List<string>(_stock.Location.Split(',')).Union(new List<string>(_item.Location.Split(',')));
                                    _stock.Location = String.Join(",", _locationList.Distinct().ToArray());
                                }
                            }
                            _db.MaterialStockInItemSet.AddRange(materialStockInItems);
                            _db.ProductStockInItemSet.AddRange(productStockInItems);
                            _db.SaveChanges();
                            DialogResult = true;
                        }
                    }
                    else
                    {
                        txtMessage.Text = stockIn.CheckMessage;
                    }
                }
                else
                {
                    if (stockOut.CanSubmit)
                    {
                        // 新建出库
                        using (var _db = new ModelContainer())
                        {
                            var _stockOut = stockOut.Object;
                            _db.StockOutSet.Add(_stockOut);
                            _db.SaveChanges();
                            for (var _i = 0; _i < materialStockOutItems.Count; _i++)
                            {
                                var _item = materialStockOutItems[_i];
                                _item.StockOutId = _stockOut.Id;
                                var _stock = _db.MaterialStockSet.FirstOrDefault(i => i.StorageId == _stockOut.StorageId
                                    && i.MaterialId == _item.MaterialId);
                                if (_stock == null)
                                {
                                    _db.MaterialStockSet.Add(new MaterialStock
                                    {
                                        StorageId = _stockOut.StorageId,
                                        MaterialId = _item.MaterialId,
                                        Quantity = _item.Quantity,
                                        Location = _item.Location
                                    });
                                }
                                else
                                {
                                    _stock.Quantity += _item.Quantity;
                                    var _locationList = new List<string>(_stock.Location.Split(',')).Union(new List<string>(_item.Location.Split(',')));
                                    _stock.Location = String.Join(",", _locationList.Distinct().ToArray());
                                }
                            }
                            for (var _j = 0; _j < productStockOutItems.Count; _j++)
                            {
                                var _item = productStockOutItems[_j];
                                _item.StockOutId = _stockOut.Id;
                                var _stock = _db.ProductStockSet.FirstOrDefault(i => i.StorageId == _stockOut.StorageId
                                    && i.ProductId == _item.ProductId);
                                if (_stock == null)
                                {
                                    _db.ProductStockSet.Add(new ProductStock
                                    {
                                        StorageId = _stockOut.StorageId,
                                        ProductId = _item.ProductId,
                                        Quantity = _item.Quantity,
                                        Location = _item.Location
                                    });
                                }
                                else
                                {
                                    _stock.Quantity += _item.Quantity;
                                    var _locationList = new List<string>(_stock.Location.Split(',')).Union(new List<string>(_item.Location.Split(',')));
                                    _stock.Location = String.Join(",", _locationList.Distinct().ToArray());
                                }
                            }
                            _db.MaterialStockOutItemSet.AddRange(materialStockOutItems);
                            _db.ProductStockOutItemSet.AddRange(productStockOutItems);
                            _db.SaveChanges();
                            DialogResult = true;
                        }
                    }
                    else
                    {
                        txtMessage.Text = stockOut.CheckMessage;
                    }
                }
            } catch (Exception)
            {
                DialogResult = false;
            }
        }
    }
}
