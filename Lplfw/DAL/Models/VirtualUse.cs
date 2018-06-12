using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lplfw.DAL
{
    public partial class VirtualUse
    {
        static public bool AddFromRequisition(int index)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _materials = _db.RequisitionItemSet.Where(i => i.RequisitionId == index).ToList();
                    if (_materials == null) return false;
                    for (var _i = 0; _i < _materials.Count; _i++)
                    {
                        var _material = _materials[_i];
                        var _use = _db.VirtualUseSet.FirstOrDefault(i => i.MaterialId == _material.MaterialId);
                        if (_use == null)
                        {
                            _db.VirtualUseSet.Add(new VirtualUse
                            {
                                MaterialId = _material.MaterialId,
                                Quantity = _material.Quantity
                            });
                        }
                        else
                        {
                            _use.Quantity += _material.Quantity;
                        }
                    }
                    _db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        static public bool RemoveFromRequisition(int index)
        {
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _materials = _db.RequisitionItemSet.Where(i => i.RequisitionId == index).ToList();
                    if (_materials == null) return false;
                    for (var _i = 0; _i < _materials.Count; _i++)
                    {
                        var _material = _materials[_i];
                        var _use = _db.VirtualUseSet.FirstOrDefault(i => i.MaterialId == _material.MaterialId);
                        _use.Quantity -= _material.Quantity;
                    }
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
