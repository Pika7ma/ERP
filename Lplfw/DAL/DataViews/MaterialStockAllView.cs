using System;
using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class MaterialStockAllView
    {
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public string MaterialName { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }

        static public List<MaterialStockAllView> GetAll()
        {
            try
            {
                var _list = new List<MaterialStockAllView>();
                var _stocks = MaterialStockView.GetAllGrouped();
                _stocks.ForEach(i =>
                {
                    var _stockAll = new MaterialStockAllView();
                    var bar = true;
                    foreach (var item in i as IGrouping<int, MaterialStockView>)
                    {
                        if (bar == true)
                        {
                            _stockAll.MaterialId = item.MaterialId;
                            _stockAll.MaterialName = item.MaterialName;
                            _stockAll.Price = item.Price;
                            bar = false;
                        }
                        _stockAll.Quantity += item.Quantity;
                    }
                    _stockAll.Cost = _stockAll.Price * _stockAll.Quantity;
                    _list.Add(_stockAll);
                });
                return _list;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
