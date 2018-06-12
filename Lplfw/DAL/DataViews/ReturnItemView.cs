using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class ReturnItemView
    {
        public int RequisitionId { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public string MaterialName { get; set; }

        static public List<ReturnItemView> GetById(int index)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from returnitemview where ReturnId={index}";
                return db.Database.SqlQuery<ReturnItemView>(sql).ToList();
            }
        }
    }
}
