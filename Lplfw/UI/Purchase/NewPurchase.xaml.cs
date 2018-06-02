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
    /// NewPurchase.xaml 的交互逻辑
    /// </summary>
    public partial class NewPurchase : Window
    {
        private int id;                  //默认订单Id
        private List<PurchaseItemView> purchaseitems;                  //dgmaterialitem的itemsource


        /// <summary>
        /// 
        /// </summary>
        public NewPurchase()
        {
            purchaseitems = new List<PurchaseItemView>();
            InitializeComponent();
            GetPurchaseId();
            txcreateat.Text = DateTime.Now.ToString();
            dgMaterialItems.ItemsSource = purchaseitems;
            var _thread1 = new Thread(new ThreadStart(Setcbuseritem));
            _thread1.Start();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mypurchaseitem"></param>
        /// <returns></returns>
        public bool GetItem(PurchaseItemView mypurchaseitem)
        {
            if (purchaseitems.Exists(pi => pi.MaterialId == mypurchaseitem.MaterialId && pi.SupplierId == mypurchaseitem.SupplierId))
            {
                return false;
            }
            else
            {
                purchaseitems.Add(mypurchaseitem);
                dgMaterialItems.Items.Refresh();
                return true;
            }

        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="selecteditem"></param>
        /// <param name="changeditem"></param>
        /// <returns></returns>

        public bool changeitem(PurchaseItemView selecteditem, PurchaseItemView changeditem)
        {
            if (selecteditem.MaterialId == changeditem.MaterialId && selecteditem.SupplierId == changeditem.SupplierId)
            {
                DeleteSelectedItem(selecteditem);
                purchaseitems.Add(changeditem);
                return true;
            }
            else
            {
                if (purchaseitems.Exists(pi => pi.MaterialId == changeditem.MaterialId && pi.SupplierId == changeditem.SupplierId))
                {
                    return false;
                }
                else
                {
                    DeleteSelectedItem(selecteditem);
                    purchaseitems.Add(changeditem);
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

            var newitem = new NewPurchaseItem();
            newitem.Purchaseid = id;
            newitem.getitem += GetItem;
            newitem.Show();


        }




        /// <summary>
        /// 
        /// </summary>
        public void GetPurchaseId()
        {

            var _thread = new Thread(new ThreadStart(GetIdthread));
            _thread.Start();

        }

        private void GetIdthread()
        {
            using (var _db = new DAL.ModelContainer())
            {
                if (_db.PurchaseSet.Select(n => n.Id).Count() != 0)
                {
                    id = _db.PurchaseSet.Select(n => n.Id).Max() + 1;

                }
                else
                {
                    id = 1;
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void Setcbuseritem()
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                using (var _db = new DAL.ModelContainer())
                {
                    cbuser.ItemsSource = _db.UserSet.Select(n => new { n.Id, n.Name }).ToList();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Ok_click(object sender, RoutedEventArgs e)
        {
            if ((txdec.Text != "") && (cbuser.Text != "") && (cbproirity.Text != ""))
            {
                if (purchaseitems != null)
                {
                    DAL.Purchase purchase = new DAL.Purchase();
                    purchase.CreateAt = Convert.ToDateTime(txcreateat.Text);
                    purchase.Description = txdec.Text;
                    purchase.Id =id;
                    purchase.Status = txst.Text;
                    purchase.Priority = cbproirity.Text;
                    purchase.UserId = (int)cbuser.SelectedValue;
                    var _thread = new Thread(new ParameterizedThreadStart(AddPurchase));
                    _thread.Start(purchase);
                    this.Close();

                }
                else
                {
                    MessageBox.Show("请添加条目");
                }
            }
            else
            {
                MessageBox.Show("请完成订单内容");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchase"></param>
        private void AddPurchase(object purchase)
        {
            var _purchase = (DAL.Purchase)purchase;

            using (var _db = new DAL.ModelContainer())
            {
                try
                {
                    _db.PurchaseSet.Add(_purchase);
                    _db.PurchaseItemSet.AddRange(PurchaseItemView.ChangeToItems(purchaseitems));
                    _db.SaveChanges();

                }
                catch (Exception dbEx)
                {
                    MessageBox.Show(dbEx.ToString());
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_item"></param>
        private void DeleteSelectedItem(PurchaseItemView _item)
        {
            foreach (var i in purchaseitems.ToArray())
            {
                if (i.MaterialId == _item.MaterialId && i.SupplierId == _item.SupplierId)
                {
                    purchaseitems.Remove(i);
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Change_click(object sender, RoutedEventArgs e)
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
                newitem.Purchaseid = id;
                newitem.changeitem += changeitem;
                newitem.ShowDialog();
                dgMaterialItems.Items.Refresh();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
