using System.Collections.Generic;

namespace DataBaseTool.Model
{
    public class TableData
    {
        /// <summary>
        /// 列名
        /// </summary>
        //public List<string> ColumnName { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<Row> RowData { get; set; }
    }

    public class Row
    {
        public List<object> Data { get; set; }
    }
}
