using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class PrivilegeView
    {
        public int UserGroupId { get; set; }
        public int PrivilegeId { get; set; }
        public int Mode { get; set; }
        public string PrivilegeName { get; set; }

        static public List<PrivilegeView> GetById(int index)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from privilegeview where UserGroupId={index}";
                return db.Database.SqlQuery<PrivilegeView>(sql).ToList();
            }
        }
    }
}
