using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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
        private List<PurchaseItemView> MyPurchaseItems { get; set; }

        /// <summary>
        /// 新建函数，在考虑用户权限时参数应加入usergroupid来限制用户对订单的修改情况
        /// </summary>
        /// <param name="Id">订单id</param>
        public EditPurchase(int Id)
        {
            InitializeComponent();
            this.Id = Id;
            var _thread = new Thread(new ParameterizedThreadStart(LoadPurchase));
            _thread.Start(Id);

        }
        private void LoadPurchase(object Id)
        {
            var _id = (int)Id;
            using (var _db = new DAL.ModelContainer())
            {
                Purchase = _db.PurchaseSet.FirstOrDefault(i => i.Id == _id);
                MyPurchaseItems = PurchaseItemView.GetByPurchaseId(_id);
                var _users = new List<DAL.User>();
                _users = _db.UserSet.Select(i => i).ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {

                    dgMaterialItems.ItemsSource = MyPurchaseItems;
                    cbUser.ItemsSource = _users;
                    cbUser.SelectedValue = Purchase.UserId;
                    cbSt.ItemsSource = LoadCbSt();
                    cbSt.SelectedValue = Purchase.Status;
                    cbProirity.Text = Purchase.Priority;
                    txtCreateAt.Text = Convert.ToString(Purchase.CreateAt);
                    dmFinishedAt.Value = Convert.ToDateTime(Purchase.FinishedAt);
                    txdec.Text = Purchase.Description;
                });
            }

        }


        /// <summary>
        /// 初始化订单状态，在有权限时会对选项来筛选,普通用户只能申请，取消，检查用户---拒绝，通过，完成
        /// </summary>
        /// <returns></returns>
        private List<Utils.KeyValue> LoadCbSt()
        {
            return new List<Utils.KeyValue>
            {
                new Utils.KeyValue{ ID=1, Name="申请" },
                new Utils.KeyValue{ ID=2, Name="取消" },
                new Utils.KeyValue{ ID=3, Name="拒绝" },
                new Utils.KeyValue{ ID=4, Name="通过" },
                new Utils.KeyValue{ ID=5, Name="完成" },
            };
        }


        /// <summary>
        /// 获得子窗口订单条目
        /// </summary>
        /// <param name="mypurchaseitem"></param>
        /// <returns></returns>
        public bool GetItem(PurchaseItemView mypurchaseitem)
        {
            if (MyPurchaseItems.Exists(pi => pi.MaterialId == mypurchaseitem.MaterialId && pi.SupplierId == mypurchaseitem.SupplierId))
            {
                return false;
            }
            else
            {
                MyPurchaseItems.Add(mypurchaseitem);
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
                MyPurchaseItems.Add(changeditem);
                return true;
            }
            else
            {
                if (MyPurchaseItems.Exists(pi => pi.MaterialId == changeditem.MaterialId && pi.SupplierId == changeditem.SupplierId))
                {
                    return false;
                }
                else
                {
                    DeleteSelectedItem(selecteditem);
                    MyPurchaseItems.Add(changeditem);
                    return true;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnAddItemClick(object sender, RoutedEventArgs e)
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

        public void OKClick(object sender, RoutedEventArgs e)
        {
            if ((cbUser.Text != "") && (cbProirity.Text != "") && (cbSt.Text != ""))
            {
                if ((cbSt.Text == "完成") && (dmFinishedAt.Value == Convert.ToDateTime("0001-01-01 00:00:00")))
                {
                    MessageBox.Show("请填写成时间");
                    return;
                }
                try
                {
                    var _thread = new Thread(new ThreadStart(OKThread));
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
        private void OKThread()
        {
            using (var _db = new DAL.ModelContainer())
            {

                DAL.Purchase purchase = _db.PurchaseSet.First(i => i.Id == Id);
                Dispatcher.BeginInvoke((Action)delegate ()
                {

                    purchase.CreateAt = Convert.ToDateTime(txtCreateAt.Text);
                    purchase.FinishedAt = (DateTime)dmFinishedAt.Value;
                    purchase.Description = txdec.Text;
                    purchase.Id = Id;
                    purchase.Status = cbSt.Text;
                    purchase.Priority = cbProirity.Text;
                    purchase.UserId = (int)cbUser.SelectedValue;
                });
                List<PurchaseItem> purchaseItems = _db.PurchaseItemSet.Where(i => i.PurchaseId == Id).ToList();
                _db.PurchaseItemSet.RemoveRange(purchaseItems);
                _db.PurchaseItemSet.AddRange(PurchaseItemView.ChangeToItems(MyPurchaseItems));
                _db.SaveChanges();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_item"></param>
        private void DeleteSelectedItem(PurchaseItemView _item)
        {
            foreach (var i in MyPurchaseItems.ToArray())
            {
                if (i.MaterialId == _item.MaterialId && i.SupplierId == _item.SupplierId)
                {
                    MyPurchaseItems.Remove(i);
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
        private void CbStSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)cbSt.SelectedValue == "完成")
            {
                dmFinishedAt.IsEnabled = true;
            }
            else
            {
                dmFinishedAt.IsEnabled = false;
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
