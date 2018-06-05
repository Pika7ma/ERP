using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public partial class Storage
    {
        static public List<Storage> GetAllToComboBox()
        {
            using (var _db = new ModelContainer())
            {
                var _storages = _db.StorageSet.ToList();
                _storages.Insert(0, new Storage
                {
                    Id = 0,
                    Name = "-所有仓库-",
                    UserId = 0,
                    Location = "",
                    Description = ""
                });
                return _storages;
            }
        }

        static public List<Storage> GetAll()
        {
            using (var _db = new ModelContainer())
            {
                return _db.StorageSet.ToList();
            }
        }
    }
}
