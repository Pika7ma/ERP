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
    /// NewSupplierClass.xaml 的交互逻辑
    /// </summary>
    public partial class NewSupplierClass : Window
    {
        public NewSupplierClass()
        {
            InitializeComponent();
        }

        private void ButtonOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _supplier = new Supplier
                    {
                        Name = txtName.Text,
                        Contact = txtContact.Text,
                        Location = txtLocation.Text,
                        Tel = txtTel.Text
                    };
                    _db.SupplierSet.Add(_supplier);
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

        private void ButtonContinueClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _supplier = new Supplier
                    {
                        Name = txtName.Text,
                        Contact = txtContact.Text,
                        Location = txtLocation.Text,
                        Tel = txtTel.Text
                    };
                    _db.SupplierSet.Add(_supplier);
                    _db.SaveChanges();
                    DialogResult = true;
                    this.Close();
                }
                txtName.Clear();
                txtContact.Clear();
                txtLocation.Clear();
                txtTel.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong Operation!");
            }
        }
    }
}
