using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class ProductionViewModel
    {
        private Production obj;

        public ProductionViewModel(int userId)
        {
            obj = new Production
            {
                Status = "生产中",
                FinishedAt = null,
                UserId = userId
            };
        }

        public string TxtCode {
            get { return obj.Code; }
            set
            {
                if (value == "") obj.Code = null;
                else obj.Code = value;
            }
        }

        public int? CbProduct
        {
            get { return obj.ProductId; }
            set
            {
                if (value == null) obj.ProductId = 0;
                else obj.ProductId = (int)value;
            }
        }

        public DateTime DpThinkFinishedAt
        {
            get { return obj.ThinkFinishedAt; }
            set { obj.ThinkFinishedAt = value; }
        }

        public string TxtDescription
        {
            get { return obj.Description; }
            set
            {
                if (value == "") obj.Description = null;
                else obj.Description = value;
            }
        }

        private string txtCheckMessage;
        public bool CanSubmit
        {
            get
            {
                if (obj.Code == null)
                {
                    txtCheckMessage = "请输入生产记录号";
                    return false;
                }
                if (obj.ProductId == 0)
                {
                    txtCheckMessage = "请选择产品";
                    return false;
                }
                using (var _db = new ModelContainer())
                {
                    var _old = _db.ProductionSet.FirstOrDefault(i => i.Code == obj.Code);
                    if (_old == null)
                    {
                        txtCheckMessage = null;
                        return true;
                    }
                    else
                    {
                        txtCheckMessage = "生产记录号已存在";
                        return false;
                    }
                }
            }
        }

        public String TxtCheckMessage
        {
            get { return txtCheckMessage; }
        }

        public bool CreateNew()
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    obj.StartAt = DateTime.Now;
                    _db.ProductionSet.Add(obj);
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
