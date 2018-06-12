using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class ProductionView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ProductId { get; set; }
        public string Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime ThinkFinishedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }

        static public List<ProductionView> GetAll()
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from productionview";
                return db.Database.SqlQuery<ProductionView>(sql).ToList();
            }
        }
    }
}
