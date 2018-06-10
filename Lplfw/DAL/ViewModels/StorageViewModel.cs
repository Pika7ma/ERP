using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class StorageViewModel
    {
        private Storage obj;

        public StorageViewModel()
        {
            obj = new Storage();
        }

        public StorageViewModel(Storage storage)
        {
            obj = storage;
        }

        public string TxtName
        {
            get { return obj.Name; }
            set
            {
                if (value == "")
                {
                    obj.Name = null;
                }
                else
                {
                    obj.Name = value;
                }
            }
        }

        public int? CbUser
        {
            get { return obj.UserId; }
            set
            {
                if (value == null)
                {
                    obj.UserId = 0;
                }
                else
                {
                    obj.UserId = (int)value;
                }
            }
        }

        public string TxtLocation
        {
            get { return obj.Location; }
            set
            {
                if (value == "")
                {
                    obj.Location = null;
                }
                else
                {
                    obj.Location = value;
                }
            }
        }

        public string TxtDescription
        {
            get { return obj.Description; }
            set
            {
                if (value == "")
                {
                    obj.Description = null;
                }
                else
                {
                    obj.Description = value;
                }
            }
        }

        public bool CanSubmit
        {
            get
            {
                if (obj.UserId == 0) return false;
                if (obj.Name == null) return false;
                if (obj.Location == null) return false;
                return true;
            }
        }

        public string CheckMessage
        {
            get
            {
                if (obj.UserId == 0) return "请选择负责人";
                if (obj.Name == null) return "请填写仓库名";
                if (obj.Location == null) return "请填写仓库位置";
                return null;
            }
        }

        public bool CreateNew()
        {
            using (var _db = new ModelContainer())
            {
                try
                {
                    _db.StorageSet.Add(obj);
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool SaveChanges()
        {
            using (var _db = new ModelContainer())
            {
                try
                {
                    var _storage = _db.StorageSet.FirstOrDefault(i => i.Id == obj.Id);
                    _storage.Name = obj.Name;
                    _storage.Location = obj.Location;
                    _storage.Description = obj.Description;
                    _storage.UserId = obj.UserId;
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
