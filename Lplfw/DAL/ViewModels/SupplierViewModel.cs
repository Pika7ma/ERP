using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class SupplierViewModel
    {
        private Supplier obj;

        public SupplierViewModel()
        {
            obj = new Supplier();
        }

        public SupplierViewModel(Supplier supplier)
        {
            obj = supplier;
        }

        public string TxtName
        {
            get { return obj.Name; }
            set {
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

        public string TxtContact
        {
            get { return obj.Contact; }
            set
            {
                if (value == "")
                {
                    obj.Contact = null;
                }
                else
                {
                    obj.Contact = value;
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

        public string TxtTel
        {
            get { return obj.Tel; }
            set
            {
                if (value == "")
                {
                    obj.Tel = null;
                }
                else
                {
                    obj.Tel = value;
                }
            }
        }

        public bool CanSubmit
        {
            get
            {
                if (obj.Name == null) return false;
                if (obj.Contact == null) return false;
                if (obj.Location == null) return false;
                if (obj.Tel == null) return false;
                return true;
            }
        }

        public string TxtCheckMessage
        {
            get
            {
                if (obj.Name == null) return "请填写供应商名称";
                if (obj.Contact == null) return "请填写联系人";
                if (obj.Location == null) return "请填写供应商地址";
                if (obj.Tel == null) return "请填写供应商联系方式";
                return null;
            }
        }

        public bool CreateNew()
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    _db.SupplierSet.Add(obj);
                    _db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _supplier = _db.SupplierSet.FirstOrDefault(i => i.Id == obj.Id);
                    _supplier.Name = obj.Name;
                    _supplier.Contact = obj.Contact;
                    _supplier.Location = obj.Location;
                    _supplier.Tel = obj.Tel;
                    _db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
