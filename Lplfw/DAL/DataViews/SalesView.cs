using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class SalesView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Customer { get; set; }
        public string Tel { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public System.DateTime CreateAt { get; set; }
        public System.DateTime DueTime { get; set; }
        public Nullable<System.DateTime> FinishedAt { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }

        public static List<SalesView> GetAll()
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from salesview";
                return db.Database.SqlQuery<SalesView>(sql).OrderByDescending(c => c.CreateAt).ToList();
            }
        }
    }
}
