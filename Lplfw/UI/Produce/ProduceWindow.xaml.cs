using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Lplfw.DAL;
using System.Linq;
using System;
using System.Data;
using System.Threading;

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
            ShowUnreceivedOrder();
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
        /// <summary>
        /// 物料确认界面待处理领料单显示相关信息
        /// </summary>
        public void ShowUnreceivedOrder()
        {
            var list = new List<Lplfw.UI.Produce.RequistionProduce>();
            using (var db = new ModelContainer())
            {
                var _unreceivedreqlist = db.RequisitionSet.ToList();
                foreach (var _order in _unreceivedreqlist)
                {
                    if (_order.Status == "unreceived")
                    {
                        var _myorder = new RequistionProduce();
                        _myorder.Id = _order.Id;
                        _myorder.AssemblyLineId = _order.AssemblyLineId;
                        _myorder.Status = _order.Status;
                        _myorder.CreatedAt = _order.CreateAt.ToString();
                        _myorder.FinishedAt = _order.FinishedAt.ToString();
                        _myorder.UserId = _order.UserId;
                        _myorder.Description = _order.Description;
                        list.Add(_myorder);
                    }
                }
            }
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    orderGrid.ItemsSource = null;
                    orderGrid.ItemsSource = list;
                }
                ));
        }

        private void NewRequisition(object sender, RoutedEventArgs e)
        {
            var _win = new NewRequsition(isRequsition: true) { type = true };
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }

        private void NewReturn(object sender, RoutedEventArgs e)
        {
            var _win = new NewRequsition(isRequsition: false) { type = false };
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
        /// <summary>
        /// 单击物料确认界面待处理领料单中任意一行，需求清单虚拟库存显示相关信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(orderGrid_SelectionChanged));
            mythread.Start(1);
        }
        private void orderGrid_SelectionChanged(object id)
        {
            RequistionProduce item = new RequistionProduce();
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    if (orderGrid.SelectedItems.Count > 0)
                    {
                        item = (RequistionProduce)orderGrid.SelectedItems[0];
                    }
                }));
            if (item.Status != null)
            {
                int requistionid = item.Id;
                var list = new List<Lplfw.UI.Produce.MaterialProduce>();
                var stocklist = new List<Lplfw.UI.Produce.MaterialProduce>();
                using (var db = new ModelContainer())
                {
                    var _requisitionitemlist = db.RequisitionItemSet.ToList();
                    var _materiallist = db.MaterialSet.ToList();
                    var _materialstocklist = db.MaterialStockSet.ToList();
                    foreach (var _requisitionitem in _requisitionitemlist)
                    {
                        if (_requisitionitem.RequisitionId == requistionid)
                        {
                            MaterialProduce _orderitem = new MaterialProduce();
                            MaterialProduce _stockitem = new MaterialProduce();
                            var _material = db.MaterialSet.Where(i => i.Id == _requisitionitem.MaterialId).ToList();
                            var _stock = db.MaterialStockSet.Where(i => i.MaterialId == _requisitionitem.MaterialId).ToList();
                            _orderitem.MaterialName = _material.First().Name;
                            _stockitem.MaterialName = _material.First().Name;
                            _orderitem.Quantity = _requisitionitem.Quantity;
                            _stockitem.Quantity = _stock.First().Quantity;
                            _orderitem.Unit = _material.First().Unit;
                            _stockitem.Unit = _material.First().Unit;
                            list.Add(_orderitem);
                            stocklist.Add(_stockitem);
                        }
                    }
                }
                this.requistionGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    requistionGrid.ItemsSource = list;
                }
                ));
                this.stockGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    stockGrid.ItemsSource = stocklist;
                }
                ));
            }
        }
        /// <summary>
        /// 绑定物料确认界面确定领料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeRequistion(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(ChangeRequistionStatus));
            mythread.Start(1);
        }
        /// <summary>
        /// 确定领料具体操作
        /// </summary>
        /// <param name="id"></param>
        private void ChangeRequistionStatus(object id)
        {
            RequistionProduce item = new RequistionProduce();
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    if (orderGrid.SelectedItems.Count > 0)
                    {
                        item = (RequistionProduce)orderGrid.SelectedItems[0];
                    }
                }));
            if (item.Status != null)
            {
                using (var db = new ModelContainer())
                {
                    Requisition requistionitem = db.RequisitionSet.First(i => i.Id == item.Id);
                    requistionitem.Status = "Recieved";
                    db.SaveChanges();
                }
                ShowUnreceivedOrder();
                this.requistionGrid.Dispatcher.Invoke(new Action(
                    delegate
                    {
                        requistionGrid.ItemsSource = null;
                    }
                    ));
                this.stockGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    stockGrid.ItemsSource = null;
                }
                ));
            }
        }
        /// <summary>
        /// 绑定领料单管理的显示所有记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetAllRequistion(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(ShowAllRequistion));
            mythread.Start(1);
        }
        /// <summary>
        /// 领料单管理的显示所有记录具体操作
        /// </summary>
        /// <param name="id"></param>
        private void ShowAllRequistion(object id)
        {
            var list = new List<Lplfw.UI.Produce.RequistionProduce>();
            using (var db = new ModelContainer())
            {
                var _requistionList = db.RequisitionSet.ToList();
                foreach (var _order in _requistionList)
                {
                    var _myorder = new RequistionProduce();
                    _myorder.Id = _order.Id;
                    _myorder.AssemblyLineId = _order.AssemblyLineId;
                    _myorder.Status = _order.Status;
                    _myorder.CreatedAt = _order.CreateAt.ToString();
                    _myorder.FinishedAt = _order.FinishedAt.ToString();
                    _myorder.UserId = _order.UserId;
                    _myorder.Description = _order.Description;
                    list.Add(_myorder);
                }
            }
            this.allrequistionGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    allrequistionGrid.ItemsSource = null;
                    allrequistionGrid.ItemsSource = list;
                }
                ));
        }

        /// <summary>
        /// 单击领料单管理中左边大方框中任意一行时右边方框显示相关信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allrequistionGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(allrequistionGrid_SelectionChanged));
            mythread.Start(1);
        }
        private void allrequistionGrid_SelectionChanged(object id)
        {
            RequistionProduce item = new RequistionProduce();
            this.allrequistionGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    if (allrequistionGrid.SelectedItems.Count > 0)
                    {
                        item = (RequistionProduce)allrequistionGrid.SelectedItems[0];
                    }
                }));
            if (item.Status != null)
            {
                int requistionid = item.Id;
                var list = new List<Lplfw.UI.Produce.MaterialProduce>();
                using (var db = new ModelContainer())
                {
                    var _requisitionitemlist = db.RequisitionItemSet.ToList();
                    var _materiallist = db.MaterialSet.ToList();
                    foreach (var _requisitionitem in _requisitionitemlist)
                    {
                        if (_requisitionitem.RequisitionId == requistionid)
                        {
                            MaterialProduce _orderitem = new MaterialProduce();
                            var _material = db.MaterialSet.Where(i => i.Id == _requisitionitem.MaterialId).ToList();
                            _orderitem.MaterialName = _material.First().Name;
                            _orderitem.Quantity = _requisitionitem.Quantity;
                            _orderitem.Unit = _material.First().Unit;
                            list.Add(_orderitem);
                        }
                    }
                }
                this.requistionmaterialGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    requistionmaterialGrid.ItemsSource = list;
                }
                ));
            }
        }
        /// <summary>
        /// 绑定还料单管理显示所有记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetAllReturn(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(ShowAllReturn));
            mythread.Start(1);
        }
        /// <summary>
        /// 还料单管理显示所有记录具体操作
        /// </summary>
        /// <param name="id"></param>
        private void ShowAllReturn(object id)
        {
            var list = new List<Lplfw.UI.Produce.RequistionProduce>();
            using (var db = new ModelContainer())
            {
                var _returnList = db.ReturnSet.ToList();
                foreach (var _order in _returnList)
                {
                    var _myorder = new RequistionProduce();
                    _myorder.Id = _order.Id;
                    var assem = db.RequisitionSet.Where(i => i.Id == _order.RequisitionId).ToList();
                    _myorder.AssemblyLineId = assem.First().AssemblyLineId;
                    _myorder.Status = _order.Status;
                    _myorder.CreatedAt = _order.CreateAt.ToString();
                    _myorder.FinishedAt = _order.FinishedAt.ToString();
                    _myorder.UserId = _order.UserId;
                    _myorder.Description = _order.Description;
                    list.Add(_myorder);
                }
            }
            this.allreturnGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    allreturnGrid.ItemsSource = null;
                    allreturnGrid.ItemsSource = list;
                }
                ));
        }
        /// <summary>
        /// 选中allreturnGrid某一行，右边显示相应信息，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allreturnGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(allreturnGrid_SelectionChanged));
            mythread.Start(1);
        }
        private void allreturnGrid_SelectionChanged(object id)
        {
            RequistionProduce item = new RequistionProduce();
            this.allreturnGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    if (allreturnGrid.SelectedItems.Count > 0)
                    {
                        item = (RequistionProduce)allreturnGrid.SelectedItems[0];
                    }
                }));
            if (item.Status != null)
            {
                int returnid = item.Id;
                var list = new List<Lplfw.UI.Produce.MaterialProduce>();
                using (var db = new ModelContainer())
                {
                    var _returnitemlist = db.ReturnItemSet.ToList();
                    var _materiallist = db.MaterialSet.ToList();
                    foreach (var _returnitem in _returnitemlist)
                    {
                        if (_returnitem.ReturnId == returnid)
                        {
                            MaterialProduce _orderitem = new MaterialProduce();
                            var _material = db.MaterialSet.Where(i => i.Id == _returnitem.MaterialId).ToList();
                            _orderitem.MaterialName = _material.First().Name;
                            _orderitem.Quantity = _returnitem.Quantity;
                            _orderitem.Unit = _material.First().Unit;
                            list.Add(_orderitem);
                        }
                    }
                }
                this.returnmaterialGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    returnmaterialGrid.ItemsSource = list;
                }
                ));
            }
        }

        /// <summary>
        /// 绑定生产记录管理界面显示所有记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetAllProduction(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(ShowAllProduction));
            mythread.Start(1);
        }
        /// <summary>
        /// 生产记录管理界面显示所有记录具体操作
        /// </summary>
        /// <param name="id"></param>
        private void ShowAllProduction(object id)
        {
            var list = new List<Lplfw.UI.Produce.ProductionProduce>();
            using (var db = new ModelContainer())
            {
                var _productionList = db.ProductionSet.ToList();
                foreach (var _order in _productionList)
                {
                    var _myorder = new ProductionProduce();
                    var _products = db.ProductSet.Where(i => i.Id == _order.ProductId).ToList();
                    _myorder.ProductName = _products.First().Name;
                    _myorder.RequisitionId = _order.RequisitionId;
                    _myorder.Id = _order.Id;
                    _myorder.Status = _order.Status;
                    _myorder.StartAt = _order.StartAt.ToString();
                    var assem = db.AssemblyLineSet.Where(i => i.Id == _order.AssemblyLineId).ToList();
                    _myorder.AssemblyLineName = assem.First().Name;
                    _myorder.ThinkFinishedAt = _order.ThinkFinishedAt.ToString();
                    _myorder.FinishedAt = _order.FinishedAt.ToString();
                    var assemuid = assem.First().UserId;
                    var users = db.UserSet.Where(i => i.Id == assemuid).ToList();
                    _myorder.UserName = users.First().Name;
                    _myorder.Description = _order.Description;
                    list.Add(_myorder);
                }
            }
            this.allproductionGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    allproductionGrid.ItemsSource = null;
                    allproductionGrid.ItemsSource = list;
                }
                ));
        }
        /// <summary>
        /// 绑定生产记录管理界面标记完成生产部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishProduction(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(LetProductionFinished));
            mythread.Start(1);
        }
        /// <summary>
        /// 生产记录管理界面标记完成生产部分操作
        /// </summary>
        /// <param name="id"></param>
        private void LetProductionFinished(object id)
        {
            var selectcount = 0;
            this.allproductionGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    selectcount = allproductionGrid.SelectedItems.Count;
                }));
            if (selectcount > 0)
            {
                var item = new ProductionProduce();
                this.allproductionGrid.Dispatcher.Invoke(new Action(
               delegate
               {
                   item = (ProductionProduce)allproductionGrid.SelectedItems[0];
               }));
                if (item.Status.Equals("unfinished"))
                {
                    using (var db = new ModelContainer())
                    {
                        db.ProductionSet.Where(i => i.Id == item.Id).FirstOrDefault().Status = "finished";
                        db.SaveChanges();
                    }
                    ShowAllProduction(null);                   //尚未调试不清楚可行性
                }
            }
        }
        /// <summary>
        /// 与生产质检浏览界面显示所有记录绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetAllQuality(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(ShowAllQuality));
            mythread.Start(1);
        }
        /// <summary>
        /// 生产质检浏览界面显示所有记录具体操作
        /// </summary>
        /// <param name="id"></param>
        private void ShowAllQuality(object id)
        {
            List<QualityProduce> allqplist = new List<QualityProduce>();
            using (var db = new ModelContainer())
            {
                var _qplist = db.ProductionQualitySet.ToList();
                foreach (var _qp in _qplist)
                {
                    var _allqp = new QualityProduce();
                    _allqp.ProductionId = _qp.ProductionId;
                    _allqp.Result = _qp.Result;
                    _allqp.Time = _qp.Time.ToString();
                    _allqp.Description = _qp.Description;
                    var users = db.UserSet.Where(i => i.Id == _qp.UserId).ToList();
                    _allqp.UserName = users.First().Name;
                    allqplist.Add(_allqp);
                }
            }
            this.allqualityGrid.Dispatcher.Invoke(new Action(
               delegate
               {
                   allqualityGrid.ItemsSource = null;
                   allqualityGrid.ItemsSource = allqplist;
               }
               ));
        }

        private void btnSearchQuality_Click(object sender, RoutedEventArgs e)
        {
            var mythred = new Thread(new ParameterizedThreadStart(SearchQualityAction));
            mythred.Start(1);
        }
        public void SearchQualityAction(object id)
        {
            string searchcontent = "";
            string searchcondition = "";
            List<QualityProduce> list = new List<QualityProduce>();
            this.txtSearchQuality.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcontent = txtSearchQuality.Text;
                }));
            this.cbSearchQuality.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcondition = cbSearchQuality.Text;
                }));
            this.allqualityGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    if (searchcontent != "")
                    {
                        var _list = GetAllQualityData();
                        switch (searchcondition)
                        {
                            case "生产记录号":
                                list = _list.Where(i => i.ProductionId == Convert.ToInt32(searchcontent)).ToList();
                                break;
                            case "检查结果":
                                list = _list.Where(i => i.Result == searchcontent).ToList();
                                break;
                            case "检查时间":
                                list = _list.Where(i => i.Time == searchcontent).ToList();
                                break;
                            case "备注":
                                list = _list.Where(i => i.Description == searchcontent).ToList();
                                break;
                            case "负责人":
                                list = _list.Where(i => i.UserName == searchcontent).ToList();
                                break;
                        }
                    }
                }));
            this.allqualityGrid.Dispatcher.Invoke(new Action(
            delegate
            {
                allqualityGrid.ItemsSource = null;
                allqualityGrid.ItemsSource = list;
            }));
        }
        private List<QualityProduce> GetAllQualityData()
        {
            List<QualityProduce> allqplist = new List<QualityProduce>();
            using (var db = new ModelContainer())
            {
                var _qplist = db.ProductionQualitySet.ToList();
                foreach (var _qp in _qplist)
                {
                    var _allqp = new QualityProduce();
                    _allqp.ProductionId = _qp.ProductionId;
                    _allqp.Result = _qp.Result;
                    _allqp.Time = _qp.Time.ToString();
                    _allqp.Description = _qp.Description;
                    var users = db.UserSet.Where(i => i.Id == _qp.UserId).ToList();
                    _allqp.UserName = users.First().Name;
                    allqplist.Add(_allqp);
                }
            }
            return allqplist;
        }

        private void btnSearchProduction_Click(object sender, RoutedEventArgs e)
        {
            var mythred = new Thread(new ParameterizedThreadStart(SearchProductionAction));
            mythred.Start(1);
        }
        public void SearchProductionAction(object id)
        {
            string searchcontent = "";
            string searchcondition = "";
            List<ProductionProduce> list = new List<ProductionProduce>();
            this.txtSearchProduction.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcontent = txtSearchProduction.Text;
                }));
            this.cbSearchProduction.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcondition = cbSearchProduction.Text;
                }));
            this.allproductionGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    if (searchcontent != "")
                    {
                        var _list = GetAllProductionData();
                        switch (searchcondition)
                        {
                            case "状态":
                                list = _list.Where(i => i.Status == searchcontent).ToList();
                                break;
                            case "生产记录号":
                                list = _list.Where(i => i.Id == Convert.ToInt32(searchcontent)).ToList();
                                break;
                            case "产品":
                                list = _list.Where(i => i.ProductName == searchcontent).ToList();
                                break;
                            case "流水线":
                                list = _list.Where(i => i.AssemblyLineName == searchcontent).ToList();
                                break;
                            case "领料单号":
                                list = _list.Where(i => i.RequisitionId == Convert.ToInt32(searchcontent)).ToList();
                                break;
                            case "开始时间":
                                list = _list.Where(i => i.StartAt == searchcontent).ToList();
                                break;
                            case "预计结束时间":
                                list = _list.Where(i => i.ThinkFinishedAt == searchcontent).ToList();
                                break;
                            case "结束时间":
                                list = _list.Where(i => i.FinishedAt == searchcontent).ToList();
                                break;
                            case "备注":
                                list = _list.Where(i => i.Description == searchcontent).ToList();
                                break;
                            case "负责人":
                                list = _list.Where(i => i.UserName == searchcontent).ToList();
                                break;
                        }
                    }
                }));
            this.allproductionGrid.Dispatcher.Invoke(new Action(
            delegate
            {
                allproductionGrid.ItemsSource = null;
                allproductionGrid.ItemsSource = list;
            }));
        }
        private List<ProductionProduce> GetAllProductionData()
        {
            var list = new List<Lplfw.UI.Produce.ProductionProduce>();
            using (var db = new ModelContainer())
            {
                var _productionList = db.ProductionSet.ToList();
                foreach (var _order in _productionList)
                {
                    var _myorder = new ProductionProduce();
                    var _products = db.ProductSet.Where(i => i.Id == _order.ProductId).ToList();
                    _myorder.ProductName = _products.First().Name;
                    _myorder.RequisitionId = _order.RequisitionId;
                    _myorder.Id = _order.Id;
                    _myorder.Status = _order.Status;
                    _myorder.StartAt = _order.StartAt.ToString();
                    var assem = db.AssemblyLineSet.Where(i => i.Id == _order.AssemblyLineId).ToList();
                    _myorder.AssemblyLineName = assem.First().Name;
                    _myorder.ThinkFinishedAt = _order.ThinkFinishedAt.ToString();
                    _myorder.FinishedAt = _order.FinishedAt.ToString();
                    var assemuid = assem.First().UserId;
                    var users = db.UserSet.Where(i => i.Id == assemuid).ToList();
                    _myorder.UserName = users.First().Name;
                    _myorder.Description = _order.Description;
                    list.Add(_myorder);
                }
            }
            return list;
        }

        private void btnSearchReturn_Click(object sender, RoutedEventArgs e)
        {
            var mythred = new Thread(new ParameterizedThreadStart(SearchReturnAction));
            mythred.Start(1);
        }
        public void SearchReturnAction(object id)
        {
            string searchcontent = "";
            string searchcondition = "";
            List<RequistionProduce> list = new List<RequistionProduce>();
            this.txtSearchReturn.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcontent = txtSearchReturn.Text;
                }));
            this.cbSearchReturn.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcondition = cbSearchReturn.Text;
                }));
            this.allreturnGrid.Dispatcher.Invoke(new Action(
               delegate
               {
                   if (searchcontent != "")
                   {
                       var _list = GetAllReturnData();
                       switch (searchcondition)
                       {
                           case "状态":
                               list = _list.Where(i => i.Status == searchcontent).ToList();
                               break;
                           case "领料单号":
                               list = _list.Where(i => i.Id == Convert.ToInt32(searchcontent)).ToList();
                               break;
                           case "流水线":
                               list = _list.Where(i => i.AssemblyLineId == Convert.ToInt32(searchcontent)).ToList();
                               break;
                           case "创建时间":
                               list = _list.Where(i => i.CreatedAt == searchcontent).ToList();
                               break;
                           case "完成时间":
                               list = _list.Where(i => i.FinishedAt == searchcontent).ToList();
                               break;
                           case "备注":
                               list = _list.Where(i => i.Description == searchcontent).ToList();
                               break;
                           case "负责人":
                               list = _list.Where(i => i.UserId == Convert.ToInt32(searchcontent)).ToList();
                               break;
                       }
                   }
               }));
            this.allreturnGrid.Dispatcher.Invoke(new Action(
            delegate
            {
                allreturnGrid.ItemsSource = null;
                allreturnGrid.ItemsSource = list;
            }));
            this.returnmaterialGrid.Dispatcher.Invoke(new Action(
            delegate
            {
                returnmaterialGrid.ItemsSource = null;
            }));
        }
        private List<RequistionProduce> GetAllReturnData()
        {
            var list = new List<Lplfw.UI.Produce.RequistionProduce>();
            using (var db = new ModelContainer())
            {
                var _returnList = db.ReturnSet.ToList();
                foreach (var _order in _returnList)
                {
                    var _myorder = new RequistionProduce();
                    _myorder.Id = _order.Id;
                    var assem = db.RequisitionSet.Where(i => i.Id == _order.RequisitionId).ToList();
                    _myorder.AssemblyLineId = assem.First().AssemblyLineId;
                    _myorder.Status = _order.Status;
                    _myorder.CreatedAt = _order.CreateAt.ToString();
                    _myorder.FinishedAt = _order.FinishedAt.ToString();
                    _myorder.UserId = _order.UserId;
                    _myorder.Description = _order.Description;
                    list.Add(_myorder);
                }
            }
            return list;
        }

        private void btnSearchRequisition_Click(object sender, RoutedEventArgs e)
        {
            var mythred = new Thread(new ParameterizedThreadStart(SearchRequisitionAction));
            mythred.Start(1);
        }
        public void SearchRequisitionAction(object id)
        {
            string searchcontent = "";
            string searchcondition = "";
            List<RequistionProduce> list = new List<RequistionProduce>();
            this.txtSearchRequisition.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcontent = txtSearchRequisition.Text;
                }));
            this.cbSearchRequisition.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcondition = cbSearchRequisition.Text;
                }));
            this.allreturnGrid.Dispatcher.Invoke(new Action(
               delegate
               {
                   if (searchcontent != "")
                   {
                       var _list = GetAllRequisitionData();
                       switch (searchcondition)
                       {
                           case "状态":
                               list = _list.Where(i => i.Status == searchcontent).ToList();
                               break;
                           case "领料单号":
                               list = _list.Where(i => i.Id == Convert.ToInt32(searchcontent)).ToList();
                               break;
                           case "流水线":
                               list = _list.Where(i => i.AssemblyLineId == Convert.ToInt32(searchcontent)).ToList();
                               break;
                           case "创建时间":
                               list = _list.Where(i => i.CreatedAt == searchcontent).ToList();
                               break;
                           case "完成时间":
                               list = _list.Where(i => i.FinishedAt == searchcontent).ToList();
                               break;
                           case "备注":
                               list = _list.Where(i => i.Description == searchcontent).ToList();
                               break;
                           case "负责人":
                               list = _list.Where(i => i.UserId == Convert.ToInt32(searchcontent)).ToList();
                               break;
                       }
                   }
               }));
            this.allrequistionGrid.Dispatcher.Invoke(new Action(
            delegate
            {
                allrequistionGrid.ItemsSource = null;
                allrequistionGrid.ItemsSource = list;
            }));
            this.requistionmaterialGrid.Dispatcher.Invoke(new Action(
            delegate
            {
                requistionmaterialGrid.ItemsSource = null;
            }));
        }
        private List<RequistionProduce> GetAllRequisitionData()
        {
            var list = new List<Lplfw.UI.Produce.RequistionProduce>();
            using (var db = new ModelContainer())
            {
                var _requistionList = db.RequisitionSet.ToList();
                foreach (var _order in _requistionList)
                {
                    var _myorder = new RequistionProduce();
                    _myorder.Id = _order.Id;
                    _myorder.AssemblyLineId = _order.AssemblyLineId;
                    _myorder.Status = _order.Status;
                    _myorder.CreatedAt = _order.CreateAt.ToString();
                    _myorder.FinishedAt = _order.FinishedAt.ToString();
                    _myorder.UserId = _order.UserId;
                    _myorder.Description = _order.Description;
                    list.Add(_myorder);
                }
            }
            return list;
        }
    }
    /// <summary>
    /// 在Produce管理部分用的Requistion领料单类
    /// </summary>
    public class RequistionProduce
    {
        public int Id { get; set; }
        public int AssemblyLineId { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }
        public string FinishedAt { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
    }
    /// <summary>
    /// 需求清单与物料库存
    /// </summary>
    public class MaterialProduce
    {
        public string MaterialName { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
    public class ProductionProduce
    {
        public string Status { get; set; }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string AssemblyLineName { get; set; }
        public int RequisitionId { get; set; }
        public string StartAt { get; set; }
        public string ThinkFinishedAt { get; set; }
        public string FinishedAt { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
    }
    public class QualityProduce
    {
        public int ProductionId { get; set; }
        public string Result { get; set; }
        public string Time { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
    }
}