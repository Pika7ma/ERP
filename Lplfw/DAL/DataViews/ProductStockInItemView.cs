using System;
using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class ProductStockInItemView
    {
        public int StockInId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public string ProductName { get; set; }

        static public List<ProductStockInItemView> GetById(int index)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    if (index == 0)
                    {
                        var _sql = $"select * from productstockinitemview";
                        return _db.Database.SqlQuery<ProductStockInItemView>(_sql).ToList();
                    }
                    else
                    {
                        var _sql = $"select * from productstockinitemview where StockInId={index}";
                        return _db.Database.SqlQuery<ProductStockInItemView>(_sql).ToList();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
