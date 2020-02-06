using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ExcelDataReader;
using UnityEngine;

namespace UnityExcel
{
    public class Excel
    {
        public struct Sheet
        {
            public string name;
            public Cell[] cells;

            public Cell GetCell (int row, int column)
            {
                return cells.FirstOrDefault (c => c.row == row && c.column == column);
            }

            public Cell[] GetRowCells (int row)
            {
                return cells.Where (c => c.row == row).ToArray ();
            }

            public Cell[] GetColumnCells (int column)
            {
                return cells.Where (c => c.column == column).ToArray ();
            }
        }

        public struct Cell
        {
            public int row;
            public int column;
            public string value;
        }

        public string error
        {
            get
            {
                return _error;
            }
        }

        public Sheet[] Sheets
        {
            get
            {
                return _sheets;
            }
        }

        private Sheet[] _sheets = null;
        private string _error = string.Empty;
        private string _name = string.Empty;

        public static bool TryRead (string path, out Excel excel)
        {
            excel = Read (path);
            return string.IsNullOrEmpty (excel._error);
        }

        public static Excel Read (string path)
        {
            var excel = new Excel ();
            try
            {
                using (var stream = File.Open (path, FileMode.Open, FileAccess.Read))
                {
                    var reader = ExcelReaderFactory.CreateOpenXmlReader (stream);
                    if (reader != null)
                    {
                        excel.ParseDataSet (reader.AsDataSet ());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError (e);
                excel._sheets = new Sheet[] { };
                excel._error = e.ToString ();
            }
            return excel;
        }

        public Sheet GetSheet (string name)
        {
            return _sheets.FirstOrDefault (s => s.name == name);
        }

        private void ParseDataSet (DataSet dataSet)
        {
            var sheetList = new List<Sheet> ();
            foreach (DataTable table in dataSet.Tables)
            {
                var sheet = new Sheet ();
                sheet.name = table.TableName;
                var cellList = new List<Cell> ();
                for (int row = 0; row < table.Rows.Count; row++)
                {
                    for (int column = 0; column < table.Columns.Count; column++)
                    {
                        var cell = new Cell ();
                        cell.row = row;
                        cell.column = column;
                        cell.value = table.Rows[row][column].ToString ();
                        cellList.Add (cell);
                    }
                }
                sheet.cells = cellList.ToArray ();
                sheetList.Add (sheet);
            }
            _sheets = sheetList.ToArray ();
        }
    }
}