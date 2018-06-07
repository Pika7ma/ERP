using Lplfw.DAL;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Lplfw.UI.Purchase
{
    /// <summary>
    /// NewPurchaseItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewPurchaseItem : Window
    {
        private PurchaseItemViewModel item;

        public NewPurchaseItem()
        {
            InitializeComponent();
            SetControls();
            item = new PurchaseItemViewModel();
            cbMaterial.Binding(item, "CbMaterial");
            cbSupplier.Binding(item, "CbSupplier");
            txtQuantity.Binding(item, "TxtQuantity");
        }

        private void SetControls()
        {
            new Thread(new ThreadStart(SetControlsThread)).Start();
        }

        private void SetControlsThread()
        {
            var _list = MaterialClass.GetAllClasses(false);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                cbMaterialClass.ItemsSource = _list;
            });
        }

        private void SelectMaterialClass(object sender, SelectionChangedEventArgs e)
        {
            var _class = cbMaterialClass.SelectedValue as int?;
            if (_class == null)
            {
                cbMaterial.ItemsSource = null;
                return;
            }
            cbMaterial.ItemsSource = MaterialClass.GetSubClassMaterials(_class);
        }

        private void SelectMaterial(object sender, SelectionChangedEventArgs e)
        {
            var _material = cbMaterial.SelectedValue as int?;
            if (_material == null)
            {
                cbSupplier.ItemsSource = null;
                return;
            }
            else
            {
                cbSupplier.ItemsSource = MaterialPriceView.GetBySupplierId((int)_material);
            }
        }

        private void SelectSupplier(object sender, SelectionChangedEventArgs e)
        {
            var _supplier = cbSupplier.SelectedValue as int?;
            if (_supplier == null)
            {
                txtPrice.Text = null;
                txtStartQuantity.Text = null;
                txtMaxQuantity.Text = null;
                item.StartQuantity = 0;
                item.MaxQuantity = 0;
                item.Price = 0;
                return;
            }
            else
            {
                var _material = cbMaterial.SelectedValue as int?;
                using (var _db = new ModelContainer())
                {
                    var _price = _db.MaterialPriceSet.FirstOrDefault(i => i.MaterialId == _material && i.SupplierId == _supplier);
                    if (_price == null)
                    {
                        txtPrice.Text = null;
                        txtStartQuantity.Text = null;
                        txtMaxQuantity.Text = null;
                        item.StartQuantity = 0;
                        item.MaxQuantity = 0;
                        item.Price = 0;
                        return;
                    }
                    else
                    {
                        txtPrice.Text = _price.Price.ToString();
                        txtStartQuantity.Text = _price.StartQuantity.ToString();
                        txtMaxQuantity.Text = _price.MaxQuantity.ToString();
                        item.StartQuantity = _price.StartQuantity;
                        item.MaxQuantity = _price.MaxQuantity;
                        item.Price = _price.Price;
                        return;
                    }
                }
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (item.CanSubmit)
            {
                Utils.TempObject = item.Object;
                DialogResult = true;
            }
            else
            {
                txtMessage.Text = item.TxtCheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

