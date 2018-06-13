using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using Lplfw.DAL;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using NPOI.SS.Util;

namespace Lplfw.BLL
{
    public class ExcelWriter
    {
        private string fileName = null;
        private IWorkbook workbook = null;
        private ISheet sheet = null;
        private FileStream fs = null;
        private string[] headers = null;
        private string sheetName = null;
        private object data = null;
        private Type type;

        public ExcelWriter()
        {
            SelectFileName();
        }

        public ExcelWriter(Type type, object data)
        {
            this.type = type;
            headers = defaultHeaders[(int)type];
            sheetName = defaultSheetName[(int)type];
            this.data = data;
        }

        public bool Write()
        {
            var _open = SelectFileName();
            if (_open == false) return false;
            return WriteToExcel();
        }

        private bool SelectFileName()
        {
            var _win = new Microsoft.Win32.SaveFileDialog();
            _win.Filter = "Excel 2007(*.xlsx)|*.xlsx|Excel 2003(*.xls)|*.xls";
            _win.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _win.AddExtension = true;

            if (_win.ShowDialog() == true)
            {
                fileName = _win.FileName;
                var suffix = _win.FilterIndex;
                switch (suffix)
                {
                    case 1:
                        workbook = new XSSFWorkbook();
                        break;
                    case 2:
                        workbook = new HSSFWorkbook();
                        break;
                    default:
                        break;
                }
                return true;
            }
            return false;
        }

        private bool WriteToExcel()
        {
            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            try
            {
                if (workbook == null) return false;
                sheet = workbook.CreateSheet(sheetName);
                WriteHeaders();
                WriteData();

                workbook.Write(fs);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        private void WriteHeaders()
        {
            IRow row = sheet.CreateRow(0);
            for (var _i = 0; _i < headers.Length; _i++)
            {
                row.CreateCell(_i).SetCellValue(headers[_i]);
            }
        }

        private void WriteData()
        {
            switch (type)
            {
                case Type.Product:
                    WriteProduct();
                    break;
                case Type.Material:
                    WriteMaterial();
                    break;
                case Type.StorageView:
                    WriteStorageView();
                    break;
                case Type.MaterialSafety:
                    WriteMaterialSafety();
                    break;
                case Type.MaterialStorage:
                    WriteMaterialStorage();
                    break;
                case Type.ProductStorage:
                    WriteProductStorage();
                    break;
                case Type.StockIn:
                    WriteStockIn();
                    break;
                case Type.StockOut:
                    WriteStockOut();
                    break;
                case Type.Order:
                    WriteOrder();
                    break;
                case Type.Requistion:
                    WriteRequistion();
                    break;
                case Type.ProduceRecord:
                    WriteProduceRecord();
                    break;
                case Type.MaterialLack:
                    WriteMaterialLack();
                    break;
                case Type.MaterialPrice:
                    WriteMaterialPrice();
                    break;
                case Type.Supplier:
                    WriteSupplier();
                    break;
                case Type.Purchase:
                    WritePurchase();
                    break;
                case Type.Users:
                    WriteUsers();
                    break;
                default:
                    break;
            }
        }

        #region 预设值
        public enum Type
        {
            Product,
            Material,
            StorageView,
            MaterialSafety,
            MaterialStorage,
            ProductStorage,
            StockIn,
            StockOut,
            Order,
            Requistion,
            ProduceRecord,
            MaterialLack,
            MaterialPrice,
            Supplier,
            Purchase,
            Users
        }

        public string[][] defaultHeaders = new string[][] {
            new string[]{ "状态", "名称", "型号", "规格", "单位", "价格" },
            new string[]{ "状态", "名称", "规格", "单位", "平均价格"},
            new string[]{ "仓库名称", "负责人", "位置", "备注"},
            new string[]{ "状态", "名称", "安全库存量"},
            new string[]{ "原料名称", "货位", "数量", "单价", "库存成本"},
            new string[]{ "产品名称", "货位", "数量", "单价", "库存成本"},
            new string[]{ "入库单号", "仓库", "入库时间", "负责人", "备注"},
            new string[]{ "出库单号", "仓库", "出库时间", "负责人", "备注"},
            new string[]{ "状态", "优先级", "订单号", "客户", "联系方式","订货时间", "应付时间", "完成时间", "负责人", "备注"},
            new string[]{ "状态", "领料单号", "订单号", "创建时间", "完成时间", "负责人", "备注"},
            new string[]{ "状态", "生产记录号", "产品", "开始时间", "预计结束时间", "结束时间", "负责人", "备注"},
            new string[]{ "原料名称", "库存", "虚拟用量", "虚拟结余", "安全库存"},
            new string[]{ "原料", "供应商", "价格", "起购数量", "最大数量"},
            new string[]{ "供应商", "联系人", "联系方式", "地址"},
            new string[]{ "状态", "优先级", "创建时间", "完成时间", "备注", "负责人" },
            new string[]{ "工号", "姓名", "联系方式"}
        };

        public string[] defaultSheetName = new string[]
        {
            "产品","原料","仓库","原料安全库存","原料库存","产品库存","入库单","出库单","订单","领料单","生产记录","缺料浏览","材料报价","供应商","采购单","用户"
        };

        private void WriteProduct()
        {
            var _data = data as List<Product>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Status);
                _row.CreateCell(1).SetCellValue(_item.Name);
                _row.CreateCell(2).SetCellValue(_item.Type);
                _row.CreateCell(3).SetCellValue(_item.Format);
                _row.CreateCell(4).SetCellValue(_item.Unit);
                _row.CreateCell(5).SetCellValue(_item.Price);
            }
        }

