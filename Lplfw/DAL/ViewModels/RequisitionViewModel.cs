using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class RequisitionViewModel
    {
        private Requisition obj;
        private List<RequisitionItem> items;
        public Requisition Object { get; set; }

        public RequisitionViewModel(int userId, int salesId)
        {
            obj = new Requisition
            {
                Status = "处理中",
                UserId = userId,
                SalesId = salesId
            };

            items = new List<RequisitionItem>();
            var _requisitionMaterials = RequisitionProcedure.GetById(salesId);
            for (var _i = 0; _i<_requisitionMaterials.Count; _i++)
            {
                items.Add(new RequisitionItem
                {
                    MaterialId = _requisitionMaterials[_i].MaterialId,
                    Quantity = _requisitionMaterials[_i].MaterialQuantity
                });
            }
        }

        public string TxtCode {
            get {
                return obj.Code;
            }
            set {
                if (value == "") obj.Code = null;
                else obj.Code = value;
            }
        }
        public string TxtDescription
        {
            get
            {
                return obj.Description;
            }
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
                    txtCheckMessage = "请输入物料单号";
                    return false;
                }
                using (var _db = new ModelContainer())
                {
                    var _item = _db.RequisitionSet.FirstOrDefault(i => i.Code == obj.Code);
                    if (_item == null)
                    {
                        txtCheckMessage = null;
                        return true;
                    }
                    else
                    {
                        txtCheckMessage = "该物料单号已存在";
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
                    obj.CreateAt = DateTime.Now;
                    _db.RequisitionSet.Add(obj);
                    _db.SaveChanges();
                    for (var _i = 0; _i < items.Count; _i++)
                    {
                        items[_i].RequisitionId = obj.Id;
                    }
                    _db.RequisitionItemSet.AddRange(items);
                    var _sales = _db.SalesSet.FirstOrDefault(i => i.Id == obj.SalesId);
                    _sales.RequsitionStatus = true;
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
