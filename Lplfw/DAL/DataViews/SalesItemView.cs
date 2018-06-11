using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class SalesItemView
    {
        public int SalesId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }

        public static List<SalesItemView> GetById(int salesId)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from salesitemview where SalesId={salesId}";
                return db.Database.SqlQuery<SalesItemView>(sql).ToList();
            }
        }
    }
}
