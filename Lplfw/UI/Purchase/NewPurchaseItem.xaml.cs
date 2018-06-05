using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lplfw.UI.Purchase
{
    /// <summary>
    /// NewPurchaseItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewPurchaseItem : Window
    {
        public delegate bool Getitem(PurchaseItemView purchaseItem);                              //委托，兴建订单时返回条目
        public delegate bool Changeitem(PurchaseItemView selecteditem, PurchaseItemView changeditem);                      //委托，修改订单条目时使用
        public Getitem getitem;
        public Changeitem changeitem;
        public int Purchaseid;
        public int MaterialId;
        public int SupplierId;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaterialId"></param>
        /// <param name="SupplierId"></param>
        /// <param name="Quantity"></param>
        /// <param name="Price"></param>
        public NewPurchaseItem(int MaterialId, int SupplierId, int Quantity, double Price)
        {
            this.SupplierId = SupplierId;
            this.MaterialId = MaterialId;
            InitializeComponent();
            var _thread = new Thread(new ThreadStart(Initiwindow1));
            _thread.Start();
            cbMaterial.SelectedValue = MaterialId;
            cbSupplier.SelectedValue = SupplierId;
            txq.Text = Convert.ToString(Quantity);
            txp.Text = Convert.ToString(Price);
        }
        /// <summary>
        /// 
        /// </summary>
        public NewPurchaseItem()
        {

            InitializeComponent();
            var _thread = new Thread(new ThreadStart(Initiwindow2));
            _thread.Start();

        }

        /// <summary>
        /// 
        /// </summary>
        private void Initiwindow1()
        {
            using (var _db = new DAL.ModelContainer())
            {
                List<Material> materials = _db.MaterialSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbMaterial.ItemsSource = materials;
                });
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void Initiwindow2()
        {
            using (var _db = new DAL.ModelContainer())
            {
                List<Material> materials = _db.MaterialSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbMaterial.ItemsSource = materials;
                });
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMaterial.SelectedValue == null) return;
            else
            {
                var _thread = new Thread(new ParameterizedThreadStart(GetPrice));
                _thread.Start((int)cbMaterial.SelectedValue);
               
            }
        }

        private void GetPrice(object i)
        {
            //var _mid = (int)i;
            //List<MaterialPriceView> _mpv= MaterialPriceView.GetMaterialPriceViewbymid(_mid);
            //Dispatcher.BeginInvoke((Action)delegate ()
            //{
            //    cbSupplier.ItemsSource = _mpv;
            //});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_click(object sender, EventArgs e)
        {
            if (txq.Text == "")
            {
                MessageBox.Show("请输入数量");
            }
            else
            {
                MaterialPriceView materialPriceView = (MaterialPriceView)cbSupplier.SelectedItem;
                if (materialPriceView == null)
                {
                    MessageBox.Show("请选择供货商");
                }
                else
                {
                    PurchaseItemView purchaseItem = new PurchaseItemView();
                    purchaseItem.PurchaseId = Purchaseid;
                    purchaseItem.MaterialId = materialPriceView.MaterialId;
                    purchaseItem.SupplierId = materialPriceView.SupplierId;
                    purchaseItem.Price = materialPriceView.Price;
                    purchaseItem.Quantity = Convert.ToInt32(txq.Text);
                    purchaseItem.MaterialName = materialPriceView.MaterialName;
                    purchaseItem.SupplierName = materialPriceView.SupplierName;
                    if (getitem != null)
                    {
                        if (getitem(purchaseItem))
                        {
                            MessageBox.Show("添加成功");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("条目重复");
                        }
                    }
                    else
                    {
                        var selecteditem = new PurchaseItemView();
                        selecteditem.MaterialId = MaterialId;
                        selecteditem.SupplierId = SupplierId;
                        if (changeitem(selecteditem, purchaseItem))
                        {
                            MessageBox.Show("修改成功");

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("条目重复");
                        }
                    }

                }
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {

            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("[^0-9.-]+");

            e.Handled = re.IsMatch(e.Text);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Concel_click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txq_lostfocus(object sender, RoutedEventArgs e)
        {
            var _mprice = (MaterialPriceView)cbSupplier.SelectedItem;
            if (_mprice == null) return;
            else
            {
                if (txq.Text == "") return;
                else
                {
                    if (Convert.ToInt32(txq.Text) < _mprice.StartQuantity || Convert.ToInt32(txq.Text) > _mprice.MaxQuantity)
                    {
                        MessageBox.Show("数量不符合要求,应该在" + _mprice.StartQuantity + "----" + _mprice.MaxQuantity);
                        txq.Text = "";

                    }
                }
            }
        }

        private void cbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MaterialPriceView materialPriceView = (MaterialPriceView)cbSupplier.SelectedItem;
            if (materialPriceView != null)
            {
                txp.Text = materialPriceView.Price.ToString("G");
            }
        }

    }
}

