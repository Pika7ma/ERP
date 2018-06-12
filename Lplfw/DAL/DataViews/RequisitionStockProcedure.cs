using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public class RequisitionStockProcedure
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public int Quantity { get; set; }

        static public List<RequisitionStockProcedure> GetById(int index)
        {
            var _list = new List<RequisitionStockProcedure>();
            var _stocks = MaterialStockAllView.GetAll();
            var _materials = RequisitionItemView.GetById(index);
            List<VirtualUse> _uses;
            using (var _db = new ModelContainer())
            {
                _uses = _db.VirtualUseSet.ToList();
            }
            for (var _i = 0; _i < _materials.Count; _i++)
            {
                MaterialStockAllView _stock;
                if (_stocks == null) _stock = null;
                else _stock = _stocks.FirstOrDefault(i => i.MaterialId == _materials[_i].MaterialId);
                var _item = new RequisitionStockProcedure
                {
                    MaterialId = _materials[_i].MaterialId,
                    MaterialName = _materials[_i].MaterialName,
                    Quantity = 0
                };
                if (_stock != null)
                {
                    _item.Quantity = _stock.Quantity;
                }

                var _use = _uses.FirstOrDefault(i => i.MaterialId == _materials[_i].MaterialId);
                if (_use != null)
                {
                    _item.Quantity -= _use.Quantity;
                }
                _list.Add(_item);
            }
            return _list;
        }
    }
}
