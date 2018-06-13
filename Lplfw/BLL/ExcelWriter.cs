﻿using System;
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
                default:
                    break;
            }
        }

        #region 预设值
        public enum Type
        {
            Product,
        }

        public string[][] defaultHeaders = new string[][] {
            new string[]{ "状态", "名称", "型号", "规格", "单位", "价格" }
        };

        public string[] defaultSheetName = new string[]
        {
            "产品"
        };

        private void WriteProduct()
        {
            var _data = data as List<Product>;
            for (var _i = 0; _i < _data.Count; _i++)
            {
                IRow _row = sheet.CreateRow(_i+1);
                var _item = _data[_i];
                _row.CreateCell(0).SetCellValue(_item.Status);
                _row.CreateCell(1).SetCellValue(_item.Name);
                _row.CreateCell(2).SetCellValue(_item.Type);
                _row.CreateCell(3).SetCellValue(_item.Format);
                _row.CreateCell(4).SetCellValue(_item.Unit);
                _row.CreateCell(5).SetCellValue(_item.Price);
            }
        }
        #endregion

        #region 各种清单
        public bool WriteRecipe(Product product, List<RecipeView> items)
        {
            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            try
            {
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
        #endregion
    }
}
