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
    /// NewPriceClass.xaml 的交互逻辑
    /// </summary>
    public partial class NewPriceClass : Window
    {
        bool ifHandeled = false;
        public NewPriceClass()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口初始化，加载组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var _thread = new Thread(new ThreadStart(WLthread));
            _thread.Start();
        }

        private void WLthread()
        {
            using (var _db = new ModelContainer())
            {
                List<Material> _mlist = _db.MaterialSet.Select(i => i).ToList();
                List<Supplier> _slist = _db.SupplierSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbMaterial.ItemsSource = _mlist;
                    cbSupplier.ItemsSource = _slist;
                });
            }
        }

        /// <summary>
        /// 提交新建项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var _db = new ModelContainer())
            {
                var _price = new MaterialPrice
                {
                    MaterialId = (int)cbMaterial.SelectedValue,
                    SupplierId = (int)cbSupplier.SelectedValue,
                    Price = double.Parse(txtPrice.Text),
                    MaxQuantity = Int32.Parse(txtMquantity.Text),
                    StartQuantity = Int32.Parse(txtSquantity.Text)

                };
                _db.MaterialPriceSet.Add(_price);
                _db.SaveChanges();
            }
            ifHandeled = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var _price = new MaterialPrice
            {
                MaterialId = (int)cbMaterial.SelectedValue,
                SupplierId = (int)cbSupplier.SelectedValue,
                Price = double.Parse(txtPrice.Text),
                MaxQuantity = Int32.Parse(txtMquantity.Text),
                StartQuantity = Int32.Parse(txtSquantity.Text)

            };
            var _thread = new Thread(new ParameterizedThreadStart(BC1thread));
            _thread.Start(_price);
            ifHandeled = true;
        }

        private void BC1thread(object price)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    _db.MaterialPriceSet.Add((MaterialPrice)price);
                    _db.SaveChanges();
                }
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbMaterial.SelectedValue = null;
                    cbSupplier.SelectedValue = null;
                    txtPrice.Clear();
                    txtSquantity.Clear();
                    txtMquantity.Clear();
                });
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong Operation!");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ifHandeled;
        }
    }
}
