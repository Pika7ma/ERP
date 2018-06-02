using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
     public class PurchaseItemView
     {
        public int PurchaseId { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public int SupplierId { get; set; }
        public double Price { set; get; }
        public string MaterialName { get; set; }
        public string SupplierName { get; set; }

        public static List<PurchaseItemView> GetByPurchaseId(int pid)
        {
            using (var _db = new DAL.ModelContainer())
            {
                string sql = $"select * from purchaseitemview where PurchaseId={pid}";
                return _db.Database.SqlQuery<PurchaseItemView>(sql).ToList();
            }
        }


        public static DAL.PurchaseItem ChangeToItem(PurchaseItemView purchaseitem)
        {
            DAL.PurchaseItem _purchaseItem = new DAL.PurchaseItem();
            _purchaseItem.MaterialId =purchaseitem.MaterialId;
           _purchaseItem.PurchaseId =purchaseitem.PurchaseId;
            _purchaseItem.SupplierId =purchaseitem.SupplierId;
            _purchaseItem.Quantity =purchaseitem.Quantity;
            _purchaseItem.Price =purchaseitem.Price;
            return _purchaseItem;
        }
        public static List<DAL.PurchaseItem> ChangeToItems(List<PurchaseItemView> purchaseitems)
        {
            List<DAL.PurchaseItem> purchaseItems = new List<DAL.PurchaseItem>();
            foreach (var i in purchaseitems)
            {
                purchaseItems.Add(PurchaseItemView.ChangeToItem(i));

            }
            return purchaseItems;

        }
    }
}
