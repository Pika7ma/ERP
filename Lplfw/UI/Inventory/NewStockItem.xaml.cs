using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading;

namespace Lplfw.UI.Inventory
{
    /// <summary>
    /// NewStockItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewStockItem : Window
    {
        private int? storageId;
        private bool isNew;
        private bool isMaterial;
        private bool isIn;
        private object obj;
        private enum TYPE { NewMaterialIn, NewMaterialOut, NewProductIn, NewProductOut, EditMaterialIn, EditMaterialOut, EditProductIn, EditProductOut };
        private TYPE type;
        private int storageQuantity = 0;

        public NewStockItem(bool isMaterial, bool isIn, int? storageId)
        {
            InitializeComponent();
            Title = "新建条目";
            isNew = true;
            this.isMaterial = isMaterial;
            this.isIn = isIn;
            this.storageId = storageId;
            SetType();
            SetControls();
        }

        public NewStockItem(Object edit, bool isMaterial, bool isIn, int? storageId)
        {
            InitializeComponent();
            Title = "修改条目";
            isNew = false;
            this.isMaterial = isMaterial;
            this.isIn = isIn;
            this.storageId = storageId;
            obj = edit;
            SetType();
            SetControls();
        }

        #region 根据情况初始化窗口
        private void SetType()
        {
            if (isNew)
            {
                if (isMaterial)
                {
                    if (isIn) type = TYPE.NewMaterialIn;
                    else type = TYPE.NewMaterialOut;
                }
                else
                {
                    if (isIn) type = TYPE.NewProductIn;
                    else type = TYPE.NewProductOut;
                }
            }
            else
            {
                if (isMaterial)
                {
                    if (isIn) type = TYPE.EditMaterialIn;
                    else type = TYPE.EditMaterialOut;
                }
                else
                {
                    if (isIn) type = TYPE.EditProductIn;
                    else type = TYPE.EditProductOut;
                }
            }
        }

        private void SetItemsSourceMaterialOrProduct() {
            if (isMaterial)
            {
                cbClass.ItemsSource = MaterialClass.GetAllClasses(false);
                btnConfirm.Click += SubmitMaterialItem;
            }
            else
            {
                cbClass.ItemsSource = ProductClass.GetAllClasses(false);
                btnConfirm.Click += SubmitProductItem;
            }
        }

        private void SetItemsSourceInOrOut()
        {
            if (!isIn)
            {
                lblStorage.Visibility = Visibility.Visible;
                txtLocation.Visibility = Visibility.Hidden;
                cbLocation.Visibility = Visibility.Visible;
                txtStorage.Visibility = Visibility.Visible;
            }
        }

        private void SetControls()
        {
            SetItemsSourceMaterialOrProduct();
            SetItemsSourceInOrOut();

            using (var _db = new ModelContainer())
            {
                switch (type)
                {
                    case TYPE.EditMaterialIn:
                        {
                            var _obj = obj as MaterialStockInItem;
                            var _id = _obj.MaterialId;
                            var _material = _db.MaterialSet.FirstOrDefault(i => i.Id == _id);
                            if (_material == null) return;
                            cbClass.SelectedValue = _material.ClassId;
                            cbName.ItemsSource = _db.MaterialSet.Where(i => i.ClassId == _material.ClassId).ToList();
                            cbName.SelectedValue = _material.Id;
                            txtQuantity.Text = _obj.Quantity.ToString();
                            txtLocation.Text = _obj.Location.ToString();
                        }
                        break;
                    case TYPE.EditMaterialOut:
                        {
                            var _obj = obj as MaterialStockOutItem;
                            var _id = _obj.MaterialId;
                            var _material = _db.MaterialSet.FirstOrDefault(i => i.Id == _id);
                            if (_material == null) return;
                            cbClass.SelectedValue = _material.ClassId;
                            cbName.ItemsSource = _db.MaterialSet.Where(i => i.ClassId == _material.ClassId).ToList();
                            cbName.SelectedValue = _material.Id;
                            txtQuantity.Text = _obj.Quantity.ToString();
                            var _stockItems = _db.MaterialStockSet.Where(i => i.StorageId == storageId && i.MaterialId == _id).ToList();
                            cbLocation.ItemsSource = _stockItems;
                            cbLocation.SelectedValue = _obj.Location;
                            var _stockItem = _stockItems.FirstOrDefault(i => i.Location == _obj.Location);
                            storageQuantity = _stockItem.Quantity;
                            txtStorage.Text = _stockItem.Quantity.ToString();
                        }
                        break;
                    case TYPE.EditProductIn:
                        {
                            var _obj = obj as ProductStockInItem;
                            var _id = _obj.ProductId;
                            var _product = _db.ProductSet.FirstOrDefault(i => i.Id == _id);
                            if (_product == null) return;
                            cbClass.SelectedValue = _product.ClassId;
                            cbName.ItemsSource = _db.ProductSet.Where(i => i.ClassId == _product.ClassId).ToList();
                            cbName.SelectedValue = _product.Id;
                            txtQuantity.Text = _obj.Quantity.ToString();
                            txtLocation.Text = _obj.Location.ToString();
                        }
                        break;
                    case TYPE.EditProductOut:
                        {
                            var _obj = obj as ProductStockInItem;
                            var _id = _obj.ProductId;
                            var _product = _db.ProductSet.FirstOrDefault(i => i.Id == _id);
                            if (_product == null) return;
                            cbClass.SelectedValue = _product.ClassId;
                            cbName.ItemsSource = _db.ProductSet.Where(i => i.ClassId == _product.ClassId).ToList();
                            cbName.SelectedValue = _product.Id;
                            txtQuantity.Text = _obj.Quantity.ToString();
                            var _stockItems = _db.ProductStockSet.Where(i => i.StorageId == storageId && i.ProductId == _id).ToList();
                            cbLocation.ItemsSource = _stockItems;
                            cbLocation.SelectedValue = _obj.Location;
                            var _stockItem = _stockItems.FirstOrDefault(i => i.Location == _obj.Location);
                            storageQuantity = _stockItem.Quantity;
                            txtStorage.Text = _stockItem.Quantity.ToString();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool CanSubmit()
        {
            var _id = cbName.SelectedValue as int?;
            if (_id == null || _id == 0)
            {
                txtMessage.Text = "请选择原料或产品";
                return false;
            }
            if (isIn && int.TryParse(txtLocation.Text, out int _locationInt) == false)
            {
                txtMessage.Text = "请输入正确的货位";
                return false;
            }
            var _location = cbLocation.SelectedValue as int?;
            if (!isIn && (_location == null || _location == 0))
            {
                txtMessage.Text = "请选择正确的货位";
                return false;
            }
            if (int.TryParse(txtQuantity.Text, out int _quantity) == false)
            {
                txtMessage.Text = "请输入正确的数量";
                return false;
            }
            else if (_quantity == 0)
            {
                txtMessage.Text = "请输入正确的数量";
                return false;
            }
            else if (!isIn && _quantity > storageQuantity)
            {
                txtMessage.Text = "出库量大于实际库存量";
                return false;
            }
            return true;
        }

        private void SubmitMaterialItem(object sender, RoutedEventArgs e) {
            if (!CanSubmit()) return;
            if (isIn)
            {
                var _item = new MaterialStockInItem
                {
                    MaterialId = (int)cbName.SelectedValue,
                    Quantity = int.Parse(txtQuantity.Text),
                    Location = int.Parse(txtLocation.Text)
                };
                Utils.TempObject = _item;
            }
            else
            {
                var _item = new MaterialStockOutItem
                {
                    MaterialId = (int)cbName.SelectedValue,
                    Quantity = int.Parse(txtQuantity.Text),
                    Location = (int)cbLocation.SelectedValue
                };
                Utils.TempObject = _item;
            }
            DialogResult = true;
        }

        private void SubmitProductItem(object sender, RoutedEventArgs e) {
            if (!CanSubmit()) return;
            if (isIn)
            {
                var _item = new ProductStockInItem
                {
                    ProductId = (int)cbName.SelectedValue,
                    Quantity = int.Parse(txtQuantity.Text),
                    Location = int.Parse(txtLocation.Text)
                };
                Utils.TempObject = _item;
            }
            else
            {
                var _item = new ProductStockOutItem
                {
                    ProductId = (int)cbName.SelectedValue,
                    Quantity = int.Parse(txtQuantity.Text),
                    Location = (int)cbLocation.SelectedValue
                };
                Utils.TempObject = _item;
            }
            DialogResult = true;
        }

        private void ChangeClass(object sender, SelectionChangedEventArgs e)
        {
            var _class = cbClass.SelectedValue as int?;
            if (_class == null) return;
            if (isMaterial) cbName.ItemsSource = MaterialClass.GetSubClassMaterials(_class);
            else cbName.ItemsSource = ProductClass.GetSubClassProducts(_class);
        }

        private void ChangeName(object sender, SelectionChangedEventArgs e)
        {
            if (isMaterial)
            {
                var _id = cbName.SelectedValue as int?;
                if (_id == null) return;
                using (var _db = new ModelContainer())
                {
                    var _locations = _db.MaterialStockSet.Where(i => i.StorageId == storageId && i.MaterialId == _id).ToList();
                    cbLocation.ItemsSource = _locations;
                }
            }
            else
            {
                var _id = cbName.SelectedValue as int?;
                if (_id == null) return;
                using (var _db = new ModelContainer())
                {
                    var _locations = _db.ProductStockSet.Where(i => i.StorageId == storageId && i.ProductId == _id).ToList();
                    cbLocation.ItemsSource = _locations;
                }
            }
        }

        private void SelectLocation(object sender, SelectionChangedEventArgs e)
        {
            var _id = cbLocation.SelectedValue as int?;
            if (_id == null) return;
            if (isMaterial)
            {
                var _locations = cbLocation.ItemsSource as List<MaterialStock>;
                var _storage = _locations.FirstOrDefault(i => i.Location == _id);
                if (_storage == null) return;
                storageQuantity = _storage.Quantity;
                txtStorage.Text = storageQuantity.ToString();
            }
            else
            {
                var _locations = cbLocation.ItemsSource as List<ProductStock>;
                var _storage = _locations.FirstOrDefault(i => i.Location == _id);
                if (_storage == null) return;
                storageQuantity = _storage.Quantity;
                txtStorage.Text = storageQuantity.ToString();
            }
        }
    }
}
