using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class StockOutView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int StorageId { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public string StorageName { get; set; }
        public string UserName { get; set; }

        static public List<StockOutView> GetById(int index)
        {
            using (var _db = new ModelContainer())
            {
                if (index == 0)
                {
                    var _sql = $"select * from stockoutview";
                    return _db.Database.SqlQuery<StockOutView>(_sql).ToList();
                }
                else
                {
                    var _sql = $"select * from stockoutview where StorageId={index}";
                    return _db.Database.SqlQuery<StockOutView>(_sql).ToList();
                }
            }
        }
    }
}
