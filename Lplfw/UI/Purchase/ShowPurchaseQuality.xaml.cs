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
    /// ShowPurchaseQuality.xaml 的交互逻辑
    /// </summary>
    public partial class ShowPurchaseQuality : Window
    {
        public ShowPurchaseQuality()
        {
            InitializeComponent();
            var _thread = new Thread(new ThreadStart(ShowAll));
            _thread.Start();
        }

       public void ShowAll()
        {
            using(var _db=new DAL.ModelContainer())
            {
                List<PurchaseQuality> _pqs = _db.PurchaseQualitySet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    dgQuality.ItemsSource = _pqs;
                });
            }
        }
    }
}
