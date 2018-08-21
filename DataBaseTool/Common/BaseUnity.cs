using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DataBaseTool.Model;
using Oracle.ManagedDataAccess.Client;
using DataBaseTool.Extension;

namespace DataBaseTool.Common
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public static class BaseUnity
    {
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool GetConnection(ConnectConfig config)
        {
            bool bRet = false;
            switch (config.DataType)
            {
                case DataTypes.SQLSERVER:
                    bRet = GetConnectionSql(config);
                    break;
                case DataTypes.ORACLE:
                    bRet = GetConnectionOracle(config);
                    break;
                case DataTypes.MYSQL:
                    bRet = GetConnectionMysql(config);
                    break;
                default:
                    break;
            }

            if (bRet)
            {
                WriteConnection(config);
            }

            return bRet;
        }

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool GetTableColumn(string connStr, string table, DataTypes dataType)
        {
            bool bRet = false;
            switch (dataType)
            {
                case DataTypes.SQLSERVER:
                    break;
                case DataTypes.ORACLE:
                    break;
                case DataTypes.MYSQL:
                    break;
                default:
                    break;
            }

            return bRet;
        }

        /// <summary>
        /// 获取DATa Table类型数据
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static DataTable QueryForDataTable(string connStr, DataTypes? dataType, string commandText, params SqlParameter[] parms)
        {
            switch (dataType)
            {
                case DataTypes.SQLSERVER:
                    return GetDataTableSql(connStr, commandText, parms);
                case DataTypes.ORACLE:
                    return GetDataTableOracle(connStr, commandText, parms);
                case DataTypes.MYSQL:
                    return GetDataTableMysql(connStr, commandText, parms);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取泛型 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static List<T> QueryForList<T>(string connStr, DataTypes? dataType, string commandText, params SqlParameter[] parms) where T : new()
        {
            switch (dataType)
            {
                case DataTypes.SQLSERVER:
                    return GetDataTableSql(connStr, commandText, parms).DataTableConvertToListGenuric<T>();
                case DataTypes.ORACLE:
                    return GetDataTableOracle(connStr, commandText, parms).DataTableConvertToListGenuric<T>();
                case DataTypes.MYSQL:
                    return GetDataTableMysql(connStr, commandText, parms).DataTableConvertToListGenuric<T>();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取List<string>
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static List<string> QueryForListStr(string connStr, DataTypes dataType, string commandText, params SqlParameter[] parms)
        {
            switch (dataType)
            {
                case DataTypes.SQLSERVER:
                    return GetDataTableSql(connStr, commandText, parms).DataTableConvertToListString();
                case DataTypes.ORACLE:
                    return GetDataTableOracle(connStr, commandText, parms).DataTableConvertToListString();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取泛型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static T QueryForObject<T>(string connStr, DataTypes dataType, string commandText, params SqlParameter[] parms) where T : new()
        {
            switch (dataType)
            {
                case DataTypes.SQLSERVER:
                    return GetDataTableSql(connStr, commandText, parms).DataTableConvertToListGenuric<T>().FirstOrDefault();
                case DataTypes.ORACLE:
                    return GetDataTableOracle(connStr, commandText, parms).DataTableConvertToListGenuric<T>().FirstOrDefault();
                default:
                    return default(T);
            }
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool BulkInsert(string connStr, DataTypes dataType, string tableName, DataTable dt)
        {
            switch (dataType)
            {
                case DataTypes.SQLSERVER:
                    return BulkInsertSQLServer(connStr, tableName, dt);
                case DataTypes.ORACLE:
                    return BulkInsertOracle(connStr, tableName, dt);
                default:
                    return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static TValue GetTValueData<TValue>(string connStr, DataTypes? dataType, string commandText, params SqlParameter[] parms)
        {
            switch (dataType)
            {
                case DataTypes.SQLSERVER:
                    return GetTValueDataSqlServer<TValue>(connStr, commandText, parms);
                case DataTypes.ORACLE:
                    return GetTValueDataOracle<TValue>(connStr, commandText, parms);
                default:
                    return default(TValue);
            }
        }


        #region 

        private static TValue GetTValueDataSqlServer<TValue>(string connStr, string commandText, params SqlParameter[] parms)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();

                    command.CommandText = commandText;

                    if (parms != null)
                    {
                        command.Parameters.AddRange(parms);
                    }

                    var obj = command.ExecuteScalar();

                    return obj.ToObject<TValue>();

                }
            }
            catch (SqlException e)
            {
                throw (new Exception(string.Format("Error--[{0}]--{1}--{2}", DataTypeStr._SQLSERVER, e.Number, e.Message)));
            }
            catch (Exception e)
            {
                throw (new Exception("发生未知异常，请联系开发人员。"));
            }
        }

        private static TValue GetTValueDataOracle<TValue>(string connStr, string commandText, params SqlParameter[] parms)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connStr))
                {
                    OracleCommand command = conn.CreateCommand();

                    conn.Open();

                    command.CommandText = commandText;

                    if (parms != null)
                    {
                        command.Parameters.AddRange(parms);
                    }

                    var obj = command.ExecuteScalar();

                    return obj.ToObject<TValue>();
                }
            }
            catch (OracleException e)
            {
                throw (new Exception(string.Format("Error--[{0}]--{1}--{2}", DataTypeStr._SQLSERVER, e.Number, e.Message)));
            }
            catch (Exception e)
            {
                throw (new Exception("发生未知异常，请联系开发人员。"));
            }
        }
        #endregion


        #region 数据库链接

        /// <summary>
        /// SqlServer
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static bool GetConnectionSql(ConnectConfig config)
        {
            try
            {
                string connStr = string.Format(Consts.Connection.SqlConnectionStr, config.DataSource, config.UserID, config.PassWord);

                var conn = new SqlConnection(connStr);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Oracle
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static bool GetConnectionOracle(ConnectConfig config)
        {
            try
            {
                string connStr = string.Format(Consts.Connection.OracleConnectionStr, config.UserID, config.PassWord, config.DataSource, config.Port, config.DataBase);

                var conn = new OracleConnection(connStr);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();

                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Mysql
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static bool GetConnectionMysql(ConnectConfig config)
        {
            try
            {
                //string connStr = string.Format(Consts.MysqlConnectionStr, config.DataSource, config.Port, config.UserID, config.PassWord, config.DataBase);

                //ConnectionMysql = new MySqlConnection(connStr);

                //if (ConnectionMysql.State != ConnectionState.Open)
                //{
                //    ConnectionMysql.Open();
                //}

                //if (ConnectionMysql.State == ConnectionState.Open)
                //{
                //    DataBase = config.DataBase;
                //    DataSource = config.DataSource;
                //    DataType = DataTypes.MYSQL;

                //    ConnectionMysql.Close();

                //    return true;
                //}
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region 获取数据绑定DATATABLE
        /// <summary>
        /// Sql Server
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        private static DataTable GetDataTableSql(string connStr, string commandText, params SqlParameter[] parms)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand command = conn.CreateCommand();

                    command.CommandText = commandText;

                    if (parms != null)
                    {
                        command.Parameters.AddRange(parms);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
            catch (SqlException e)
            {
                throw (new Exception(string.Format("Error--[{0}]--{1}--{2}", DataTypeStr._SQLSERVER, e.Number, e.Message)));
            }
            catch (Exception e)
            {
                throw (new Exception("发生未知异常，请联系开发人员。"));
            }
        }

        /// <summary>
        /// Oracle
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        private static DataTable GetDataTableOracle(string connStr, string commandText, params SqlParameter[] parms)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connStr))
                {
                    conn.Open();
                    OracleCommand command = conn.CreateCommand();
                    command.CommandText = commandText;
                    if (parms != null)
                    {
                        command.Parameters.AddRange(parms);
                    }
                    OracleDataAdapter adapter = new OracleDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
            catch (OracleException e)
            {
                throw (new Exception(string.Format("Error--[{0}]--{1}--{2}", DataTypeStr._ORACLE, e.Number, e.Message)));
            }
            catch (Exception e)
            {
                throw (new Exception("发生未知异常，请联系开发人员。"));
            }
        }


        /// <summary>
        /// Mysql
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        private static DataTable GetDataTableMysql(string connStr, string commandText, params SqlParameter[] parms)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = commandText;

                    if (parms != null)
                    {
                        command.Parameters.AddRange(parms);
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
            catch (MySqlException e)
            {
                throw (new Exception(string.Format("Error--[{0}]--{1}--{2}", DataTypeStr._MYSQL, e.Number, e.Message)));
            }
            catch (Exception e)
            {
                throw (new Exception("发生未知异常，请联系开发人员。"));
            }
        }
        #endregion

        #region 批量插入
        /// <summary>
        /// SQL server
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static bool BulkInsertSQLServer(string connStr, string tableName, DataTable dt)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {

                        bulkCopy.DestinationTableName = tableName;//要插入的表的表名
                        bulkCopy.BatchSize = dt.Rows.Count;
                        foreach (var item in dt.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(item.ToString(), item.ToString());//映射字段名 DataTable列名 ,数据库 对应的列名  
                        }

                        bulkCopy.WriteToServer(dt);

                        return true;
                    }
                }
            }
            catch (SqlException e)
            {
                throw (new Exception(string.Format("Error--[{0}]--{1}--{2}", DataTypeStr._SQLSERVER, e.Number, e.Message)));
            }
            catch (Exception e)
            {
                throw (new Exception("发生未知异常，请联系开发人员。"));
            }
        }

        private static bool BulkInsertOracle(string connStr, string tableName, DataTable dt)
        {

            try
            {
                var allFieldsSql = string.Format(Consts.Fields.OracleFieldsInfo, "USER_TAB_COLS.COLUMN_NAME AS ColumnName, USER_TAB_COLS.DATA_TYPE AS DataType," +
                                                    "USER_TAB_COLS.DATA_LENGTH AS StringLength", tableName);

                var allFields = QueryForList<FieldsOracle>(connStr, DataTypes.ORACLE, allFieldsSql, null);

                var sql = "INSTER INTO {0} ({1}) VALUES ({3})";

                using (OracleConnection conn = new OracleConnection(connStr))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    //建立InsertCommand
                    adapter.InsertCommand = new OracleCommand();
                    adapter.InsertCommand.CommandText = string.Format(sql, tableName, string.Join(",", allFields.Select(s => s.ColumnName))
                                                        , string.Join(",", allFields.Select(s => ":" + s.ColumnName)));

                    adapter.InsertCommand.Connection = conn;

                    foreach (var item in allFields)
                    {
                        OracleParameter oparam = new OracleParameter();

                        oparam.ParameterName = "@" + item.ColumnName;

                        if (dt.Columns.Contains(item.ColumnName))
                        {
                            oparam.OracleDbType = item.DataType;

                        }

                        oparam.SourceVersion = DataRowVersion.Current;
                        oparam.SourceColumn = item.ColumnName;
                        adapter.InsertCommand.Parameters.Add(oparam);
                    }
                    int rows = adapter.Update(dt);


                    return rows >= 0 ? true : false;
                }
            }
            catch (OracleException e)
            {
                throw (new Exception(string.Format("Error--[{0}]--{1}--{2}", DataTypeStr._ORACLE, e.Number, e.Message)));
            }
            catch (Exception e)
            {
                throw (new Exception("发生未知异常，请联系开发人员。"));
            }
        }


        #endregion

        #region 获取表字段属性
        public static List<FieldInfo> GetTableColunmSql(string connStr, string table)
        {
            var result = new List<FieldInfo>();
            var sql = string.Format(Consts.Fields.SqlServerFieldsBySelect, table);

            var dt = GetDataTableSql(connStr, sql,null);

            foreach (DataColumn column in dt.Columns)
            {
                var temp = new FieldInfo();
                temp.OrderNo = column.Ordinal;
                temp.ColumnName = column.ColumnName;
                temp.DataType = column.DataType.Name;

                result.Add(temp);
            }
            return result;
        }


        #endregion

        private static void WriteConnection(ConnectConfig config)
        {
            var data = LoadingConnection.GetConnectionQuery(config);
            if (data == null || data.Count <= 0)
            {
                LoadingConnection.SetConnection(config);
            }
        }
    }
}

