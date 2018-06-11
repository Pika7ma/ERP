using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Lplfw.DAL;
using System.Threading;

namespace Lplfw.UI.Order
{
    /// <summary>
    /// NewOrderItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewOrderItem : Window
    {
        private SalesItemViewModel item;
        private bool isNew;

        public NewOrderItem()
        {
            InitializeComponent();
            Title = "新建订单项目";
            item = new SalesItemViewModel();
            isNew = true;
            SetControls();
            btnConfirm.Click += SubmitNewItem;
        }

        public NewOrderItem(SalesItem item)
        {
            InitializeComponent();
            Title = "编辑订单项目";
            this.item = new SalesItemViewModel(item);
            isNew = false;
            SetControls();
            btnConfirm.Click += SubmitEditItem;
        }

        private void SetControls()
        {
            if (!isNew)
            {
                var _id = item.Object.ProductId;
                using (var _db = new ModelContainer())
                {
                    var _product = _db.ProductSet.FirstOrDefault(i => i.Id == _id);
                    if (_product == null) return;
                    var _classes = _db.ProductClassSet.ToList();
                    cbClass.ItemsSource = _classes;
                    cbClass.SelectedValue = _product.ClassId;
                    var _products = _db.ProductSet.Where(i => i.ClassId == _product.ClassId).ToList();
                    cbProduct.ItemsSource = _products;
                    cbProduct.SelectedValue = _product.Id;
                    txtOriginPrice.Text = _product.Price.ToString();
                }
                cbClass.IsEnabled = false;
                cbProduct.IsEnabled = false;
            }
            cbProduct.Binding(item, "CbProduct");
            txtPrice.Binding(item, "TxtPrice");
            txtQuantity.Binding(item, "TxtQuantity");
            new Thread(new ThreadStart(SetControlsThread)).Start();
        }

        private void SetControlsThread()
        {
            var _classes = ProductClass.GetAllClasses(false);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                cbClass.ItemsSource = _classes;
            });
        }

        private void SelectProductClass(object sender, SelectionChangedEventArgs e)
        {
            var _id = cbClass.SelectedValue as int?;
            if (_id == null) return;
            var _products = ProductClass.GetSubClassProducts(_id);
            cbProduct.ItemsSource = _products;
        }

        private void SelectProduct(object sender, SelectionChangedEventArgs e)
        {
            var _product = cbProduct.SelectedItem as Product;
            if (_product == null) return;
            txtOriginPrice.Text = _product.Price.ToString();
        }

        private void SubmitNewItem(object sender, RoutedEventArgs e)
        {
            if (item.CanSubmit) {
                Utils.TempObject = item.Object;
                DialogResult = true;
            }
            else
            {
                txtMessage.Text = item.TxtCheckMessage;
            }
        }

        private void SubmitEditItem(object sender, RoutedEventArgs e)
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
