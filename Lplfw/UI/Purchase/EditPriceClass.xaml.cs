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
    /// EditPriceClass.xaml 的交互逻辑
    /// </summary>
    public partial class EditPriceClass : Window
    {
        private int mid;
        private int sid;

        public EditPriceClass()
        {
            InitializeComponent();
        }

        public EditPriceClass(int mId, int sId)
        {
            InitializeComponent();
            mid = mId;
            sid = sId;
        }

        /// <summary>
        /// 提交修改项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _price = _db.MaterialPriceSet.FirstOrDefault(i => i.MaterialId == mid && i.SupplierId == sid);
                    _price.Price = double.Parse(txtPrice.Text);
                    _price.StartQuantity = Int32.Parse(txtSquantity.Text);
                    _price.MaxQuantity = Int32.Parse(txtMquantity.Text);
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
