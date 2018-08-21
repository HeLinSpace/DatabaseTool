using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseTool.Model;

namespace DataBaseTool.Common
{
    public static class Service
    {
        public static List<string> GetTables(string connStr,DataTypes? dataType,string dataBase)
        {
            switch (dataType)
            {
                case DataTypes.MYSQL:
                    return GetTablesInfoMysql(connStr, dataBase);
                case DataTypes.ORACLE:
                    return GetTablesInfoOracle(connStr, dataBase);
                case DataTypes.SQLSERVER:
                    return GetTablesInfoSqlServer(connStr, dataBase);
                default:
                    return null;
            }
        }

        public static List<string> GetDataBases(string connStr, DataTypes? dataType)
        {
            switch (dataType)
            {
                case DataTypes.MYSQL:
                case DataTypes.ORACLE:
                case DataTypes.SQLSERVER:
                    return GetDataBaseInfoSqlServer(connStr);
                default:
                    return null;
            }
        }

        public static List<FieldInfo> GetTableColumns(string connStr, DataTypes? dataType, string dataBase, string tableName)
        {
            var sql = string.Empty;

            switch (dataType)
            {
                case DataTypes.MYSQL:
                case DataTypes.ORACLE:
                    sql = string.Format(Consts.Fields.OracleFieldsInfo, "USER_TAB_COLS.COLUMN_NAME AS ColumnName, USER_TAB_COLS.DATA_TYPE AS DataType," +
                                                    "USER_TAB_COLS.DATA_LENGTH AS StringLength,USER_TAB_COLS.COLUMN_ID AS OrderNo", tableName);
                    break;
                case DataTypes.SQLSERVER:
                    sql = string.Format(Consts.Fields.SqlServerFieldsInfo, "COLUMN_NAME AS ColumnName,DATA_TYPE AS DataType,CHARACTER_MAXIMUM_LENGTH As StringLength,ORDINAL_POSITION AS OrderNo", dataBase, tableName);
                    return BaseUnity.GetTableColunmSql(connStr, tableName);
                    break;
                default:
                    return null;
            }

            return BaseUnity.QueryForList<FieldInfo>(connStr, dataType, sql, null);
        }

        private static List<string> GetTablesInfoSqlServer(string connStr, string dataBase)
        {

            var sql = string.Format(Consts.Tables.SqlServerAllTables, dataBase);

            return BaseUnity.QueryForListStr(connStr, DataTypes.SQLSERVER, sql, null);
        }

        private static List<string> GetTablesInfoOracle(string connStr, string dataBase)
        {

            var sql = string.Format(Consts.Tables.OracleAllTables, dataBase);

            return BaseUnity.QueryForListStr(connStr, DataTypes.ORACLE, sql, null);
        }

        private static List<string> GetTablesInfoMysql(string connStr, string dataBase)
        {

            var sql = string.Format(Consts.Tables.MysqlAllTables, dataBase);

            return BaseUnity.QueryForListStr(connStr, DataTypes.MYSQL,sql, null);
        }

        private static List<string> GetDataBaseInfoSqlServer(string connStr)
        {

            return BaseUnity.QueryForListStr(connStr,DataTypes.SQLSERVER, Consts.DataBase.SqlServer, null);
        }
    }
}
