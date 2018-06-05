using System;
using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class ProductStockOutItemView
    {
        public int StockOutId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public string ProductName { get; set; }

        static public List<ProductStockOutItemView> GetById(int index)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    if (index == 0)
                    {
                        var _sql = $"select * from productstockoutitemview";
                        return _db.Database.SqlQuery<ProductStockOutItemView>(_sql).ToList();
                    }
                    else
                    {
                        var _sql = $"select * from productstockoutitemview where StockOutId={index}";
                        return _db.Database.SqlQuery<ProductStockOutItemView>(_sql).ToList();
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
