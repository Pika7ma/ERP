using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lplfw.DAL;

namespace Lplfw.UI.Inventory
{
    /// <summary>
    /// EditLocation.xaml 的交互逻辑
    /// </summary>
    public partial class EditLocation : Window
    {
        private int id;
        private int storageId;

        public EditLocation(MaterialStockView material)
        {
            InitializeComponent();
            txtLocation.Text = material.Location;
            id = material.MaterialId;
            storageId = material.StorageId;
            btnConfirm.Click += SubmitEditMaterialLocation;
        }

        public EditLocation(ProductStockView product)
        {
            InitializeComponent();
            txtLocation.Text = product.Location;
            id = product.ProductId;
            storageId = product.StorageId;
            btnConfirm.Click += SubmitEditProductLocation;
        }

        private bool CanSubmit()
        {
            if (txtLocation.Text == null || txtLocation.Text == "")
            {
                txtMessage.Text = "请填写正确的货位信息";
                return false;
            }
            return true;
        }

        private void SubmitEditMaterialLocation(object sender, RoutedEventArgs e)
        {
            if (CanSubmit())
            {
                using (var _db = new ModelContainer())
                {
                    var _item = _db.MaterialStockSet.FirstOrDefault(i => i.StorageId == storageId && i.MaterialId == id);
                    if (_item == null) return;
                    _item.Location = txtLocation.Text;
                    _db.SaveChanges();
                    Utils.TempObject = txtLocation.Text;
                    DialogResult = true;
                }
            }   
        }

        private void SubmitEditProductLocation(object sender, RoutedEventArgs e)
        {
            if (CanSubmit())
            {
                using (var _db = new ModelContainer())
                {
                    var _item = _db.ProductStockSet.FirstOrDefault(i => i.StorageId == storageId && i.ProductId == id);
                    if (_item == null) return;
                    _item.Location = txtLocation.Text;
                    _db.SaveChanges();
                    Utils.TempObject = txtLocation.Text;
                    DialogResult = true;
                }
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
