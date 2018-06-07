using System.Collections.Generic;

namespace Lplfw.DAL
{
    public partial class Privilege
    {
        /// <summary>
        /// 初始创建权限
        /// </summary>
        static public void Init()
        {
            List<Privilege> _privis = new List<Privilege>();
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    switch (j)
                    {
                        case 0:
                            _privis.Add(new Privilege { Description = "不可见" });
                            break;
                        case 1:
                            _privis.Add(new Privilege { Description = "只读" });
                            break;
                        case 2:
                            _privis.Add(new Privilege { Description = "可修改" });
                            break;
                    }
                }
            }
            using (var _db = new ModelContainer())
            {
                _db.PrivilegeSet.AddRange(_privis);
                _db.SaveChanges();
            }
        }

    }
}
