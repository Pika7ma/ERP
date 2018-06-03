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

namespace Lplfw.UI.Order
{
    /// <summary>
    /// NewOrder.xaml 的交互逻辑
    /// </summary>
    public partial class NewOrder : Window
    {
        public Dictionary<string, MyProduct> productslist { get; set; }
        public NewOrder()
        {
            InitializeComponent();
            productslist = new Dictionary<string, MyProduct>();
            cbPriority.ItemsSource = PriorityFields;
        }
        static public List<Utils.KeyValue> PriorityFields = new List<Utils.KeyValue>
        {
            new Utils.KeyValue { ID=0, Name="低" },
            new Utils.KeyValue { ID=1, Name="中" },
            new Utils.KeyValue { ID=2, Name="高" },
        };
        /// <summary>
        /// 绑定新建订单条目，增加订单中的商品，如果重复增加则均以后一次为准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewSalesItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewOrderItem(isNew: true);
            var _rtn = _win.ShowDialog();
            if (_rtn == false)
            {
                var newproduct = _win.newproduct;
                if (_win.result)
                {
                    if (productslist.ContainsKey(newproduct.Name))
                    {
                        productslist[newproduct.Name].Price = newproduct.Price;
                        productslist[newproduct.Name].Quantity = newproduct.Quantity;
                    }
                    else
                    {
                        productslist.Add(newproduct.Name, newproduct);
                    }
                    dgSalesItems.ItemsSource = productslist.Values.ToList();
                }
            }
        }
        /// <summary>
        /// 绑定“修改订单条目”，会自动在窗口显示相关信息
        /// ！！！不允许修改商品类和商品名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditSalesItem(object sender, RoutedEventArgs e)
        {
            if (dgSalesItems.SelectedItems.Count > 0)
            {
                var _win = new NewOrderItem(isNew: false);
                _win.cbClass.IsReadOnly = true;
                _win.cbName.IsReadOnly = true;
                _win.newproduct = (MyProduct)dgSalesItems.SelectedItems[0];
                _win.CheckProduct();
                var _rtn = _win.ShowDialog();
                if (_rtn == false && _win.result)
                {
                    productslist[_win.newproduct.Name].Price = _win.newproduct.Price;
                    productslist[_win.newproduct.Name].Quantity = _win.newproduct.Quantity;
                }
                dgSalesItems.ItemsSource = productslist.Values.ToList();
            }
        }
        /// <summary>
        /// 绑定“删除订单条目”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSalesItem(object sender, RoutedEventArgs e)
        {
            if (dgSalesItems.SelectedItems.Count > 0)
            {
                var selecteditem = (MyProduct)dgSalesItems.SelectedItems[0];
                productslist.Remove(selecteditem.Name);
                dgSalesItems.ItemsSource = productslist.Values.ToList();
            }
        }
        /// <summary>
        /// 绑定“确认”按钮
        /// 将订单信息插入数据库，目前设定user为默认值,时间默认为开始日期的0点，和结束日期的24点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm(object sender, RoutedEventArgs e)
        {
            var cp1 = tbCustomer.Text;
            var cp2 = cbPriority.Text;
            var tp2 = tbCustomerTel.Text;
            var dp1 = dpCreated.Text;
            var dp2 = dpRequired.Text;
            var tp = tbDescription.Text;
            var dg = dgSalesItems.Items.Count;
            if (cp1 != "" && cp2 != "" && dp1 != "" && dp2 != "" && tp != "" && dg > 0 && tp2 != "")
            {
                dp1 += " 00:00:00";
                dp2 += " 23:59:59";
                var _created = Convert.ToDateTime(dp1);
                var _required = Convert.ToDateTime(dp2);
                using (var db = new ModelContainer())
                {
                    var _newsale = new Sales();
                    _newsale.UserId = Utils.CurrentUser.Id;
                    _newsale.Tel = tp2;
                    _newsale.Customer = cp1;
                    _newsale.Status = "未完成";
                    _newsale.Priority = cp2;
                    _newsale.CreateAt = _created;
                    _newsale.FinishedAt = _required;
                    _newsale.Description = tp;
                    db.SalesSet.Add(_newsale);
                    db.SaveChanges();
                    foreach (MyProduct _item in dgSalesItems.Items)
                    {
                        var _newitem = new SalesItem();
                        _newitem.Price = _item.Price;
                        _newitem.Quantity = _item.Quantity;
                        _newitem.SalesId = _newsale.Id;
                        // 为什么要这么写...简直在逗我
                        _newitem.ProductId = db.ProductSet.Where(i => i.Name == _item.Name).First().Id;
                        db.SalesItemSet.Add(_newitem);
                    }
                    db.SaveChanges();
                }
                Close();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    public class MyProduct
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
