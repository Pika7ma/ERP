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
using System.Threading;
using System.Globalization;

namespace Lplfw.UI.Produce
{
    /// <summary>
    /// NewQuality.xaml 的交互逻辑
    /// </summary>
    public partial class NewQuality : Window
    {
        public NewQuality()
        {
            InitializeComponent();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            using (var db = new ModelContainer())
            {
                var _quality = new ProductionQuality
                {
                    ProductionId = int.Parse(txtProductionId.Text),
                    Time = (DateTime)dtTime.SelectedDate,
                    Result = txtResult.Text,
                    Description = txtDescription.Text,
                    UserId = Utils.CurrentUser.Id
                };
                db.ProductionQualitySet.Add(_quality);
                db.SaveChanges();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
