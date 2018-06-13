using System;
using System.Collections.Generic;
using System.Linq;

namespace Lplfw.DAL
{
    public class ProductStockAllView
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }

        static public List<ProductStockAllView> GetAll()
        {
            try
            {
                var _list = new List<ProductStockAllView>();
                var _stocks = ProductStockView.GetAllGrouped();
                _stocks.ForEach(i =>
                {
                    var _stockAll = new ProductStockAllView();
                    var bar = true;
                    foreach (var item in i as IGrouping<int, ProductStockView>)
                    {
                        if (bar == true)
                        {
                            _stockAll.ProductId = item.ProductId;
                            _stockAll.ProductName = item.ProductName;
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
