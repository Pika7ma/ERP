using System;
using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class MaterialStockAllView
    {
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public string MaterialName { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }

        static public List<MaterialStockAllView> GetAll()
        {
            try
            {
                using (var db = new ModelContainer())
                {
                    string sql = $"select * from materialstockallview";
                    return db.Database.SqlQuery<MaterialStockAllView>(sql).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