        private void WriteMaterial()
        {
            var _data = data as List<Material>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Status);
                _row.CreateCell(1).SetCellValue(_item.Name);
                _row.CreateCell(2).SetCellValue(_item.Format);
                _row.CreateCell(3).SetCellValue(_item.Unit);
                _row.CreateCell(4).SetCellValue(_item.Price);
            }
        }

        private void WriteStorageView()
        {
            var _data = data as List<StorageView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Name);
                _row.CreateCell(1).SetCellValue(_item.UserName);
                _row.CreateCell(2).SetCellValue(_item.Location);
                _row.CreateCell(3).SetCellValue(_item.Description);
            }
        }

        private void WriteMaterialSafety()
        {
            var _data = data as List<Material>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Status);
                _row.CreateCell(1).SetCellValue(_item.Name);
                _row.CreateCell(2).SetCellValue(_item.SafeQuantity);
            }
        }

        private void WriteMaterialStorage()
        {
            var _data = data as List<MaterialStockAllView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.MaterialName);
                //_row.CreateCell(1).SetCellValue(_item.Location);
                _row.CreateCell(2).SetCellValue(_item.Quantity);
                _row.CreateCell(3).SetCellValue(_item.Price);
                _row.CreateCell(4).SetCellValue(_item.Cost);
            }
        }

        private void WriteProductStorage()
        {
            var _data = data as List<ProductStockAllView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.ProductName);
                //_row.CreateCell(1).SetCellValue(_item.Location);
                _row.CreateCell(2).SetCellValue(_item.Quantity);
                _row.CreateCell(3).SetCellValue(_item.Price);
                _row.CreateCell(4).SetCellValue(_item.Cost);
            }
        }

        private void WriteStockIn()
        {
            var _data = data as List<StockInView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Code);
                _row.CreateCell(1).SetCellValue(_item.StorageName);
                _row.CreateCell(2).SetCellValue(_item.Time);
                _row.CreateCell(3).SetCellValue(_item.UserName);
                _row.CreateCell(4).SetCellValue(_item.Description);
            }
        }

        private void WriteStockOut()
        {
            var _data = data as List<StockOutView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Code);
                _row.CreateCell(1).SetCellValue(_item.StorageName);
                _row.CreateCell(2).SetCellValue(_item.Time);
                _row.CreateCell(3).SetCellValue(_item.UserName);
                _row.CreateCell(4).SetCellValue(_item.Description);
            }
        }

        private void WriteOrder()
        {
            var _data = data as List<SalesView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Status);
                _row.CreateCell(1).SetCellValue(_item.Priority);
                _row.CreateCell(2).SetCellValue(_item.Code);
                _row.CreateCell(3).SetCellValue(_item.Customer);
                _row.CreateCell(4).SetCellValue(_item.Tel);
                _row.CreateCell(5).SetCellValue(_item.CreateAt);
                _row.CreateCell(6).SetCellValue(_item.DueTime);
                _row.CreateCell(7).SetCellValue((DateTime)_item.FinishedAt);
                _row.CreateCell(8).SetCellValue(_item.UserName);
                _row.CreateCell(9).SetCellValue(_item.Description);
            }
        }

        private void WriteRequistion()
        {
            var _data = data as List<RequisitionView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Status);
                _row.CreateCell(1).SetCellValue(_item.Code);
                _row.CreateCell(2).SetCellValue(_item.SalesCode);
                _row.CreateCell(3).SetCellValue(_item.CreateAt);
                _row.CreateCell(4).SetCellValue((DateTime)_item.FinishedAt);
                _row.CreateCell(5).SetCellValue(_item.UserName);
                _row.CreateCell(6).SetCellValue(_item.Description);
            }
        }

        private void WriteProduceRecord()
        {
            var _data = data as List<ProductionView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Status);
                _row.CreateCell(1).SetCellValue(_item.Code);
                _row.CreateCell(2).SetCellValue(_item.ProductName);
                _row.CreateCell(3).SetCellValue(_item.StartAt);
                _row.CreateCell(4).SetCellValue(_item.ThinkFinishedAt);
                _row.CreateCell(5).SetCellValue((DateTime)_item.FinishedAt);
                _row.CreateCell(6).SetCellValue(_item.UserName);
                _row.CreateCell(7).SetCellValue(_item.Description);
            }
        }

        private void WriteMaterialLack()
        {
            var _data = data as List<MaterialLackView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.MaterialName);
                _row.CreateCell(1).SetCellValue(_item.SumQuantity);
                _row.CreateCell(2).SetCellValue(_item.VirtualUsage);
                _row.CreateCell(3).SetCellValue(_item.VirtualQuantity);
                _row.CreateCell(4).SetCellValue(_item.SafeQuantity);
            }
        }

        private void WriteMaterialPrice()
        {
            var _data = data as List<MaterialPriceView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.MaterialName);
                _row.CreateCell(1).SetCellValue(_item.SupplierName);
                _row.CreateCell(2).SetCellValue(_item.Price);
                _row.CreateCell(3).SetCellValue(_item.StartQuantity);
                _row.CreateCell(4).SetCellValue(_item.MaxQuantity);
            }
        }

        private void WriteSupplier()
        {
            var _data = data as List<Supplier>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Name);
                _row.CreateCell(1).SetCellValue(_item.Contact);
                _row.CreateCell(2).SetCellValue(_item.Tel);
                _row.CreateCell(3).SetCellValue(_item.Location);
            }
        }

        private void WritePurchase()
        {
            var _data = data as List<PurchaseView>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Status);
                _row.CreateCell(1).SetCellValue(_item.Priority);
                _row.CreateCell(2).SetCellValue(_item.CreateAt);
                _row.CreateCell(3).SetCellValue((DateTime)_item.FinishedAt);
                _row.CreateCell(4).SetCellValue(_item.Description);
                _row.CreateCell(5).SetCellValue(_item.Name);
            }
        }

        private void WriteUsers()
        {
            var _data = data as List<User>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i + 1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Id);
                _row.CreateCell(1).SetCellValue(_item.Name);
                _row.CreateCell(2).SetCellValue(_item.Tel);
            }
        }

        #endregion

        #region 各种清单
        public bool WriteRecipe(Product product, List<RecipeView> items)
        {
            try
            {
                fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                if (workbook == null) return false;
                sheet = workbook.CreateSheet("产品配方");

                var _row = sheet.CreateRow(0);
                _row.CreateCell(0).SetCellValue("产品配方清单");
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));
                _row = sheet.CreateRow(1);
                _row.CreateCell(0).SetCellValue("状态");
                _row.CreateCell(1).SetCellValue(product.Status);
                _row.CreateCell(2).SetCellValue("名称");
                _row.CreateCell(3).SetCellValue(product.Name);
                _row.CreateCell(4).SetCellValue("型号");
                _row.CreateCell(5).SetCellValue(product.Type);
                _row = sheet.CreateRow(2);
                _row.CreateCell(0).SetCellValue("规格");
                _row.CreateCell(1).SetCellValue(product.Format);
                _row.CreateCell(2).SetCellValue("单位");
                _row.CreateCell(3).SetCellValue(product.Unit);
                _row.CreateCell(4).SetCellValue("价格");
                _row.CreateCell(5).SetCellValue(product.Price);
                _row = sheet.CreateRow(3);
                _row.CreateCell(0).SetCellValue("原料名称");
                _row.CreateCell(1).SetCellValue("数量");
                _row.CreateCell(2).SetCellValue("单位");

                for (var _i=0; _i<items.Count; _i++)
                {
                    var _item = items[_i];
                    _row = sheet.CreateRow(_i+4);
                    _row.CreateCell(0).SetCellValue(_item.Name);
                    _row.CreateCell(1).SetCellValue(_item.Quantity.ToString());
                    _row.CreateCell(2).SetCellValue(_item.Unit);
                }

                workbook.Write(fs);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

        public bool WriteRequisition(RequisitionView requisition, List<RequisitionItemView> items)
        {
            try
            {
                fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                if (workbook == null) return false;
                sheet = workbook.CreateSheet("物料领取单");

                var _titleStyle = workbook.CreateCellStyle();
                _titleStyle.Alignment = HorizontalAlignment.Center;
                _titleStyle.VerticalAlignment = VerticalAlignment.Center;
                var _titleFont = workbook.CreateFont();
                _titleFont.FontName = "宋体";
                _titleFont.FontHeightInPoints = 18;
                _titleFont.Boldweight = (short)FontBoldWeight.Bold;
                _titleStyle.SetFont(_titleFont);

                var _lineStyle = workbook.CreateCellStyle();
                _lineStyle.BorderBottom = BorderStyle.Thin;
                _lineStyle.Alignment = HorizontalAlignment.Center;

                var _row = sheet.CreateRow(0);
                var _title = _row.CreateCell(0);
                _title.SetCellValue("物料领取单");
                _title.CellStyle = _titleStyle;

                _row = sheet.CreateRow(2);
                for (var _i = 0; _i < 9; _i++)
                {
                    _row.CreateCell(_i).CellStyle = _lineStyle;
                }
                sheet.AddMergedRegion(new CellRangeAddress(0, 2, 0, 8));

                _row = sheet.CreateRow(4);
                _row.CreateCell(0).SetCellValue("领料单号");
                _row.CreateCell(1).SetCellValue(requisition.Code.ToString());
                _row.CreateCell(5).SetCellValue("订单号");
                _row.CreateCell(6).SetCellValue(requisition.SalesCode.ToString());
                _row = sheet.CreateRow(5);
                _row.CreateCell(0).SetCellValue("创建时间");
                _row.CreateCell(1).SetCellValue(requisition.CreateAt.ToString());
                _row.CreateCell(5).SetCellValue("负责人");
                _row.CreateCell(6).SetCellValue(requisition.UserName.ToString());
                _row = sheet.CreateRow(6);
                _row.CreateCell(0).SetCellValue("备注");
                _row.CreateCell(1).SetCellValue(requisition.Description);
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 3));
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 6, 8));
                sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 3));
                sheet.AddMergedRegion(new CellRangeAddress(5, 5, 6, 8));
                sheet.AddMergedRegion(new CellRangeAddress(6, 7, 0, 0));
                sheet.AddMergedRegion(new CellRangeAddress(6, 7, 1, 8));

                _row = sheet.CreateRow(9);
                _row.CreateCell(0).SetCellValue("物料清单");
                for (var _i = 1; _i < 9; _i++)
                {
                    _row.CreateCell(_i).CellStyle = _lineStyle;
                }
                _row = sheet.CreateRow(10);
                for (var _i = 1; _i < 9; _i++)
                {
                    var _cell = _row.CreateCell(_i);
                    _cell.CellStyle = _lineStyle;
                    if (_i == 1) _cell.SetCellValue("原料");
                    if (_i == 5) _cell.SetCellValue("数量");
                }
                sheet.AddMergedRegion(new CellRangeAddress(10, 10, 1, 4));
                sheet.AddMergedRegion(new CellRangeAddress(10, 10, 5, 8));
                var _count = 11;
                // 数据

                for (var _j = 0; _j < items.Count; _j++)
                {
                    _row = sheet.CreateRow(_count);
                    var _item = items[_j];
                    for (var _i = 1; _i < 9; _i++)
                    {
                        var _cell = _row.CreateCell(_i);
                        _cell.CellStyle = _lineStyle;
                        if (_i == 1) _cell.SetCellValue(_item.MaterialName);
                        if (_i == 5) _cell.SetCellValue(_item.Quantity);
                    }
                    sheet.AddMergedRegion(new CellRangeAddress(_count, _count, 1, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(_count, _count, 5, 8));
                    _count++;
                }

                _count = _count + 2;

                _row = sheet.CreateRow(_count);
                _row.CreateCell(0).SetCellValue("领取时间");
                _count++;
                _row = sheet.CreateRow(_count);
                _row.CreateCell(0).SetCellValue("仓库负责人签字");
                _row.CreateCell(5).SetCellValue("生产负责人签字");
                sheet.AddMergedRegion(new CellRangeAddress(_count, _count, 0, 3));
                sheet.AddMergedRegion(new CellRangeAddress(_count, _count, 5, 8));

                workbook.Write(fs);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}
