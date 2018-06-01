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
    /// EditSupplierClass.xaml 的交互逻辑
    /// </summary>
    public partial class EditSupplierClass : Window
    {
        private int id;

        public EditSupplierClass()
        {
            InitializeComponent();
        }

        public EditSupplierClass(int Id)
        {
            InitializeComponent();
            id = Id;
        }

        private void ButtonOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _supplier = _db.SupplierSet.FirstOrDefault(i => i.Id == id);
                    _supplier.Name = txtName.Text;
                    _supplier.Contact = txtContact.Text;
                    _supplier.Location = txtLocation.Text;
                    _supplier.Tel = txtTel.Text;
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
    }
}
