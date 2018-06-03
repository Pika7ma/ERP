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

namespace Lplfw.UI.Order
{
    /// <summary>
    /// NewOrderItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewOrderItem : Window
    {
        public MyProduct newproduct { get; set; }
        public bool result { get; set; }
        public bool isEdit { get; set; }
        public NewOrderItem(bool isNew)
        {
            InitializeComponent();
            newproduct = new MyProduct();
            result = false;
            if (isNew)
            {
                Title = "新建订单项目";
                isEdit = false;
            }
            else
            {
                Title = "修改订单项目";
                isEdit = true;
            }
            using (var db = new ModelContainer())
            {
                cbClass.ItemsSource = db.ProductClassSet.ToList();
            }
        }
        /// <summary>
        /// 当启用修改订单项目时，设置初始值
        /// </summary>
        public void CheckProduct()
        {
            using (var db = new ModelContainer())
            {
                var _classid = db.ProductSet.Where(i => i.Name == newproduct.Name).FirstOrDefault().ClassId;
                cbClass.ItemsSource = db.ProductClassSet.Where(i => i.Id == _classid).ToList();
                cbClass.SelectedValue = _classid;
                cbName.ItemsSource = db.ProductSet.Where(i => i.Name == newproduct.Name).ToList();
                cbName.SelectedValue = db.ProductSet.Where(i => i.Name == newproduct.Name).FirstOrDefault().Id;
                tbNumber.Text = newproduct.Quantity.ToString();
                tbPrice.Text = newproduct.Price.ToString();
                tbOriginPrice.Text = db.ProductSet.Where(i => i.Name == newproduct.Name).FirstOrDefault().Price.ToString();
            }
        }
        /// <summary>
        /// 当选中产品类时，显示该类下所有产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetProductName(object sender, SelectionChangedEventArgs e)
        {
            if (!isEdit)
            {
                tbOriginPrice.Text = "";
                var mythread = new Thread(new ParameterizedThreadStart(GetProductNameAction));
                mythread.Start(1);
            }
            else
            {
                isEdit = false;
            }
        }
        /// <summary>
        /// 显示所有产品的具体操作
        /// </summary>
        /// <param name="id"></param>
        private void GetProductNameAction(object id)
        {
            string name = "";
            this.cbClass.Dispatcher.Invoke(new Action(
                delegate
                {
                    name = cbClass.Text;
                }));
            List<Product> mylist = new List<Product>();
            using (var db = new ModelContainer())
            {
                var _classid = db.ProductClassSet.Where(i => i.Name == name).FirstOrDefault().Id;
                mylist = db.ProductSet.Where(i => i.ClassId == _classid).ToList();
            }
            this.cbName.Dispatcher.Invoke(new Action(
                delegate
                {
                    cbName.ItemsSource = mylist;
                }));
        }
        /// <summary>
        /// 选中产品时获取其原价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetOriginPrice(object sender, SelectionChangedEventArgs e)
        {
            var mythread = new Thread(new ParameterizedThreadStart(GetOriginPriceAction));
            mythread.Start(1);
        }
        /// <summary>
        /// 获取产品原价的具体操作
        /// </summary>
        /// <param name="id"></param>
        private void GetOriginPriceAction(object id)
        {
            string name = "";
            this.cbName.Dispatcher.Invoke(new Action(
                delegate
                {
                    name = cbName.Text;
                }));
            if (name != "")
            {
                string mystring = "";
                using (var db = new ModelContainer())
                {
                    mystring = db.ProductSet.Where(i => i.Name == name).FirstOrDefault().Price.ToString();
                }
                this.tbOriginPrice.Dispatcher.Invoke(new Action(
                    delegate
                    {
                        tbOriginPrice.Text = mystring;
                    }));
            }
        }
        /// <summary>
        /// 绑定“确定”按钮，将订单项信息传输给父窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetNewProduct(object sender, RoutedEventArgs e)
        {
            if (cbClass.Text != "" && cbName.Text != "" && tbNumber.Text != "" && tbPrice.Text != "")
            {
                MyProduct _newproduct = new MyProduct();
                _newproduct.Name = cbName.Text;
                _newproduct.Quantity = Convert.ToInt32(tbNumber.Text);
                _newproduct.Price = Convert.ToDouble(tbPrice.Text);
                newproduct = _newproduct;
                result = true;
                isEdit = true;
                this.Close();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
