using Lplfw.DAL;
using System;
using System.Linq;
using System.Windows;

namespace Lplfw.UI.Purchase
{
    /// <summary>
    /// NewPurchaseQuality.xaml 的交互逻辑
    /// </summary>
    public partial class NewPurchaseQuality : Window
    {
        public NewPurchaseQuality(int id)
        {
            InitializeComponent();
            txtPid.Text = Convert.ToString(id);
            using (var _db = new DAL.ModelContainer())
            {
                cbUid.ItemsSource = _db.UserSet.Select(i => i);
            }
            txtTime.Text = Convert.ToString(DateTime.Now);
        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ok_click(object sender, RoutedEventArgs e)
        {
            if (txResult.Text != "" && txtDsc.Text != "" && cbUid.Text != "")
            {
                var _quality = new PurchaseQuality();
                _quality.PurchaseId = Convert.ToInt32(txtPid.Text);
                _quality.Result = txResult.Text;
                _quality.Time = Convert.ToDateTime(txtTime.Text);
                _quality.UserId = (int)cbUid.SelectedValue;
                _quality.Description = txtDsc.Text;
                using (var _db = new DAL.ModelContainer())
                {
                    _db.PurchaseQualitySet.Add(_quality);
                    _db.SaveChanges();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("请补完信息");
            }
        }
    }
}
