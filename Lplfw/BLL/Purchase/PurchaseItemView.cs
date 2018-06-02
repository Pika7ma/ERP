using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
     public class PurchaseItemView
    {
        public int Quantity { get; set; }
        public string mName { get; set; }
        public int MaterialId { get; set; }
        public string sName { get; set; }
        public double Price { set; get; }
        public int SupplierId { get; set; }
        public int PurchaseId { get; set; }

        public static List<PurchaseItemView> GetPurchaseitemsbymid(int mid)
        {
            using (var _db = new DAL.ModelContainer())
            {
                string sql = $"select * from purchaseitemview where MaterialId={mid}";
                return _db.Database.SqlQuery<PurchaseItemView>(sql).ToList();
            }
        }


        public static List<PurchaseItemView> GetPurchaseitemsbypid(int pid)
        {
            using (var _db = new DAL.ModelContainer())
            {
                string sql = $"select * from purchaseitemview where PurchaseId={pid}";
                return _db.Database.SqlQuery<PurchaseItemView>(sql).ToList();
            }
        }


        public static DAL.PurchaseItem changetoitem(PurchaseItemView purchaseitem)
        {
            DAL.PurchaseItem _purchaseItem = new DAL.PurchaseItem();
            _purchaseItem.MaterialId =purchaseitem.MaterialId;
           _purchaseItem.PurchaseId =purchaseitem.PurchaseId;
            _purchaseItem.SupplierId =purchaseitem.SupplierId;
            _purchaseItem.Quantity =purchaseitem.Quantity;
            _purchaseItem.Price =purchaseitem.Price;
            return _purchaseItem;
        }
        public static List<DAL.PurchaseItem> changetoitems(List<PurchaseItemView> purchaseitems)
        {
            List<DAL.PurchaseItem> purchaseItems = new List<DAL.PurchaseItem>();
            foreach (var i in purchaseitems)
            {
                purchaseItems.Add(PurchaseItemView.changetoitem(i));

            }
            return purchaseItems;

        }
    }
}
