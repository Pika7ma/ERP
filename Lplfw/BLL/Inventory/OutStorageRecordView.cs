using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lplfw.DAL
{
    public class OutStorageRecordView
    {
        public int InlId { get; set; }
        public int StorageId { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }

        static public List<OutStorageRecordView> GetByStorageId(int index)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from outstoragerecordview where StorageId={index}";
                return db.Database.SqlQuery<OutStorageRecordView>(sql).ToList();
            }
        }
        static public List<OutStorageRecordView> GetAll()
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from OutStorageRecordview";
                return db.Database.SqlQuery<OutStorageRecordView>(sql).ToList();
            }
        }
    }
}
