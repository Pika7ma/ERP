using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class RequisitionProcedure
    {
        public int MaterialId { get; set; }
        public int MaterialQuantity { get; set; }

        static public List<RequisitionProcedure> GetById(int index)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"call getrequsitionfromsales({index})";
                return db.Database.SqlQuery<RequisitionProcedure>(sql).ToList();
            }
        }
    }
}
