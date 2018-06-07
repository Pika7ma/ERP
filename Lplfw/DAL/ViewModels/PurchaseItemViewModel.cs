using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class PurchaseItemViewModel
    {
        private PurchaseItem obj;

        public PurchaseItemViewModel() {
            obj = new PurchaseItem
            {
                MaterialId = 0,
                SupplierId = 0,
                PurchaseId = 0,
                Price = 0,
                Quantity = 0,
            };
        }

        public PurchaseItem Object
        {
            get { return obj; }
        }

        public int? CbMaterial
        {
            get { return obj.MaterialId; }
            set
            {
                if (value == null)
                {
                    obj.MaterialId = 0;
                }
                else
                {
                    obj.MaterialId = (int)value;
                }
            }
        }

        public int? CbSupplier
        {
            get { return obj.SupplierId; }
            set
            {
                if (value == null)
                {
                    obj.SupplierId = 0;
                }
                else
                {
                    obj.SupplierId = (int)value;
                }
            }
        }

        public double Price
        {
            get { return obj.Price; }
            set { obj.Price = value; }
        }

        public string TxtQuantity
        {
            get { return obj.Quantity.ToString(); }
            set
            {
                if (int.TryParse(value, out int _quantity))
                {
                    obj.Quantity = _quantity;
                }
                else
                {
                    obj.Quantity = 0;
                }
            }
        }

        public double StartQuantity = 0;

        public double MaxQuantity = 0;

        public bool CanSubmit
        {
            get
            {
                if (obj.MaterialId == 0) return false;
                if (obj.SupplierId == 0) return false;
                if (obj.Quantity < StartQuantity || obj.Quantity > MaxQuantity) return false;
                if (obj.Quantity == 0) return false;
                return true;
            }
        }

        public string TxtCheckMessage
        {
            get
            {
                if (obj.MaterialId == 0) return "请选择材料";
                if (obj.SupplierId == 0) return "请选择供货商";
                if (obj.Quantity < StartQuantity || obj.Quantity > MaxQuantity) return "请填写正确的数量";
                if (obj.Quantity == 0) return "请填写正确的数量";
                return null;
            }
        }
    }
}
