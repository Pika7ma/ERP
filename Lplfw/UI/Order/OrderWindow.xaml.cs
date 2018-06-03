using System.Windows;
using System.Collections.Generic;
using Lplfw.DAL;
using System.Linq;
using System;
using System.Threading;

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
        /// <summary>
        /// 点击一条订单信息时，在右侧显示相应的订单详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetOrderInfo(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(GetOrderInfo));
            mythread.Start(1);
        }
        /// <summary>
        /// 显示订单详情的具体操作
        /// </summary>
        /// <param name="id"></param>
        private void GetOrderInfo(object id)
        {
            int selectcount = 0;
            Order order = new Order();
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate {
                    selectcount = orderGrid.SelectedItems.Count;
                    order = (Order)orderGrid.SelectedItems[0];
                }));
            if (selectcount > 0)
            {
                var orderid = order.Id;
                var list = new List<Lplfw.UI.Order.OrderInfo>();
                using (var db = new ModelContainer())
                {
                    var _salesitems = db.SalesItemSet.Where(i => i.SalesId == orderid).ToList();
                    foreach (var _salesitem in _salesitems)
                    {
                        var _item = new OrderInfo();
                        _item.Number = _salesitem.Quantity;
                        _item.Price = _salesitem.Price;
                        var _plist = db.ProductSet.Where(i => i.Id == 1).ToList();
                        var _product = db.ProductSet.Where(i => i.Id == _salesitem.ProductId);
                        _item.Name = _product.First().Name;
                        list.Add(_item);
                    }
                }
                this.orderInfoGrid.Dispatcher.Invoke(new Action(
                    delegate
                    {
                        orderInfoGrid.ItemsSource = list;
                    }));
            }
        }
        /// <summary>
        /// 绑定“显示所有订单”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetAllOrder(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(GetAllOrderAction));
            mythread.Start(1);
        }
        /// <summary>
        /// 获取所有订单的具体操作
        /// </summary>
        /// <param name="id"></param>
        private void GetAllOrderAction(object id)
        {
            var list = new List<Lplfw.UI.Order.Order>();
            using (var db = new ModelContainer())
            {
                var _orderList = db.SalesSet.ToList();
                var _userList = db.UserSet.ToList();
                foreach (var _order in _orderList)
                {
                    var _myorder = new Order();
                    _myorder.Status = _order.Status;
                    _myorder.Id = _order.Id;
                    _myorder.CreateAt = _order.CreateAt.ToString();
                    _myorder.FinishedAt = _order.FinishedAt.ToString();
                    _myorder.Priority = _order.Priority;
                    _myorder.Customer = _order.Customer;
                    _myorder.Tel = _order.Tel;
                    if (_myorder.Status.Equals("完成"))
                        _myorder.Finished = _myorder.FinishedAt;
                    else
                        _myorder.Finished = "订单暂未完成！";
                    var _user = _userList.Where(i => i.Id == _order.UserId).ToList().First();
                    _myorder.Principal = _user.Name;
                    list.Add(_myorder);
                }
            }
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    orderGrid.ItemsSource = list;
                }
                ));
        }
        /// <summary>
        /// 绑定“显示未完成订单”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetUnFinishedOrder(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(GetUnFinishedOrderAction));
            mythread.Start(1);
        }
        /// <summary>
        /// 显示未完成订单的具体操作
        /// </summary>
        /// <param name="id"></param>
        private void GetUnFinishedOrderAction(object id)
        {
            var list = new List<Lplfw.UI.Order.Order>();
            using (var db = new ModelContainer())
            {
                var _orderList = db.SalesSet.ToList();
                var _userList = db.UserSet.ToList();
                foreach (var _order in _orderList)
                {
                    if (_order.Status.Equals("未完成"))
                    {
                        var _myorder = new Order();
                        _myorder.Status = _order.Status;
                        _myorder.Id = _order.Id;
                        _myorder.CreateAt = _order.CreateAt.ToString();
                        _myorder.FinishedAt = _order.FinishedAt.ToString();
                        _myorder.Priority = _order.Priority;
                        _myorder.Customer = _order.Customer;
                        _myorder.Tel = _order.Tel;
                        if (_myorder.Status.Equals("完成"))
                            _myorder.Finished = _myorder.FinishedAt;
                        else
                            _myorder.Finished = "订单暂未完成！";
                        var _user = _userList.Where(i => i.Id == _order.UserId).ToList().First();
                        _myorder.Principal = _user.Name;
                        list.Add(_myorder);
                    }
                }
            }
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    orderGrid.ItemsSource = list;
                }));
        }
        /// <summary>
        /// 绑定“完成订单”
        /// 需要在选中未完成订单的情况下点击按钮才会生效，目前只支持选中一个目标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishOrder(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(FinishOrderAction));
            mythread.Start(1);
        }
        /// <summary>
        /// 完成订单的具体操作
        /// </summary>
        /// <param name="id"></param>
        private void FinishOrderAction(object id)
        {
            int selectcount = 0;
            Order item = new Order();
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    selectcount = orderGrid.SelectedItems.Count;
                    item = (Order)orderGrid.SelectedItems[0];
                }));
            if (selectcount > 0)
            {
                if (item.Status.Equals("未完成"))
                {
                    using (var db = new ModelContainer())
                    {
                        db.SalesSet.Where(i => i.Id == item.Id).FirstOrDefault().Status = "完成";
                        db.SaveChanges();
                    }
                    GetAllOrderAction(id);
                }
            }
        }
        /// <summary>
        /// 绑定“取消订单”
        /// 需要选中未完成订单，目前支持选中一个目标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelOrder(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(CancelOrderAction));
            mythread.Start(1);
        }
        /// <summary>
        /// 取消订单的具体操作
        /// </summary>
        /// <param name="id"></param>
        private void CancelOrderAction(object id)
        {
            int selectcount = 0;
            Order item = new Order();
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    selectcount = orderGrid.SelectedItems.Count;
                    item = (Order)orderGrid.SelectedItems[0];
                }));
            if (selectcount > 0)
            {
                if (item.Status.Equals("未完成"))
                {
                    using (var db = new ModelContainer())
                    {
                        var _order = db.SalesSet.Where(i => i.Id == item.Id).FirstOrDefault();
                        var _id = _order.Id;
                        db.SalesSet.Remove(_order);
                        var _items = db.SalesItemSet.Where(i => i.SalesId == _id);
                        foreach (var _item in _items)
                        {
                            db.SalesItemSet.Remove(_item);
                        }
                        db.SaveChanges();
                    }
                    GetAllOrderAction(id);
                }
            }
        }
        /// <summary>
        /// 绑定“推迟订单”
        /// 需要选中未完成订单，目前为天数加20，而且支持一个目标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostponeOrder(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(PostponeOrderAction));
            mythread.Start(1);
        }
        /// <summary>
        /// 延期订单的具体操作
        /// </summary>
        /// <param name="id"></param>
        private void PostponeOrderAction(object id)
        {
            int selectcount = 0;
            Order item = new Order();
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    selectcount = orderGrid.SelectedItems.Count;
                    item = (Order)orderGrid.SelectedItems[0];
                }));
            if (selectcount > 0)
            {
                if (item.Status.Equals("未完成"))
                {
                    using (var db = new ModelContainer())
                    {
                        var _time = db.SalesSet.Where(i => i.Id == item.Id).FirstOrDefault().FinishedAt.Value;
                        _time = _time.AddDays(20.5);
                        db.SalesSet.Where(i => i.Id == item.Id).FirstOrDefault().FinishedAt = _time;
                        db.SaveChanges();
                    }
                    GetAllOrderAction(id);
                }
            }
        }
        /// <summary>
        /// 绑定“物料分解”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecomposeMaterial(object sender, RoutedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(DecomposeMaterialAction));
            mythread.Start(1);
        }
        /// <summary>
        /// 物料分解的具体操作，将领料单生成并插入到数据库中
        /// 首先计算每种货物的总库存，判断能否出货，有一个不满足就不出领料单
        /// 之后按表中顺序选取仓库，优先将每个仓库中的货物全部拿出，再去下一个仓库
        /// 暂时没有考虑生产线的问题，以及用户是谁
        /// </summary>
        /// <param name="id"></param>
        private void DecomposeMaterialAction(object id)
        {
            int selectsount = 0;
            Order order = new Order();
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    selectsount = orderGrid.SelectedItems.Count;
                    order = (Order)orderGrid.SelectedItems[0];
                }));
            if (selectsount > 0)
            {
                var orderid = order.Id;
                using (var db = new ModelContainer())
                {
                    var _productlist = db.SalesItemSet.Where(i => i.SalesId == orderid);
                    Dictionary<int, int> _materialdic = new Dictionary<int, int>();
                    foreach (var _product in _productlist)
                    {
                        var _materials = db.RecipeItemSet.Where(i => i.ProductId == _product.ProductId);
                        foreach (var _material in _materials)
                        {
                            if (_materialdic.ContainsKey(_material.MaterialId))
                            {
                                _materialdic[_material.MaterialId] += (_material.Quantity * _product.Quantity);
                            }
                            else
                            {
                                _materialdic.Add(_material.MaterialId, (_material.Quantity * _product.Quantity));
                            }
                        }
                    }//获取全部物料，计算总额
                    bool _isfull = true;
                    foreach (KeyValuePair<int, int> _material in _materialdic)
                    { //判断仓库存储是否充足
                        var _stocklist = db.MaterialStockSet.Where(i => i.MaterialId == _material.Key);
                        int _sum = 0;
                        foreach (var _stock in _stocklist)
                        {
                            _sum += _stock.Quantity;
                        }
                        if (_sum < _material.Value)
                            _isfull = false;
                    }
                    if (_isfull)
                    {
                        var _requsition = new Requisition();
                        _requsition.AssemblyLineId = 1;
                        _requsition.CreateAt = System.DateTime.Now;
                        _requsition.Description = "test requsition";
                        _requsition.FinishedAt = System.DateTime.Now;
                        _requsition.Status = "未完成";
                        _requsition.UserId = 1;
                        db.RequisitionSet.Add(_requsition);
                        db.SaveChanges();//添加领料单
                        var _myreqsition = db.RequisitionSet.ToList().Last();
                        var _reqid = _myreqsition.Id;
                        foreach (KeyValuePair<int, int> _material in _materialdic)//遍历每种物料，生成领料单
                        {
                            int _residue = 0;
                            var _stocklist = db.MaterialStockSet.Where(i => i.MaterialId == _material.Key);
                            foreach (var _stock in _stocklist)//遍历每个仓库，生成领料单
                            {
                                int _quantity = 0;
                                if (_residue > _stock.Quantity)
                                {
                                    _quantity = _residue - _stock.Quantity;
                                    _residue -= _stock.Quantity;
                                }
                                else
                                {
                                    _quantity = _residue;
                                    _residue = 0;
                                }
                                var _reqsitionitem = new RequisitionItem();
                                _reqsitionitem.RequisitionId = _reqid;
                                _reqsitionitem.MaterialId = _material.Key;
                                _reqsitionitem.Quantity = _quantity;
                                _reqsitionitem.StorageId = _stock.StorageId;
                                db.RequisitionItemSet.Add(_reqsitionitem);
                                if (_residue == 0)
                                    break;
                            }
                        }
                        db.SaveChanges();
                    }
                }
            }
        }
        /// <summary>
        /// 绑定搜索按钮，要求搜索框内要有内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search(object sender, RoutedEventArgs e)
        {
            var mythred = new Thread(new ParameterizedThreadStart(SearchAction));
            mythred.Start(1);
        }
        /// <summary>
        /// 搜索的具体实现
        /// </summary>
        /// <param name="id"></param>
        public void SearchAction(object id)
        {
            string searchcontent = "";
            string searchcondition = "";
            List<Lplfw.UI.Order.Order> list = new List<Lplfw.UI.Order.Order>();
            this.txtSearchSales.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcontent = txtSearchSales.Text;
                }));
            this.cbSearchSales.Dispatcher.Invoke(new Action(
                delegate
                {
                    searchcondition = cbSearchSales.Text;
                }));
            List<Order> _list = new List<Order>();
            this.orderGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    if (searchcontent != "")
                        _list = GetAllOrderData();
                }));
            if (searchcontent != "")
            {
                switch (searchcondition)
                {
                    case "状态":
                        list = _list.Where(i => i.Status == searchcontent).ToList();
                        break;
                    case "订单号":
                        list = _list.Where(i => i.Id == Convert.ToInt32(searchcontent)).ToList();
                        break;
                    case "客户":
                        list = _list.Where(i => i.Customer == searchcontent).ToList();
                        break;
                    case "订货时间":
                        list = _list.Where(i => i.CreateAt == searchcontent).ToList();
                        break;
                    case "应付时间":
                        list = _list.Where(i => i.FinishedAt == searchcontent).ToList();
                        break;
                    case "完成时间":
                        list = _list.Where(i => i.Finished == searchcontent).ToList();
                        break;
                    case "优先级":
                        list = _list.Where(i => i.Priority == searchcontent).ToList();
                        break;
                    case "负责人":
                        list = _list.Where(i => i.Principal == searchcontent).ToList();
                        break;
                }
            }
            this.orderGrid.Dispatcher.Invoke(new Action(
                    delegate
                    {
                        orderGrid.ItemsSource = list;
                    }));
            this.orderInfoGrid.Dispatcher.Invoke(new Action(
                delegate
                {
                    orderInfoGrid.ItemsSource = null;
                }));
        }
        /// <summary>
        /// 获取全部订单信息以便搜索
        /// </summary>
        /// <returns></returns>
        private List<Lplfw.UI.Order.Order> GetAllOrderData()
        {
            var list = new List<Lplfw.UI.Order.Order>();
            using (var db = new ModelContainer())
            {
                var _orderList = db.SalesSet.ToList();
                var _userList = db.UserSet.ToList();
                foreach (var _order in _orderList)
                {
                    var _myorder = new Order();
                    _myorder.Status = _order.Status;
                    _myorder.Id = _order.Id;
                    _myorder.CreateAt = _order.CreateAt.ToString();
                    _myorder.FinishedAt = _order.FinishedAt.ToString();
                    _myorder.Priority = _order.Priority;
                    _myorder.Customer = _order.Customer;
                    _myorder.Tel = _order.Tel;
                    if (_myorder.Status.Equals("完成"))
                        _myorder.Finished = _myorder.FinishedAt;
                    else
                        _myorder.Finished = "订单暂未完成！";
                    var _user = _userList.Where(i => i.Id == _order.UserId).ToList().First();
                    _myorder.Principal = _user.Name;
                    list.Add(_myorder);
                }
            }
            return list;
        }
    }
    public class Order
    {
        public string Status { get; set; }
        public int Id { get; set; }
        public string CreateAt { get; set; }
        public string FinishedAt { get; set; }
        public string Priority { get; set; }
        public string Customer { get; set; }
        public string Finished { get; set; }
        public string Principal { get; set; }
        public string Tel { get; set; }
    }
    public class OrderInfo
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public double Price { get; set; }
    }

}
