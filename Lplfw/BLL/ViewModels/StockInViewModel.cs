using System;

namespace Lplfw.DAL
{
    public class StockInViewModel
    {
        private StockIn obj = new StockIn();

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

        public StockIn Object
        {
            get { return obj; }
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

        public bool CanSubmit
        {
            get
            {
                if (obj.StorageId == 0) return false;
                if (obj.UserId == 0) return false;
                return true;
            }
        }

        public string CheckMessage
        {
            get
            {
                if (obj.StorageId == 0) return "请选择仓库";
                if (obj.UserId == 0) return "请选择负责人";
                return null;
            }
        }
    }
}
