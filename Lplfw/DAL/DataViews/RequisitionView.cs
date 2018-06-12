using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class RequisitionView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public String Description { get; set; }
        public int UserId { get; set; }
        public int SalesId { get; set; }
        public string UserName { get; set; }
        public string SalesCode { get; set; }

        static public List<RequisitionView> GetAll()
        {
            using (var _db = new ModelContainer())
            {
                var _sql = $"select * from requisitionview";
                return _db.Database.SqlQuery<RequisitionView>(_sql).ToList();
            }
        }

        static public List<RequisitionView> GetUnfinished()
        {
            using (var _db = new ModelContainer())
            {
                var _sql = $"select * from requisitionview where Status='处理中' or Status='领料中'";
                return _db.Database.SqlQuery<RequisitionView>(_sql).ToList();
            }
        }
    }
}
