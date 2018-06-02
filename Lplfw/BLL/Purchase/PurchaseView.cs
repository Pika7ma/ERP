using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
     public class PurchaseView
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public String Status { get; set; }
        public System.DateTime CreateAt { get; set; }
        public System.DateTime FinishedAt { get; set; }
        public String mName { get; set; }
        public String sName { get; set; }
        public string uName { get; set; }

        public static List<PurchaseView> Getall()
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }



        public static List<PurchaseView> Getallbyid(int id)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview where Id={id}";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }

        public static List<PurchaseView> Getallbysupplier(string supplier)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview where sName={supplier}";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }


        public static List<PurchaseView> Getallbyuser(string user)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview where uName={user}";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }

        public static List<PurchaseView> Getallbycreateat(string createat)
        {
            var _createat = Convert.ToDateTime(createat);
            var _createat2 = _createat.AddDays(1);
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview where CreateAt>='{_createat}' and CreateAt<'{_createat2}'";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }

        public static List<PurchaseView> Getallbycfinishedat(string finishedat)
        {
            var _finishedat = Convert.ToDateTime(finishedat);
            var _finishedat2 = _finishedat.AddDays(1);
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview where FinishedAt>='{_finishedat}' and FinishedAt<'{_finishedat2}'";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }
    }
}
