namespace DataBaseTool.Model
{
    public enum DataTypes
    {
        ORACLE,
        SQLSERVER,
        MYSQL
    }

    public enum ItemTypes
    {
        Server,
        DataBase,
        Table
    }

    public enum DataSourceFormats
    {
        JSON,
        EXCEL,
        XML,
        CSV
    }

    /// <summary>
    /// 数据库类型名称
    /// </summary>
    public struct DataTypeStr
    {
        /// <summary>
        /// ORACLE
        /// </summary>
        public const string _ORACLE = "Oracle";

        /// <summary>
        /// SQLSERVER
        /// </summary>
        public const string _SQLSERVER = "SQLServer";

        /// <summary>
        /// MYSQL
        /// </summary>
        public const string _MYSQL = "Mysql";

    }

    public static class DataTypeExtend
    {
        public static string GetName(this DataTypes types)
        {
            switch (types)
            {
                case DataTypes.MYSQL:
                    return DataTypeStr._MYSQL;
                case DataTypes.ORACLE:
                    return DataTypeStr._ORACLE;
                case DataTypes.SQLSERVER:
                    return DataTypeStr._SQLSERVER;
                default:
                    return null;
            }
        }
    }
}
