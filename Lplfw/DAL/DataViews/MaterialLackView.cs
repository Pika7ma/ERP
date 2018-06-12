using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class MaterialLackView
    {
        public int MaterialId { get; set; }
        public int ClassId { get; set; }
        public string MaterialName { get; set; }
        public int SafeQuantity { get; set; }
        public int SumQuantity { get; set; }
        public int VirtualUsage { get; set; }
        public int VirtualQuantity { get; set; }

        static public List<MaterialLackView> GetByClassId(int? index)
        {
            using (var _db = new ModelContainer())
            {
                try
                {
                    if (index == 0 || index == null)
                    {
                        var _sql = $"select * from materiallackview where VirtualQuantity<=SafeQuantity";
                        return _db.Database.SqlQuery<MaterialLackView>(_sql).ToList();
                    }
                    else
                    {
                        var _classes = MaterialClass.GetSubClassIndexs((int)index);
                        var _lists = new List<MaterialLackView>();
                        foreach (var _classId in _classes)
                        {
                            var _sql = $"select * from materiallackview where VirtualQuantity<=SafeQuantity and ClassId={_classId}";
                            _lists.AddRange(_db.Database.SqlQuery<MaterialLackView>(_sql).ToList());
                        }
                        return _lists;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                
            }
        }
    }
}
