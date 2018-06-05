using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lplfw.DAL
{
    public class InStorageRecordView
    {
        public int InlId { get; set; }
        public int StorageId { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }


        static public List<InStorageRecordView> GetByStorageId(int index)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from instoragerecordview where StorageId={index}";
                return db.Database.SqlQuery<InStorageRecordView>(sql).ToList();
            }
        }

        static public List<InStorageRecordView> GetAll()
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from  InStorageRecordview";
                return db.Database.SqlQuery<InStorageRecordView>(sql).ToList();
            }
        }
    }
}
