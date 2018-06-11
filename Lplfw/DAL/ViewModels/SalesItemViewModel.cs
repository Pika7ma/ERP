using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class SalesItemViewModel
    {
        private SalesItem obj;

        public SalesItemViewModel()
        {
            obj = new SalesItem();
        }

        public SalesItemViewModel(SalesItem item)
        {
            obj = item;
        }

        public SalesItem Object
        {
            get { return obj; }
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

        public string TxtPrice
        {
            get { return obj.Price.ToString(); }
            set
            {
                if (double.TryParse(value, out double _price))
                {
                    obj.Price = _price;
                }
                else
                {
                    obj.Price = 0;
                }
            }
        }

        public bool CanSubmit
        {
            get
            {
                if (obj.ProductId == 0) return false;
                if (obj.Quantity == 0) return false;
                if (obj.Price == 0) return false;
                return true;
            }
        }

        public string TxtCheckMessage
        {
            get
            {
                if (obj.ProductId == 0) return "请选择产品";
                if (obj.Quantity == 0) return "请填写正确的数量";
                if (obj.Price == 0) return "请填写正确的价格";
                return null;
            }
        }
    }
}
