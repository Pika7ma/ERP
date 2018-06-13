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
using Lplfw.DAL;

namespace Lplfw.UI.Inventory
{
    /// <summary>
    /// EditSafeLocation.xaml 的交互逻辑
    /// </summary>
    public partial class EditSafeQuantity : Window
    {
        private int id;

        public EditSafeQuantity(Material material)
        {
            InitializeComponent();
            id = material.Id;
            txtName.Text = material.Name;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (CanSubmit())
            {
                using (var _db = new ModelContainer())
                {
                    var _material = _db.MaterialSet.FirstOrDefault(i => i.Id == id);
                    if (_material == null) return;
                    _material.SafeQuantity = int.Parse(txtQuantity.Text);
                    _db.SaveChanges();
                    DialogResult = true;
                }
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool CanSubmit()
        {
            if (int.TryParse(txtQuantity.Text, out int _temp) && _temp >= 0)
            {
                return true;
            }
            else
            {
                txtMessage.Text = "请输入正确的数量";
                return false;
            }
        }
    }
}
