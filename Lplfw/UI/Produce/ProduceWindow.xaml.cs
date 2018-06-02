using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            var _thread = new Thread(new ThreadStart(Refresh));
            _thread.Start();

        }



        #region 权限控制

        private void Refresh()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 4 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
                if (_temp.Mode == "只读")
                {
                    Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        OnlyRead();
                    });
                }
            }
        }
        /// <summary>
        /// 只读
        /// </summary>
        private void OnlyRead()
        {
            btnFinishProduce.Visibility = Visibility.Hidden;
            btnNewCheck.Visibility = Visibility.Hidden;
            btnNewProduceOrder.Visibility = Visibility.Hidden;
            btnNewRequisition.Visibility = Visibility.Hidden;
            btnNewReturn.Visibility = Visibility.Hidden;
        }


        #endregion




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
            var _win = new NewRequsition(isRequsition: true);
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
