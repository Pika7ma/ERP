using System;
using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class ProductStockAllView
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }

        static public List<ProductStockAllView> Get()
        {
            try
            {
                using (var db = new ModelContainer())
                {
                    string sql = $"select * from productstockallview";
                    return db.Database.SqlQuery<ProductStockAllView>(sql).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
