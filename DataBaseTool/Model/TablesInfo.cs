namespace DataBaseTool.Model
{
    public class TablesInfo
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表的架构
        /// </summary>
        public string SchemaName { get; set; }
        /// <summary>
        /// 表的记录数
        /// </summary>
        public DataTypes Rows { get; set; }

        /// <summary>
        /// 是否含有主键
        /// </summary>
        public bool HasPrimaryKey { get; set; }
    }
}
