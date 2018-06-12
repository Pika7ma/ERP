using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class ReturnView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreateAt { get; set; }
        public String Description { get; set; }
        public int UserId { get; set; }
        public int RequisitionId { get; set; }
        public string UserName { get; set; }
        public string RequisitionCode { get; set; }

        static public List<ReturnView> GetAll()
        {
            using (var _db = new ModelContainer())
            {
                var _sql = $"select * from returnview";
                return _db.Database.SqlQuery<ReturnView>(_sql).ToList();
            }
        }
    }
}
