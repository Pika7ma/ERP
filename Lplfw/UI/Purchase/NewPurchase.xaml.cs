using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Lplfw.UI.Purchase
{
    /// <summary>
    /// NewPurchase.xaml 的交互逻辑
    /// </summary>
    public partial class NewPurchase : Window
    {
        public List<Material> Materials { get; set; }
        public List<Supplier> Suppliers { get; set; }

        private DAL.Purchase purchase;
        private List<PurchaseItem> items;
        private List<Utils.KeyValue> priorityList = new List<Utils.KeyValue>
        {
            new Utils.KeyValue{ Id=0, Name="普通" },
            new Utils.KeyValue{ Id=1, Name="高" },
        };

        public NewPurchase()
        {
            InitializeComponent();
            DataContext = this;
            purchase = new DAL.Purchase
            {
                Status = "处理中",
                Priority = "普通",
                FinishedAt = null,
                UserId = Utils.CurrentUser.Id
            };
            items = new List<PurchaseItem>();
            dgItems.ItemsSource = items;
            SetControls();
        }

        private void SetControls()
        {
            cbProirity.ItemsSource = priorityList;
            cbProirity.SelectedIndex = 0;
            new Thread(new ThreadStart(SetControlsThread)).Start();
        }

        private void SetControlsThread()
        {
            using (var _db = new ModelContainer())
            {
                Materials = _db.MaterialSet.ToList();
                Suppliers = _db.SupplierSet.ToList();
            }
        }

        private void NewItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewPurchaseItem();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                var _item = Utils.TempObject as PurchaseItem;
                var _old = items.FirstOrDefault(i => i.MaterialId == _item.MaterialId && i.SupplierId == _item.SupplierId);
                if (_old == null)
                {
                    items.Add(_item);
                }
                else
                {
                    _old.Quantity += _item.Quantity;
                }
                Utils.TempObject = null;
                dgItems.Items.Refresh();
            }
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            var _item = dgItems.SelectedItem as PurchaseItem;
            if (_item == null) return;
            items.Remove(_item);
        }

        private bool CanSubmit()
        {
            if(items.Count == 0)
            {
                txtMessage.Text = "请填写采购条目";
                return false;
            }
            return true;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    purchase.CreateAt = DateTime.Now;
                    purchase.Description = txtDescription.Text;
                    _db.PurchaseSet.Add(purchase);
                    _db.SaveChanges();

                    for (var _i = 0; _i < items.Count; _i++)
                    {
                        items[_i].PurchaseId = purchase.Id;
                    }
                    _db.PurchaseItemSet.AddRange(items);
                    _db.SaveChanges();
                    DialogResult = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("创建采购单失败!", null, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
