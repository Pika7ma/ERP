using System.Windows;
using System.Collections.Generic;
using Lplfw.DAL;
using System.Linq;
using System;

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
        }

        private void show(object sender, RoutedEventArgs e)
        {

            //var list = new List<Lplfw.UI.Order.C>();
            //var c = new C { Message = "hello!" };
            //list.Add(c);

            //using (var db = new ModelContainer())
            //{
            //    var list2 = db.ProductClassSet.FirstOrDefault();
            //    var list3 = db.ProductClassSet.Where(i => i.Id == 1).ToList();
            //    db.ProductClassSet.Add(new ProductClass());
            //    db.SaveChanges();
            //}
            //grid.ItemsSource = list;
        }
        
        /// <summary>
        /// 绑定“显示所有订单”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetAllOrder(object sender, RoutedEventArgs e)
        {
            var list=new List<Lplfw.UI.Order.Order>();
            using (var db=new ModelContainer()){
                var _orderList=db.SalesSet.ToList();
                
                var _customerList=db.CustomerSet.ToList();
                foreach (var _order in _orderList) { 
                    var _myorder=new Order();
                    _myorder.Status = _order.Status;
                    _myorder.Id = _order.Id;
                    _myorder.CreateAt = _order.CreateAt.ToString();
                    _myorder.FinishedAt = _order.FinishedAt.ToString();
                    _myorder.Priority = _order.Priority;
                    var _customer = db.CustomerSet.Where(i => i.Id == _order.CustomerId).ToList();
                    _myorder.Customer = _customer.First().Name;
                    if (_myorder.Status.Equals("完成"))
                        _myorder.Finished = _myorder.FinishedAt;
                    else
                        _myorder.Finished = "订单暂未完成！";
                    list.Add(_myorder);
                }
            }
            orderGrid.ItemsSource = list;
        }
        /// <summary>
        /// 绑定“显示未完成订单”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetUnFinishedOrder(object sender, RoutedEventArgs e)
        {
            var list = new List<Lplfw.UI.Order.Order>();
            using (var db = new ModelContainer())
            {
                var _orderList = db.SalesSet.ToList();
                var _customerList = db.CustomerSet.ToList();
                foreach (var _order in _orderList)
                {
                    if (_order.Status.Equals("未完成")) {
                        var _myorder = new Order();
                        _myorder.Status = _order.Status;
                        _myorder.Id = _order.Id;
                        _myorder.CreateAt = _order.CreateAt.ToString();
                        _myorder.FinishedAt = _order.FinishedAt.ToString();
                        _myorder.Priority = _order.Priority;
                        var _customer = db.CustomerSet.Where(i => i.Id == _order.CustomerId).ToList();
                        _myorder.Customer = _customer.First().Name;
                        if (_myorder.Status.Equals("完成"))
                            _myorder.Finished = _myorder.FinishedAt;
                        else
                            _myorder.Finished = "订单暂未完成！";
                        list.Add(_myorder);
                    }
                }
            }
            orderGrid.ItemsSource = list;
        }
        /// <summary>
        /// 绑定“新建订单”暂时为点击就自动添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOrder(object sender, RoutedEventArgs e)
        {
            using (var db = new ModelContainer())
            {
                var _orderList = db.SalesSet.ToList();
                var _neworder = new Sales();
                Random _r = new Random();
                var _judge = _r.Next(0, 12);
                if(_judge>=6)
                    _neworder.Status = "未完成";
                else
                    _neworder.Status = "完成";
                if (_judge >= 8)
                    _neworder.Priority = "高";
                else if (_judge >= 4)
                    _neworder.Priority = "中";
                else
                    _neworder.Priority = "低";
                _neworder.CustomerId = 1;
                _neworder.UserId = 1;
                _neworder.Description = "addtest";
                _neworder.CreateAt = System.DateTime.Now;
                _neworder.FinishedAt = System.DateTime.Now.AddDays(2.3).AddHours(10.1);
                db.SalesSet.Add(_neworder);
                db.SaveChanges();
            }
            GetAllOrder(sender,e);
        }
        /// <summary>
        /// 绑定“完成订单”
        /// 需要在选中未完成订单的情况下点击按钮才会生效，目前只支持选中一个目标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishOrder(object sender, RoutedEventArgs e)
        {
            if (orderGrid.SelectedItems.Count > 0) {
                var item = (Order)orderGrid.SelectedItems[0];
                if (item.Status.Equals("未完成")) {
                    using (var db = new ModelContainer())
                    {
                        db.SalesSet.Where(i => i.Id == item.Id).FirstOrDefault().Status = "完成";
                        db.SaveChanges();
                    }
                    GetAllOrder(sender, e);
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
            if (orderGrid.SelectedItems.Count > 0)
            {
                var item = (Order)orderGrid.SelectedItems[0];
                if (item.Status.Equals("未完成"))
                {
                    using (var db = new ModelContainer())
                    {
                        var _order = db.SalesSet.Where(i => i.Id == item.Id).FirstOrDefault();
                        db.SalesSet.Remove(_order);
                        db.SaveChanges();
                    }
                    GetAllOrder(sender, e);
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

            if (orderGrid.SelectedItems.Count > 0)
            {
                var item = (Order)orderGrid.SelectedItems[0];
                if (item.Status.Equals("未完成"))
                {
                    using (var db = new ModelContainer())
                    {
                        var _time=db.SalesSet.Where(i => i.Id == item.Id).FirstOrDefault().FinishedAt.Value;
                        _time=_time.AddDays(20.5);
                        db.SalesSet.Where(i => i.Id == item.Id).FirstOrDefault().FinishedAt = _time;
                        db.SaveChanges();
                    }
                    GetAllOrder(sender, e);
                }
            }
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
    }
}
