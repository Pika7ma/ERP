using System.Collections.Generic;
using System.Windows;

namespace Lplfw.UI.Produce
{
    /// <summary>
    /// ProduceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProduceWindow : Window
    {
        public ProduceWindow()
        {
            InitializeComponent();
            cbSearchRequisition.ItemsSource = RequisitionFields;
            cbSearchRequisition.SelectedValue = 0;
            cbSearchReturn.ItemsSource = RequisitionFields;
            cbSearchReturn.SelectedValue = 0;
            cbSearchProduction.ItemsSource = ProductionFields;
            cbSearchProduction.SelectedValue = 0;
            cbSearchQuality.ItemsSource = QualityFields;
            cbSearchQuality.SelectedValue = 0;
        }

        static public List<Utils.KeyValue> RequisitionFields = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { ID=0, Name="领料单号" },
            new Utils.KeyValue { ID=1, Name="流水线" },
            new Utils.KeyValue { ID=2, Name="状态" },
            new Utils.KeyValue { ID=3, Name="创建时间" },
            new Utils.KeyValue { ID=4, Name="完成时间" },
            new Utils.KeyValue { ID=5, Name="负责人" },
            new Utils.KeyValue { ID=6, Name="备注" }
        };

        static public List<Utils.KeyValue> ProductionFields = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { ID=0, Name="生产记录号" },
            new Utils.KeyValue { ID=1, Name="产品" },
            new Utils.KeyValue { ID=2, Name="流水线" },
            new Utils.KeyValue { ID=3, Name="领料单" },
            new Utils.KeyValue { ID=4, Name="状态" },
            new Utils.KeyValue { ID=5, Name="开始时间" },
            new Utils.KeyValue { ID=6, Name="预计结束时间" },
            new Utils.KeyValue { ID=7, Name="结束时间" },
            new Utils.KeyValue { ID=8, Name="负责人" },
            new Utils.KeyValue { ID=9, Name="备注" }
        };

        static public List<Utils.KeyValue> QualityFields = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { ID=0, Name="生产记录号" },
            new Utils.KeyValue { ID=1, Name="检查时间" },
            new Utils.KeyValue { ID=2, Name="负责人" },
            new Utils.KeyValue { ID=3, Name="备注" }
        };

        private void NewRequisition(object sender, RoutedEventArgs e)
        {
            var _win = new NewRequsition(isRequsition:true);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void NewReturn(object sender, RoutedEventArgs e)
        {
            var _win = new NewRequsition(isRequsition: false);
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void NewProduction(object sender, RoutedEventArgs e)
        {
            var _win = new NewProduction();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void NewQuality(object sender, RoutedEventArgs e)
        {
            var _win = new NewQuality();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }
    }
}
