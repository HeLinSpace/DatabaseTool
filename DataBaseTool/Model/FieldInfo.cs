
using Oracle.ManagedDataAccess.Client;
using System;

namespace DataBaseTool.Model
{
    public class FieldInfo
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 字符型长度
        /// </summary>
        public int StringLength { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int OrderNo { get; set; }

    }

    public class FieldsOracle
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public OracleDbType DataType { get; set; }

        /// <summary>
        /// 字符型长度
        /// </summary>
        public int StringLength { get; set; }

    }
}
