using System;
using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class MaterialStockOutItemView
    {
        public int StockOutId { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }
        public string MaterialName { get; set; }

        static public List<MaterialStockOutItemView> GetById(int index)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    if (index == 0)
                    {
                        var _sql = $"select * from materialstockoutitemview";
                        return _db.Database.SqlQuery<MaterialStockOutItemView>(_sql).ToList();
                    }
                    else
                    {
                        var _sql = $"select * from materialstockoutitemview where StockOutId={index}";
                        return _db.Database.SqlQuery<MaterialStockOutItemView>(_sql).ToList();
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
