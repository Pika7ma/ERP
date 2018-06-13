using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class UserViewModel
    {
        private User obj;

        public UserViewModel()
        {
            obj = new User();
        }

        public string TxtName
        {
            get { return obj.Name; }
            set {
                if (value == "") obj.Name = null;
                else obj.Name = value;
            }
        }

        public string TxtPassword
        {
            get { return obj.Password; }
            set
            {
                if (value == "") obj.Password = null;
                else obj.Password = value;
            }
        }

        public int? CbGroup
        {
            get { return obj.UserGroupId; }
            set
            {
                if (value == null) obj.UserGroupId = 0;
                else obj.UserGroupId = (int)value;
            }
        }

        public string TxtTel
        {
            get { return obj.Tel; }
            set
            {
                if (value == "") obj.Tel = null;
                else obj.Tel = value;
            }
        }

        private string txtCheckMessage;

        public bool CanSubmit
        {
            get
            {
                if (obj.UserGroupId == 0)
                {
                    txtCheckMessage = "请选择用户组";
                    return false;
                }
                if (obj.Name == null)
                {
                    txtCheckMessage = "请输入用户名";
                    return false;
                }
                if (obj.Password == null)
                {
                    txtCheckMessage = "请输入密码";
                    return false;
                }
                using (var _db = new ModelContainer())
                {
                    var _user = _db.UserSet.FirstOrDefault(i => i.Name == obj.Name);
                    if (_user == null)
                    {
                        txtCheckMessage = null;
                        return true;
                    }
                    else
                    {
                        txtCheckMessage = "该用户名已被使用";
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
            var _password = obj.Password;
            obj.Password = User.Encrypt(_password);
            try
            {
                using (var _db = new ModelContainer())
                {
                    _db.UserSet.Add(obj);
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
