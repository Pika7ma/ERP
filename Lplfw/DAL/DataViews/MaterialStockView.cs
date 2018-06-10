using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class MaterialStockView
    {
        public int MaterialId { get; set; }
        public int StorageId { get; set; }
        public int Quantity { get; set; }
        public int Location { get; set; }
        public string MaterialName { get; set; }
        public string StorageName { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }

        static public List<MaterialStockView> GetById(int index)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from materialstockview where StorageId={index}";
                return db.Database.SqlQuery<MaterialStockView>(sql).ToList();
            }
        }
    }
}
