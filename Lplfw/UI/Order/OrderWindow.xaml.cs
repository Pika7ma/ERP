using System.Collections.Generic;
using System.Windows;

namespace Lplfw.UI.Order
{
    /// <summary>
    /// OrderWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
            cbSearchSales.ItemsSource = SalesFields;
            cbSearchSales.SelectedValue = 0;
        }

        static public List<Utils.KeyValue> SalesFields = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { ID=0, Name="状态" },
            new Utils.KeyValue { ID=1, Name="订单号" },
            new Utils.KeyValue { ID=2, Name="客户" },
            new Utils.KeyValue { ID=3, Name="订货时间" },
            new Utils.KeyValue { ID=4, Name="应付时间" },
            new Utils.KeyValue { ID=5, Name="完成时间" },
            new Utils.KeyValue { ID=6, Name="优先级" },
            new Utils.KeyValue { ID=7, Name="负责人" },
        };

        private void NewSales(object sender, RoutedEventArgs e)
        {
            var _win = new NewOrder();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }
    }
}
