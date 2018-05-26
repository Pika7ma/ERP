using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using Lplfw.DAL;

namespace Lplfw.Bll.Bom
{
    /// <summary>
    /// 从excel文件读取数据到Material类中
    /// </summary>
    public class ExcelReader
    {
        private string path;
        private IRow headers;
        private ISheet sheet;
        private int colomnNumbers;
        private int firstRow;
        private int lastRow;
        public int Rows
        {
            get
            {
                return lastRow - firstRow + 1;
            }
        }

        /// <summary>
        /// 打开一个系统默认的文件选择框
        /// </summary>
        /// <returns>如果正确选择了文件,返回true; 否则返回false</returns>
        public bool OpenFileDialog()
        {
            var _openFile = new Microsoft.Win32.OpenFileDialog();
            _openFile.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";
            _openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _openFile.Multiselect = false;

            if (_openFile.ShowDialog() == true)
            {
                path = _openFile.FileName;
                string fileSuffix = Path.GetExtension(path);
                if (string.IsNullOrEmpty(fileSuffix)) return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 开始读取文件中的第一个Sheet
        /// </summary>
        /// <param name="withHeader"></param>
        public void ReadFile(bool withHeader = true)
        {
            var _workbook = WorkbookFactory.Create(path);
            sheet = _workbook.GetSheetAt(0);
            var _headers = sheet.GetRow(0);
            colomnNumbers = _headers.LastCellNum;
            lastRow = sheet.LastRowNum;
            firstRow = sheet.FirstRowNum;

            if (withHeader)
            {
                headers = _headers;
                firstRow++;
            }

        }

        /// <summary>
        /// 将指定行的数据包装为一个Material
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Material ReadLine(int index)
        {
            var _row = sheet.GetRow(index);
            return new Material
            {
                Name = _row.Cells[0].ToString(),
                ClassId = int.Parse(_row.Cells[1].ToString()),
                Format = _row.Cells[2].ToString(),
                Unit = _row.Cells[3].ToString(),
                Price = double.Parse(_row.Cells[4].ToString()),
                Status = _row.Cells[5].ToString()
            };
        }

        /// <summary>
        /// sheet中所有的Material记录
        /// </summary>
        public List<Material> Materials
        {
            get
            {
                var _list = new List<Material>();
                for (var _i = firstRow; _i <= lastRow; _i++)
                {
                    _list.Add(ReadLine(_i));
                }
                return _list;
            }
        }
    }
}
