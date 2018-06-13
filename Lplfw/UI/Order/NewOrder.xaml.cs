using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Lplfw.UI.Order
{
    /// <summary>
    /// NewOrder.xaml 的交互逻辑
    /// </summary>
    public partial class NewOrder : Window
    {
        public List<Product> Products { get; set; }
        private SalesViewModel sales;
        private List<Utils.KeyValue> PriorityList = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { Id = 0, Name= "普通" },
            new Utils.KeyValue { Id = 0, Name= "高" },
        };

        public NewOrder()
        {
            InitializeComponent();
            sales = new SalesViewModel(Utils.CurrentUser.Id);
            SetControls();
        }

        private void SetControls()
        {
            DataContext = this;
            txtCode.Binding(sales, "TxtCode");
            txtCustomer.Binding(sales, "TxtCustomer");
            txtTel.Binding(sales, "TxtTel");
            txtDescription.Binding(sales, "TxtDescription");
            cbPriority.Binding(sales, "CbPriority");
            dpDueTime.Binding(sales, "DpDueTime");
            dpDueTime.Value = DateTime.Now;
            dgSalesItems.ItemsSource = sales.Items;
            cbPriority.ItemsSource = PriorityList;
            cbPriority.SelectedIndex = 0;
            new Thread(new ThreadStart(SetControlsThread)).Start();
        }

        private void SetControlsThread()
        {
            using (var _db = new ModelContainer())
            {
                Products = _db.ProductSet.ToList();
            }
        }

        private void NewSalesItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewOrderItem();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                var _item = Utils.TempObject as SalesItem;
                if (_item == null) return;
                Utils.TempObject = null;
                sales.AddItem(_item);
                dgSalesItems.Items.Refresh();
            }
        }

        private void EditSalesItem(object sender, RoutedEventArgs e)
        {
            var _item = dgSalesItems.SelectedItem as SalesItem;
            if (_item == null) return;
            var _win = new NewOrderItem(_item);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                var _new = Utils.TempObject as SalesItem;
                if (_new == null) return;
                Utils.TempObject = null;
                if(sales.EditItem(_new)) dgSalesItems.Items.Refresh();
            }
        }

        private void DeleteSalesItem(object sender, RoutedEventArgs e)
        {
            var _item = dgSalesItems.SelectedItem as SalesItem;
            if (_item == null) return;
            if (sales.DeleteItem(_item)) dgSalesItems.Items.Refresh();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (sales.CanSubmit)
            {
                var _rtn = sales.CreateNew();
                if (_rtn == true) {
                    MessageBox.Show("创建订单成功");
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("创建订单失败");
                }
            } else
            {
                txtMessage.Text = sales.TxtCheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
