using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTool.Common
{
    public class Consts
    {

        /// <summary>
        /// 链接用SQL
        /// </summary>
        public struct Connection
        {

            /// <summary>
            /// SqlServer 链接用SQL
            /// {0} 数据库地址 {1} 用户名 {2} 密码
            /// </summary>
            public const string SqlConnectionStr = "Data Source = {0};Integrated Security = False; Persist Security Info=False;User ID = {1}; Password={2};MultipleActiveResultSets=True;";

            public const string SqlConnEx = "{0} Initial Catalog={1};";

            /// <summary>
            /// Oracle 链接用SQL
            /// {0} 用户ID {1}密码 {2} 数据库地址 {3}端口号 {4}数据库名
            /// </summary>
            public const string OracleConnectionStr = "User ID={0};Password={1};Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = {2})(PORT = {3}))) (CONNECT_DATA = (SERVICE_NAME = {4})))";

            /// <summary>
            /// Mysql 链接用SQL
            /// {0} 数据库地址 {1}用户ID {2} 密码 {3}数据库名
            /// </summary>
            public const string MysqlConnectionStr = "Server={0};port={1};User ID = {2}; Password={3};SslMode = none;";
        }

        /// <summary>
        /// 获取所有表名
        /// </summary>
        public struct Tables
        {
            /// <summary>
            /// Sql Server 获取所有表名
            /// {0} 数据库名
            /// </summary>
            //public const string SqlServerAllTables = @"SELECT obj.name TableName,schem.name schemname,idx.rows,
            //                        CAST
            //                        (
            //                            CASE 
            //                                WHEN (SELECT COUNT(1) FROM sys.indexes WHERE object_id= obj.OBJECT_ID AND is_primary_key=1) >=1 THEN 1
            //                                ELSE 0
            //                            END 
            //                        AS BIT) HasPrimaryKey                                         
            //                        from {0}.sys.objects obj 
            //                        inner join {0}.dbo.sysindexes idx on obj.object_id=idx.id and idx.indid<=1
            //                        INNER JOIN {0}.sys.schemas schem ON obj.schema_id=schem.schema_id
            //                        where type='U' order by obj.name";

            public const string SqlServerAllTables = @"SELECT obj.name TableName
                                    from {0}.sys.objects obj 
                                    where type='U' order by obj.name ASC";

            /// <summary>
            /// Oracle 获取所有表名
            /// {0} 数据库名
            /// </summary>
            public const string OracleAllTables = "SELECT TABLE_NAME as TableName FROM USER_TABLES ORDER BY TABLE_NAME ASC";

            /// <summary>
            /// Mysql 获取所有表名
            /// {0} 数据库名
            /// </summary>
            public const string MysqlAllTables = "select table_name as TableName from information_schema.TABLES where TABLE_SCHEMA='{0}' ORDER BY table_name ASC";
        }

        /// <summary>
        /// 获取所有字段属性
        /// </summary>
        public struct Fields
        {
            /// <summary>
            /// Sql Server 获取所有字段属性（单表）
            /// {0}查询字段 {1} 数据库 {2} 表名
            /// </summary>
            public const string SqlServerFieldsInfo = "SELECT {0} from {1}.INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME = '{2}'";

            /// <summary>
            /// Oracle 获取所有字段属性（单表）
            /// {0}查询字段 {1} 表名
            /// </summary>
            public const string OracleFieldsInfo = @"SELECT {0}
                                                FROM
                                                USER_TAB_COLS
                                                INNER JOIN user_col_comments ON user_col_comments.TABLE_NAME = USER_TAB_COLS.TABLE_NAME
                                                AND user_col_comments.COLUMN_NAME = USER_TAB_COLS.COLUMN_NAME
                                                AND USER_TAB_COLS.TABLE_NAME='{1}'";

            public const string SqlServerFieldsBySelect = "SELECT Top(0) * from {0}";

        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public struct Data
        {
            /// <summary>
            /// 获取数据（单表）
            /// {0} 查询字段 {1}Order By {2} 数据库 {3}表 {4} 开始行 {5} 结束行
            /// </summary>
            public const string SqlTableData = "Select {0} From (Select row_number() over(order by {1}) as RowNo,* From {2} {5}) as t where t.RowNo >= {3} and t.RowNo <= {4}";

            /// <summary>
            /// 查询数据总条数
            /// {0} 数据库 {1}表
            /// </summary>
            public const string TableDataCount = "Select count(1) From {0}";

            /// <summary>
            /// 
            /// </summary>
            public const string OracleTableData = "Select {0} From (Select rownum, * From {1} {4} where rownum >= {2} and rownum <= {3})";

            /// <summary>
            /// Mysql获取数据（单表）
            /// {0} 查询字段 {1}表名
            /// </summary>
            public const string MySqlTableData = "SELECT {0} FROM {1} limit(100)";

        }

        /// <summary>
        /// 查询所有数据库函数
        /// </summary>
        public struct Function
        {
            /// <summary>
            /// Oracle 查询所有数据库函数
            /// {0} 查询字段
            /// </summary>
            public const string OracleAllFunction = "SELECT {0} FROM all_objects WHERE OBJECT_TYPE='FUNCTION';";

            /// <summary>
            /// SQL Server 查询所有数据库函数
            /// {0} 查询字段
            /// </summary>
            public const string SqlServerAllFunction = "SELECT {0} FROM sysobjects WHERE xtype in ('P','FN')";
        }

        /// <summary>
        /// 获取数据库名
        /// </summary>
        public struct DataBase
        {
            public const string SqlServer = "SELECT name FROM  master..sysdatabases WHERE name NOT IN ( 'master', 'model', 'msdb', 'tempdb', 'northwind','pubs' )";

        }
        
    }
}
