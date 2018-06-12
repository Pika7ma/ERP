using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class SalesViewModel
    {
        private Sales obj;
        private List<SalesItem> items;

        public SalesViewModel(int userId)
        {
            var _now = DateTime.Now;
            obj = new Sales
            {
                UserId = userId,
                Status = "处理中",
                CreateAt = _now,
                DueTime = _now,
                FinishedAt = null,
                RequsitionStatus = false
            };
            items = new List<SalesItem>();
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

        public string TxtCustomer
        {
            get { return obj.Customer; }
            set
            {
                if (value == "") obj.Customer = null;
                else obj.Customer = value;
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

        public string CbPriority
        {
            get { return obj.Priority; }
            set
            {
                if (value == "") obj.Priority = null;
                else obj.Priority = value;
            }
        }

        public DateTime DpCreateAt
        {
            get { return obj.CreateAt; }
            set { obj.CreateAt = value; }
        }

        public DateTime DpDueTime
        {
            get { return obj.DueTime; }
            set { obj.DueTime = value; }
        }

        public string TxtDescription
        {
            get { return obj.Description; }
            set {
                if (value == "") obj.Description = null;
                else obj.Description = value;
            }
        }

        public bool CanSubmit
        {
            get
            {
                if (obj.Code == null)
                {
                    txtCheckMessage = "请填写订单号";
                    return false;
                }
                if (obj.Customer == null)
                {
                    txtCheckMessage = "请填写客户";
                    return false;
                }
                if (obj.Tel == null)
                {
                    txtCheckMessage = "请填写联系方式";
                    return false;
                }
                if (obj.Priority == null)
                {
                    txtCheckMessage = "请选择优先级";
                    return false;
                }
                if (obj.CreateAt > obj.DueTime)
                {
                    txtCheckMessage = "请选择正确的时间";
                    return false;
                }
                if (items.Count == 0)
                {
                    txtCheckMessage = "请创建订单条目";
                    return false;
                }
                using (var _db = new ModelContainer())
                {
                    var _sales = _db.SalesSet.FirstOrDefault(i => i.Code == obj.Code);
                    if (_sales == null)
                    {
                        txtCheckMessage = null;
                        return true;
                    }
                    else
                    {
                        txtCheckMessage = "订单号已存在, 请重新填写";
                        return false;
                    }
                }
            }
        }

        private string txtCheckMessage;
        public string TxtCheckMessage
        {
            get
            {
                return txtCheckMessage;
            }
        }

        public List<SalesItem> Items
        {
            get { return items; }
        }

        public void AddItem(SalesItem item)
        {
            var _old = items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (_old == null)
            {
                items.Add(item);
            }
            else
            {
                _old.Quantity += item.Quantity;
                _old.Price = item.Price;
            }
        }

        public bool EditItem(SalesItem item)
        {
            var _old = items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (_old == null)
            {
                return false;
            }
            else
            {
                _old.Quantity = item.Quantity;
                _old.Price = item.Price;
                return true;
            }
        }

        public bool DeleteItem(SalesItem item)
        {
            var _old = items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (_old == null)
            {
                return false;
            }
            else
            {
                items.Remove(_old);
                return true;
            }
        }

        public bool CreateNew()
        {
            try { 
                using (var _db = new ModelContainer())
                {
                    _db.SalesSet.Add(obj);
                    _db.SaveChanges();
                    for (var _i = 0; _i < items.Count; _i++)
                    {
                        items[_i].SalesId = obj.Id;
                    }
                    _db.SalesItemSet.AddRange(items);
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
