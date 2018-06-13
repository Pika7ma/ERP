using System;
using System.Linq;

namespace Lplfw.DAL
{
    public class StockOutViewModel
    {
        private StockOut obj = new StockOut();

        public int? CbStorage
        {
            get { return obj.StorageId; }
            set
            {
                if (value == null)
                {
                    obj.StorageId = 0;
                }
                else
                {
                    obj.StorageId = (int)value;
                }
            }
        }

        public StockOut Object
        {
            get { return obj; }
        }

        public string TxtCode
        {
            get { return obj.Code; }
            set
            {
                if (value == "") obj.Code = null;
                else obj.Code = value;
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

        public DateTime DpTime
        {
            get { return obj.Time; }
            set { obj.Time = value; }
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

        public string txtCheckMessage;
        public bool CanSubmit
        {
            get
            {
                if (obj.Code == null)
                {
                    txtCheckMessage = "请输入单号";
                    return false;
                }
                if (obj.StorageId == 0)
                {
                    txtCheckMessage = "请选择仓库";
                    return false;
                }
                if (obj.UserId == 0)
                {
                    txtCheckMessage = "请选择负责人";
                    return false;
                }
                using (var _db = new ModelContainer())
                {
                    var _old = _db.StockOutSet.FirstOrDefault(i => i.Code == obj.Code);
                    if (_old == null)
                    {
                        txtCheckMessage = null;
                        return true;
                    }
                    else
                    {
                        txtCheckMessage = "该出库单号已存在";
                        return false;
                    }
                }
            }
        }

        public string TxtCheckMessage
        {
            get { return txtCheckMessage; }
        }
    }
}
