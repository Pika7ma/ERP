using Lplfw.DAL;
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

namespace Lplfw.UI.Purchase
{
    /// <summary>
    /// NewPriceWithSupplierClass.xaml 的交互逻辑
    /// </summary>
    public partial class NewPriceWithSupplierClass : Window
    {
        private int sid;
        public NewPriceWithSupplierClass()
        {
            InitializeComponent();
        }

        public NewPriceWithSupplierClass(int id)
        {
            InitializeComponent();
            sid = id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var _db = new ModelContainer())
            {
                List<Material> _mlist = _db.MaterialSet.Select(i => i).ToList();
                cbMaterial.ItemsSource = _mlist;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _price = new MaterialPrice
                    {
                        MaterialId = (int)cbMaterial.SelectedValue,
                        SupplierId = sid,
                        Price = double.Parse(txtPrice.Text),
                        MaxQuantity = Int32.Parse(txtMaxQuantity.Text),
                        StartQuantity = Int32.Parse(txtStartQuantity.Text)

                    };
                    _db.MaterialPriceSet.Add(_price);
                    _db.SaveChanges();
                    DialogResult = true;
                    this.Close();
                }


            }
            catch (Exception)
            {
                MessageBox.Show("Wrong Operation!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _price = new MaterialPrice
                    {
                        MaterialId = (int)cbMaterial.SelectedValue,
                        SupplierId = sid,
                        Price = double.Parse(txtPrice.Text),
                        MaxQuantity = Int32.Parse(txtMaxQuantity.Text),
                        StartQuantity = Int32.Parse(txtStartQuantity.Text)

                    };
                    _db.MaterialPriceSet.Add(_price);
                    _db.SaveChanges();
                }
                cbMaterial.SelectedValue = null;
                txtPrice.Clear();
                txtStartQuantity.Clear();
                txtMaxQuantity.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong Operation!");
            }
        }
    }
}
