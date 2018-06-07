using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class MaterialPriceView
    {
        public int MaterialId { get; set; }
        public int SupplierId { get; set; }
        public double Price { get; set; }
        public int StartQuantity { get; set; }
        public int MaxQuantity { get; set; }
        public string MaterialName { get; set; }
        public int ClassId { get; set; }
        public string SupplierName { get; set; }

        static public List<MaterialPriceView> GetByClassId(int? index)
        {
            using (var _db = new ModelContainer())
            {
                if (index == 0 || index == null)
                {
                    var _sql = $"select * from materialpriceview";
                    return _db.Database.SqlQuery<MaterialPriceView>(_sql).ToList();
                }
                else
                {
                    var _classes = MaterialClass.GetSubClassIndexs((int)index);
                    var _lists = new List<MaterialPriceView>();
                    foreach (var _classId in _classes)
                    {
                        var _sql = $"select * from materialpriceview where ClassId={_classId}";
                        _lists.AddRange(_db.Database.SqlQuery<MaterialPriceView>(_sql).ToList());
                    }
                    return _lists;
                }
            }
        }

        static public List<MaterialPriceView> GetBySupplierId(int index)
        {
            using (var _db = new ModelContainer())
            {
                var _sql = $"select * from materialpriceview where SupplierId={index}";
                return _db.Database.SqlQuery<MaterialPriceView>(_sql).ToList();
            }
        }
    }
}
