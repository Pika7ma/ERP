using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    class MaterialPriceView
    {
        public int mId { get; set; }
        public string mName { get; set; }
        public int sId { get; set; }
        public string sName { get; set; }
        public double Price { get; set; }
        public int StartQuantity { get; set; }
        public int MaxQuantity { get; set; }

        public static List<MaterialPriceView> GetAllPrice()
        {
            using (var _db = new ModelContainer())
            {
                string sql = $"select * from materialpriceView";
                return _db.Database.SqlQuery<MaterialPriceView>(sql).ToList();
            }
        }


        public static List<MaterialPriceView> GetMaterialPriceViewbymid(int mid)
        {
            using (var _db = new DAL.ModelContainer())
            {
                string sql = $"select * from materialpriceview where mId={mid}";
                return _db.Database.SqlQuery<MaterialPriceView>(sql).ToList();
            }
        }


        public static List<MaterialPriceView> GetbyIdList(List<Material> m)
        {
            var _mpv = new List<MaterialPriceView>();
            using (var _db = new ModelContainer())
            {
                foreach (var i in m)
                {
                    string sql = $"select * from materialpriceView where mId='" + i.Id + "'";
                    var _tm = _db.Database.SqlQuery<MaterialPriceView>(sql).ToList();
                    foreach (var n in _tm)
                    {
                        _mpv.Add(n);
                    }
                }
            }
            return _mpv;
        }

        public static List<MaterialPriceView> GetbySupplierId(int id)
        {
            using (var _db = new ModelContainer())
            {
                string sql = $"select * from materialpriceView where sId='" + id + "'";
                return _db.Database.SqlQuery<MaterialPriceView>(sql).ToList();
            }
        }

        public static List<MaterialPriceView> GetbySupplierName(string name)
        {
            using (var _db = new ModelContainer())
            {
                string sql = $"select * from materialpriceView where sName='" + name + "'";
                return _db.Database.SqlQuery<MaterialPriceView>(sql).ToList();
            }
        }

        public static List<MaterialPriceView> GetbyMaterialName(string name)
        {
            using (var _db = new ModelContainer())
            {
                string sql = $"select * from materialpriceView where mName='" + name + "'";
                return _db.Database.SqlQuery<MaterialPriceView>(sql).ToList();
            }
        }
    }
}
