using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
     public class RequisitionItemView
    {
        public int RequisitionId { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public string MaterialName { get; set; }

        static public List<RequisitionItemView> GetById(int index)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from requisitionitemview where RequisitionId={index}";
                return db.Database.SqlQuery<RequisitionItemView>(sql).ToList();
            }
        }
    }
}
