using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lplfw.DAL
{
    /// <summary>
    /// 一个用于显示配方的数据库视图
    /// </summary>
    public class RecipeView
    {
        public int ProductId { get; set; }
        public int MaterialId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }

        public static List<RecipeView> GetByProductId(int index)
        {
            using (var db = new ModelContainer())
            {
                string sql = $"select * from recipeview where ProductId={index}";
                return db.Database.SqlQuery<RecipeView>(sql).ToList();
            }
        }
    }
}
