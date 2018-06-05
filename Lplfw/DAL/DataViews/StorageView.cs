using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class StorageView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }

        static public List<StorageView> GetAll()
        {
            using (var _db = new ModelContainer())
            {
                string sql = $"select * from storageview";
                return _db.Database.SqlQuery<StorageView>(sql).ToList();
            }
        }
    }
}
