using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public partial class Supplier
    {
        /// <summary>
        /// 新建供应商
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="name">名字</param>
        /// <param name="contact">联系人</param>
        /// <param name="location">地址</param>
        /// <param name="tel">电话</param>
        static public void AddSupplier(Int32 id, String name, String contact, String location, String tel)
        {
            var _sup = new Supplier
            {
                Id = id,
                Name = name,
                Contact = contact,
                Location = location,
                Tel = tel
            };
            using (var _db = new ModelContainer())
            {
                _db.SupplierSet.Add(_sup);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// 通过ID获取供应商
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <returns>Supplier对象</returns>
        static public Supplier GetbyId(Int32 id)
        {
            using (var _db = new ModelContainer())
            {
                var _sup = _db.SupplierSet.FirstOrDefault(i => i.Id == id);
                return _sup;
            }

        }

        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="id">供应商ID</param>
        static public void DelSupplier(Int32 id)
        {
            Supplier _sup = GetbyId(id);
            using (var _db = new ModelContainer())
            {
                _db.SupplierSet.Remove(_sup);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// 修改指定ID供应商
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="contact">联系人</param>
        /// <param name="location">地址</param>
        /// <param name="tel">电话</param>
        static public void EditSupplier(Int32 id, String contact, String location, String tel)
        {
            Supplier _sup = GetbyId(id);
            _sup.Contact = contact;
            _sup.Location = location;
            _sup.Tel = tel;
            using (var _db = new ModelContainer())
            {
                _db.SaveChanges();
            }
        }
    }
}
