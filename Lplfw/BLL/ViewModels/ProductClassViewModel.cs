using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class ProductClassViewModel
    {
        private ProductClass obj;

        public ProductClassViewModel()
        {
            obj = new ProductClass();
        }

        public ProductClassViewModel(ProductClass obj)
        {
            this.obj = obj;
        }

        public ProductClassViewModel(int? index)
        {
            using (var _db = new ModelContainer())
            {
                obj = _db.ProductClassSet.FirstOrDefault(i => i.Id == index);
            }
        }

        public int? CbParentClass {
            get { return obj.ParentId; }
            set { obj.ParentId = value; }
        }

        public string TxtName
        {
            get { return obj.Name; }
            set { obj.Name = value; }
        }

        public void CreateNew()
        {
            using (var _db = new ModelContainer())
            {
                _db.ProductClassSet.Add(obj);
                _db.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            using (var _db = new ModelContainer())
            {
                var _productClass = _db.ProductClassSet.FirstOrDefault(i => i.Id == obj.Id);
                _productClass.Name = obj.Name;
                _productClass.ParentId = obj.ParentId;
                _db.SaveChanges();
            }
        }
    }
}
