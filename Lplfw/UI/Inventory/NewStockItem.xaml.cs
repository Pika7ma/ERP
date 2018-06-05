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
        private bool isNew;
        private bool isMaterial;
        private bool isIn;
        private object obj;

        public NewStockItem(bool isMaterial, bool isIn)
        {
            InitializeComponent();
            Title = "新建条目";
            isNew = true;
            this.isMaterial = isMaterial;
            this.isIn = isIn;
            SetControls();
        }

        public NewStockItem(Object edit, bool isMaterial, bool isIn)
        {
            InitializeComponent();
            Title = "修改条目";
            isNew = false;
            this.isMaterial = isMaterial;
            this.isIn = isIn;
            obj = edit;
            SetControls();
        }

        private void SetControls()
        {
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
            if (!isNew)
            {
                using (var _db = new ModelContainer())
                {
                    if (isMaterial)
                    {
                        int? _id;
                        string _quantity;
                        string _location;
                        if (isIn)
                        {
                            var _obj = obj as MaterialStockInItem;
                            _id = _obj.MaterialId;
                            _quantity = _obj.Quantity.ToString();
                            _location = _obj.Location;
                        }
                        else
                        {
                            var _obj = obj as MaterialStockOutItem;
                            _id = _obj.MaterialId;
                            _quantity = _obj.Quantity.ToString();
                            _location = _obj.Location;
                        }
                        var _material = _db.MaterialSet.FirstOrDefault(i => i.Id == _id);
                        if (_material == null) return;
                        cbClass.SelectedValue = _material.ClassId;
                        cbName.ItemsSource = _db.MaterialSet.Where(i => i.ClassId == _material.ClassId).ToList();
                        cbName.SelectedValue = _material.Id;
                        txtQuantity.Text = _quantity;
                        txtLocation.Text = _location;
                    }
                    else
                    {
                        int? _id;
                        string _quantity;
                        string _location;
                        if (isIn)
                        {
                            var _obj = obj as ProductStockInItem;
                            _id = _obj.ProductId;
                            _quantity = _obj.Quantity.ToString();
                            _location = _obj.Location;
                        }
                        else
                        {
                            var _obj = obj as ProductStockOutItem;
                            _id = _obj.ProductId;
                            _quantity = _obj.Quantity.ToString();
                            _location = _obj.Location;
                        }
                        var _product = _db.ProductSet.FirstOrDefault(i => i.Id == _id);
                        if (_product == null) return;
                        cbClass.SelectedValue = _product.ClassId;
                        cbName.ItemsSource = _db.ProductSet.Where(i => i.ClassId == _product.ClassId).ToList();
                        cbName.SelectedValue = _product.Id;
                        txtQuantity.Text = _quantity;
                        txtLocation.Text = _location;
                    }
                }
            }
        }

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
            if (int.TryParse(txtQuantity.Text, out int _quantity) == false)
            {
                txtMessage.Text = "请输入正确的数量";
                return false;
            }
            var _location = txtLocation.Text;
            if (_location == null || _location == "")
            {
                txtMessage.Text = "请输入正确的货位";
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
                    Location = txtLocation.Text
                };
                Utils.TempObject = _item;
            }
            else
            {
                var _item = new MaterialStockOutItem
                {
                    MaterialId = (int)cbName.SelectedValue,
                    Quantity = int.Parse(txtQuantity.Text),
                    Location = txtLocation.Text
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
                    Location = txtLocation.Text
                };
                Utils.TempObject = _item;
            }
            else
            {
                var _item = new ProductStockOutItem
                {
                    ProductId = (int)cbName.SelectedValue,
                    Quantity = int.Parse(txtQuantity.Text),
                    Location = txtLocation.Text
                };
                Utils.TempObject = _item;
            }
            DialogResult = true;
        }

        private void ChangeClass(object sender, SelectionChangedEventArgs e)
        {
            var _class = cbClass.SelectedValue as int?;
            if (_class == null) return;
            using (var _db = new ModelContainer())
            {
                if (isMaterial)
                    cbName.ItemsSource = _db.MaterialSet.Where(i => i.ClassId == _class).ToList();
                else
                    cbName.ItemsSource = _db.ProductSet.Where(i => i.ClassId == _class).ToList();
            }
        }
    }
}
