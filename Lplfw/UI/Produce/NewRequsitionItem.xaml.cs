using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading;
using Lplfw.DAL;

namespace Lplfw.UI.Produce
{
    public delegate void TransfDelegate(String value1, int value2);
    /// <summary>
    /// NewRequsitionItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewRequsitionItem : Window
    {
        public event TransfDelegate TransfEvent;
        public NewRequsitionItem(bool isNew)
        {
            InitializeComponent();
            using (var _db = new ModelContainer())
            {
                cbMaterialClass.ItemsSource = _db.MaterialClassSet.ToList();
            }

            if (isNew)
            {
                Title = "新建物料单条目";
            }
            else
            {
                Title = "修改物料单条目";
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            string materialname = cbMaterial.Text;
            int amount;
            Int32.TryParse(amountText.Text, out amount);
            TransfEvent(materialname, amount);
            this.DialogResult = true;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void MaterialClassChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var _class = cbMaterialClass.SelectedValue as int?;
            if (_class != null)
            {
                using (var _db = new ModelContainer())
                {
                    cbMaterial.ItemsSource = _db.MaterialSet.Where(i => i.ClassId == _class).ToList();
                }
            }
        }
    }
}
