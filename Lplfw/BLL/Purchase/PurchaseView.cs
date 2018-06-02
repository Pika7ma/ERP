using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
     public class PurchaseView
    {
        public int Id { get; set; }
        public String Status { get; set; }
        public String Priority { get; set; }
        public System.DateTime CreateAt { get; set; }
        public System.DateTime? FinishedAt { get; set; }
        public String Description { get; set; }
        public String Name { get; set; }
        public int UserId { get; set; }

        public static List<PurchaseView> GetAll()
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }
        }



        public static List<PurchaseView> GetAllById(int id)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview where Id={id}";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }

        public static List<PurchaseView> GetAllBySupplier(string supplier)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview where sName={supplier}";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }


        public static List<PurchaseView> GetAllByUser(string user)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview where uName={user}";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }

        public static List<PurchaseView> GetAllByCreateAt(string createat)
        {
            var _createat = Convert.ToDateTime(createat);
            var _createat2 = _createat.AddDays(1);
            using (var db = new ModelContainer())
            {
                string sql = $"select * from purchaseview where CreateAt>='{_createat}' and CreateAt<'{_createat2}'";
                return db.Database.SqlQuery<PurchaseView>(sql).ToList();
            }

        }

        public static List<PurchaseView> GetAllByFinishedat(string finishedat)
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
