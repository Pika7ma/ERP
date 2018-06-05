using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class MaterialPriceViewModel
    {
        private MaterialPrice obj;

        public MaterialPriceViewModel() {
            obj = new MaterialPrice();
        }

        public MaterialPriceViewModel(MaterialPriceView view)
        {
            obj = new MaterialPrice
            {
                MaterialId = view.MaterialId,
                MaxQuantity = view.MaxQuantity,
                Price = view.Price,
                StartQuantity = view.StartQuantity,
                SupplierId = view.SupplierId
            };
        }

        public int? CbMaterial
        {
            get { return obj.MaterialId; }
            set
            {
                if (value == null) obj.MaterialId = 0;
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
                if (value == null) obj.SupplierId = 0;
                else
                {
                    obj.SupplierId = (int)value;
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
            }
        }

        public string TxtStartQuantity
        {
            get { return obj.StartQuantity.ToString(); }
            set
            {
                if (int.TryParse(value, out int _startQuantity))
                {
                    obj.StartQuantity = _startQuantity;
                }
            }
        }

        public string TxtMaxQuantity
        {
            get { return obj.MaxQuantity.ToString(); }
            set
            {
                if (int.TryParse(value, out int _maxQuantity))
                {
                    obj.MaxQuantity = _maxQuantity;
                }
            }
        }

        public bool CanSubmit
        {
            get
            {
                if (obj.MaterialId == 0) return false;
                if (obj.SupplierId == 0) return false;
                if (obj.Price == 0) return false;
                return true;
            }
        }

        public string TxtCheckMessage
        {
            get
            {
                if (obj.MaterialId == 0) return "请选择正确的原料";
                if (obj.SupplierId == 0) return "请选择正确的供应商";
                if (obj.Price == 0) return "请输入正确的价格";
                return null;
            }
        }

        public bool CreateNew()
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    _db.MaterialPriceSet.Add(obj);
                    _db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _item = _db.MaterialPriceSet.FirstOrDefault(i => i.MaterialId == obj.MaterialId && i.SupplierId == obj.SupplierId);
                    if (_item == null) return false;
                    _item.Price = obj.Price;
                    _item.StartQuantity = obj.StartQuantity;
                    _item.MaterialId = obj.MaterialId;
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
