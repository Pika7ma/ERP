using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class RecipeItemViewModel
    {
        private RecipeItem obj;

        public RecipeItemViewModel()
        {
            obj = new RecipeItem();
        }

        public RecipeItemViewModel(RecipeItem item)
        {
            obj = item;
        }

        public RecipeItem Object
        {
            get { return obj; }
        }

        public int? CbMaterial
        {
            get { return obj.MaterialId; }
            set {
                if (value == null) obj.MaterialId = 0;
                else obj.MaterialId = (int)value;
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

        public bool CanSubmit
        {
            get
            {
                if (obj.MaterialId == 0) return false;
                if (obj.Quantity == 0) return false;
                else return true;
            }
        }

        public string TxtCheckMessage
        {
            get
            {
                if (obj.MaterialId == 0) return "请选择原料";
                if (obj.Quantity == 0) return "请填写正确的数量";
                else return null;
            }
        }
    }
}
