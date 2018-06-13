using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class UserGroupViewModel
    {
        private UserGroup obj;
        private List<UserGroupPrivilegeItem> privileges;

        public UserGroupViewModel()
        {
            obj = new UserGroup();
            privileges = new List<UserGroupPrivilegeItem>();
        }

        public string TxtName
        {
            get { return obj.Name; }
            set {
                if (value == "") obj.Name = null;
                else obj.Name = value;
            }
        }

        private string txtCheckMessage;
        public bool CanSubmit
        {
            get
            {
                if (obj.Name == null)
                {
                    txtCheckMessage = "请输入用户组名";
                    return false;
                }
                using (var _db = new ModelContainer())
                {
                    var _old = _db.UserGroupSet.FirstOrDefault(i => i.Name == obj.Name);
                    if (_old == null)
                    {
                        txtCheckMessage = null;
                        return true;
                    }
                    else
                    {
                        txtCheckMessage = "该用户组已存在!";
                        return false;
                    }
                }
            }
        }

        public string TxtCheckMessage
        {
            get { return txtCheckMessage; }
        }

        public bool CreateNew()
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    _db.UserGroupSet.Add(obj);
                    _db.SaveChanges();
                    var _privileges = _db.PrivilegeSet.ToList();
                    var _id = obj.Id;
                    for (var _i = 0; _i < _privileges.Count; _i++)
                    {
                        var _privilege = _privileges[_i];
                        _db.UserGroupPrivilegeItemSet.Add(new UserGroupPrivilegeItem
                        {
                            UserGroupId = _id,
                            PrivilegeId = _privilege.Id,
                            Mode = 1
                        });
                    }
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
