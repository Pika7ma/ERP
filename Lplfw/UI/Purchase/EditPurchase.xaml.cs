using Lplfw.BLL.Purchase;
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
    /// EditPurchase.xaml 的交互逻辑
    /// </summary>
    public partial class EditPurchase : Window
    {
        public int Id;
        //public int userid;
        private DAL.Purchase Purchase { set; get; }
        private List<PurchaseItemView> mypurchaseitems { get; set; }

        /// <summary>
        /// 新建函数，在考虑用户权限时参数应加入usergroupid来限制用户对订单的修改情况
        /// </summary>
        /// <param name="Id">订单id</param>
        public EditPurchase(int Id)
        {
            InitializeComponent();
            this.Id = Id;
            var _thread = new Thread(new ParameterizedThreadStart(Load_Purchase));
            _thread.Start(Id);

        }
        private void Load_Purchase(object Id)
        {
            var _id = (int)Id;
            using (var _db = new DAL.ModelContainer())
            {
                Purchase = _db.PurchaseSet.FirstOrDefault(i => i.Id == _id);
                mypurchaseitems = PurchaseItemView.GetPurchaseitemsbypid(_id);
                var _users = new List<DAL.User>();
                _users = _db.UserSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {

                    dgMaterialItems.ItemsSource = mypurchaseitems;
                    cbuser.ItemsSource = _users;
                    cbuser.SelectedValue = Purchase.UserId;
                    cbst.ItemsSource = load_cbst();
                    cbst.SelectedValue = Purchase.Status;
                    cbproirity.Text = Purchase.Priority;
                    txcreateat.Text = Convert.ToString(Purchase.CreateAt);
                    dmfinishedat.Value = Convert.ToDateTime(Purchase.FinishedAt);
                    txdec.Text = Purchase.Description;
                });
            }

        }


        /// <summary>
        /// 初始化订单状态，在有权限时会对选项来筛选,普通用户只能申请，取消，检查用户---拒绝，通过，完成
        /// </summary>
        /// <returns></returns>
        private List<SearchCombobox> load_cbst()
        {
            List<SearchCombobox> _cbst = new List<SearchCombobox>();
            var _item1 = new SearchCombobox("申请", 1);
            var _item2 = new SearchCombobox("取消", 2);
            var _item3 = new SearchCombobox("拒绝", 3);
            var _item4 = new SearchCombobox("通过", 4);
            var _item5 = new SearchCombobox("完成", 5);
            _cbst.Add(_item1);
            _cbst.Add(_item2);
            _cbst.Add(_item3);
            _cbst.Add(_item4);
            _cbst.Add(_item5);
            return _cbst;
        }


        /// <summary>
        /// 获得子窗口订单条目
        /// </summary>
        /// <param name="mypurchaseitem"></param>
        /// <returns></returns>
        public bool GetItem(PurchaseItemView mypurchaseitem)
        {
            if (mypurchaseitems.Exists(pi => pi.MaterialId == mypurchaseitem.MaterialId && pi.SupplierId == mypurchaseitem.SupplierId))
            {
                return false;
            }
            else
            {
                mypurchaseitems.Add(mypurchaseitem);
                dgMaterialItems.Items.Refresh();
                return true;
            }
        }


        /// <summary>
        /// 获得子窗口订单条目
        /// </summary>
        /// <param name="selecteditem"></param>
        /// <param name="changeditem"></param>
        /// <returns></returns>
        public bool ChangeItem(PurchaseItemView selecteditem, PurchaseItemView changeditem)
        {
            if (selecteditem.MaterialId == changeditem.MaterialId && selecteditem.SupplierId == changeditem.SupplierId)
            {
                DeleteSelectedItem(selecteditem);
                mypurchaseitems.Add(changeditem);
                return true;
            }
            else
            {
                if (mypurchaseitems.Exists(pi => pi.MaterialId == changeditem.MaterialId && pi.SupplierId == changeditem.SupplierId))
                {
                    return false;
                }
                else
                {
                    DeleteSelectedItem(selecteditem);
                    mypurchaseitems.Add(changeditem);
                    return true;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnadditem_click(object sender, RoutedEventArgs e)
        {

            NewPurchaseItem newitem = new NewPurchaseItem();
            newitem.Purchaseid = Id;
            newitem.getitem += GetItem;
            newitem.Show();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void ok_click(object sender, RoutedEventArgs e)
        {
            if ((cbuser.Text != "") && (cbproirity.Text != "") && (cbst.Text != ""))
            {
                if ((cbst.Text == "完成") && (dmfinishedat.Value == Convert.ToDateTime("0001-01-01 00:00:00")))
                {
                    MessageBox.Show("请填写成时间");
                    return;
                }
                try
                {
                    var _thread = new Thread(new ThreadStart(ok_thread));
                    _thread.Start();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


            }
            else
            {
                MessageBox.Show("请完成订单内容");
            }

        }
        private void ok_thread()
        {
            using (var _db = new DAL.ModelContainer())
            {

                DAL.Purchase purchase = _db.PurchaseSet.First(i => i.Id == Id);
                Dispatcher.BeginInvoke((Action)delegate ()
                {

                    purchase.CreateAt = Convert.ToDateTime(txcreateat.Text);
                    purchase.FinishedAt = (DateTime)dmfinishedat.Value;
                    purchase.Description = txdec.Text;
                    purchase.Id = Id;
                    purchase.Status = cbst.Text;
                    purchase.Priority = cbproirity.Text;
                    purchase.UserId = (int)cbuser.SelectedValue;
                });
                List<PurchaseItem> purchaseItems = _db.PurchaseItemSet.Where(i => i.PurchaseId == Id).ToList();
                _db.PurchaseItemSet.RemoveRange(purchaseItems);
                _db.PurchaseItemSet.AddRange(PurchaseItemView.changetoitems(mypurchaseitems));
                _db.SaveChanges();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_item"></param>
        private void DeleteSelectedItem(PurchaseItemView _item)
        {
            foreach (var i in mypurchaseitems.ToArray())
            {
                if (i.MaterialId == _item.MaterialId && i.SupplierId == _item.SupplierId)
                {
                    mypurchaseitems.Remove(i);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Delete_click(object sender, RoutedEventArgs e)
        {
            var _item = dgMaterialItems.SelectedItem as PurchaseItemView;
            if (_item == null)
            {
                MessageBox.Show("请选中条目");
                return;
            }
            else
            {
                DeleteSelectedItem(_item);

            }

            dgMaterialItems.Items.Refresh();
        }




        private void Change_click(object sender, RoutedEventArgs e)
        {
            var _item = dgMaterialItems.SelectedItem as PurchaseItemView;
            if (_item == null)
            {
                MessageBox.Show("请选中条目");
                return;
            }
            else
            {
                NewPurchaseItem newitem = new NewPurchaseItem(_item.MaterialId, _item.SupplierId, _item.Quantity, _item.Price);
                newitem.Purchaseid = Id;
                newitem.changeitem += ChangeItem;
                newitem.ShowDialog();
                dgMaterialItems.Items.Refresh();
            }
        }

        /// <summary>
        /// 订单状态改变时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)cbst.SelectedValue == "完成")
            {
                dmfinishedat.IsEnabled = true;
            }
            else
            {
                dmfinishedat.IsEnabled = false;
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cance_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
