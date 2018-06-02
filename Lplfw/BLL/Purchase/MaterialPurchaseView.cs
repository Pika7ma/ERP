using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
   public  class MaterialPurchaseView
    {
        public int mId { get; set; }
        public string mName { get; set; }
        public double Price { get; set; }
        public string sName { get; set; }
        public int sId { get; set; }
        public int SumQuantity { get; set; }
        public string Contact { get; set; }
        public string Tel { get; set; }


        public static List< MaterialPurchaseView> GetbyIdList(List<Material> m)
        {
            var _mpv = new List< MaterialPurchaseView>();
            using (var _db = new ModelContainer())
            {
                foreach (var i in m)
                {
                    string sql = $"select * from  materialpurchaseview  where mId='" + i.Id + "'";
                    var _tm = _db.Database.SqlQuery< MaterialPurchaseView>(sql).ToList();
                    foreach (var n in _tm)
                    {
                        _mpv.Add(n);
                    }
                }
            }
            return _mpv;
        }



        public static List< MaterialPurchaseView> Getall()
        {
            using (var _db = new DAL.ModelContainer())
            {
                var _sql = $"select * from materialpurchaseview";
                return _db.Database.SqlQuery< MaterialPurchaseView>(_sql).ToList();
            }

        }

        public static List< MaterialPurchaseView> Getalllow(int lownumber)
        {
            using (var _db = new DAL.ModelContainer())
            {

                var _sql = $"select * from materialpurchaseview where SumQuantity<{lownumber}";
                return _db.Database.SqlQuery< MaterialPurchaseView>(_sql).ToList();
            }

        }

        public static List< MaterialPurchaseView> Getallbysupplier(string supplier)
        {
            using (var _db = new DAL.ModelContainer())
            {

                var _sql = $"select * from  materialpurchaseview where sName={supplier}";
                return _db.Database.SqlQuery< MaterialPurchaseView>(_sql).ToList();
            }

        }


        public static List< MaterialPurchaseView> Getallbymaterial(string material)
        {
            using (var _db = new DAL.ModelContainer())
            {

                var _sql = $"select * from  materialpurchaseview where mName={material}";
                return _db.Database.SqlQuery< MaterialPurchaseView>(_sql).ToList();
            }

        }
    }
}
