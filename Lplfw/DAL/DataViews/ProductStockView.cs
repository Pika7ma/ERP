using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class ProductStockView
    {
        public int ProductId { get; set; }
        public int StorageId { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }
        public string ProductName { get; set; }
        public string StorageName { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }

        static public List<ProductStockView> GetById(int index)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from productstockview where StorageId={index}";
                return db.Database.SqlQuery<ProductStockView>(sql).ToList();
            }
        }
    }
}
