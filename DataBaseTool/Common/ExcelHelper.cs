using System.IO;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;
using System.Data;
using DataBaseTool.Model;
using System.Linq;
using System;

namespace DataBaseTool.Common
{
    public class ExcelHelper
    {
        private IWorkbook _workBook { get; set; }      //工作文件

        public ExcelHelper(string path)
        {
            FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
            if (Path.GetExtension(path).ToLower() == ".xlsx")
            {
                _workBook = new XSSFWorkbook(fileStream);
            }
            else
            {
                _workBook = new HSSFWorkbook(fileStream);
            }
        }

        private DataTable GetExcelNewData(List<FieldInfo> columns)
        {
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            //新旧字段合并
            for (int i = 0; i < columns.Count(); i++)
            {

                DataColumn column = new DataColumn();
                column.ColumnName = columns[i].ColumnName;
                //获取列类型
                var columnType = columns[i].DataType;
                column.AllowDBNull = true;
                switch (columnType)
                {
                    case "Int32":
                        column.DataType = System.Type.GetType("System.Int32");
                        break;
                    case "Boolean":
                        column.DataType = System.Type.GetType("System.Boolean");
                        break;
                    case "DateTime":
                        column.DataType = System.Type.GetType("System.DateTime");
                        break;
                    case "Decimal":
                        column.DataType = System.Type.GetType("System.Decimal");
                        break;
                    default:
                        column.DataType = System.Type.GetType("System.String");
                        break;
                }
                table.Columns.Add(column);
            }

            for (int i = 0; i < tableTemp.Rows.Count; i++)
            {
                DataRow dataRow = table.NewRow();
                for (int j = 0; j < tableTemp.Columns.Count; j++)
                {
                    if (table.Columns[j].DataType != System.Type.GetType("System.String") && tableTemp.Rows[i][j].ToString() == "")
                    {
                        dataRow[j] = DBNull.Value;
                    }
                    else if (table.Columns[j].DataType == System.Type.GetType("System.Boolean"))
                    {
                        dataRow[j] = tableTemp.Rows[i][j].ToString() == "1" ? false : true;
                    }
                    else
                    {
                        dataRow[j] = tableTemp.Rows[i][j];
                    }
                }
                table.Rows.Add(dataRow);
            }


            return table;
        }

        private object GetCellValue(ICell cell,string dataType)
        {
            var result = new object();

            if (dataType.Contains("Int"))
            {
                result = cell.NumericCellValue;
            }
            else if (dataType == "DateTime")
            {
                result = cell.DateCellValue;
            }
            else
            {
                result = cell.StringCellValue;
            }

            return result;
        }























    }
}
