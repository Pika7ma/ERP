using System;
using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class MaterialStockInItemView
    {
        public int StockInId { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }
        public string MaterialName { get; set; }

        static public List<MaterialStockInItemView> GetById(int index)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    if (index == 0)
                    {
                        var _sql = $"select * from materialstockinitemview";
                        return _db.Database.SqlQuery<MaterialStockInItemView>(_sql).ToList();
                    }
                    else
                    {
                        var _sql = $"select * from materialstockinitemview where StockInId={index}";
                        return _db.Database.SqlQuery<MaterialStockInItemView>(_sql).ToList();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
