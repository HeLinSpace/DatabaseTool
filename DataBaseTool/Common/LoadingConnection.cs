using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System;
using Newtonsoft.Json;
using DataBaseTool.Model;

namespace DataBaseTool.Common
{
    public static class LoadingConnection
    {
        private static List<ConnectConfig> ConnectionList { get; set; }

        public static List<ConnectConfig> GetConnectionList()
        {
            StreamReader reader = null;
            try
            {
                if (ConnectionList == null)
                {
                    ConnectionList = new List<ConnectConfig>();
                }
                string config = "./Connection.txt";

                if (!File.Exists(config))
                {
                    var file = File.Create(config);
                    file.Close();
                    return ConnectionList;
                }

                reader = new StreamReader(config, Encoding.UTF8);

                string line = string.Empty;

                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    ConnectConfig connectionStr = JsonConvert.DeserializeObject<ConnectConfig>(line);

                    ConnectionList.Add(connectionStr);
                }

                return ConnectionList;
            }
            catch (Exception e)
            {
                throw (new Exception("初始化失败！！！"));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static bool SetConnection(ConnectConfig configData)
        {
            StreamWriter writer = null;
            try
            {
                if (ConnectionList == null)
                {
                    ConnectionList = new List<ConnectConfig>();
                }

                string config = "./Connection.txt";

                if (!File.Exists(config))
                {
                    var file = File.Create(config);
                    file.Close();
                }

                writer = new StreamWriter(config, true, Encoding.UTF8);

                configData.Id = "N" + GuidExtend.NewId();

                writer.WriteLine(JsonConvert.SerializeObject(configData));

                writer.Close();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("保存登陆信息失败！！！");
            }
        }

        public static ConnectConfig GetConnectionInfo(ConnectConfig config)
        {
            var connection = new ConnectConfig();
            try
            {
                if (ConnectionList == null)
                {
                    return connection;
                }

                connection = ConnectionList.Where(s => s.DataSource == config.DataSource && s.DataType == config.DataType && s.UserID == config.UserID).FirstOrDefault();
            }
            catch
            {
                throw new Exception("初始化失败！！！");
            }
            return connection;
        }

        public static ConnectConfig GetConnectionById(string id,out string connStr)
        {
            var config = new ConnectConfig();

            connStr = string.Empty;

            try
            {
                if (ConnectionList == null)
                {
                    throw new Exception("初始化失败！！！");
                }

                config = ConnectionList.Where(s => s.Id == id).FirstOrDefault();

                switch (config.DataType)
                {
                    case DataTypes.SQLSERVER:
                        connStr = string.Format(Consts.Connection.SqlConnectionStr, config.DataSource, config.UserID, config.PassWord);
                        break;
                    case DataTypes.ORACLE:
                        connStr = string.Format(Consts.Connection.OracleConnectionStr, config.UserID, config.PassWord, config.DataSource, config.Port, config.DataBase);
                        break;
                    case DataTypes.MYSQL:
                        //connStr = string.Format(Consts.MysqlConnectionStr, config.DataSource, config.Port, config.UserID, config.PassWord, config.DataBase);
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                throw new Exception("初始化失败！！！");
            }

            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception("初始化失败！！！");
            }

            return config;
        }

        public static List<ConnectConfig> GetConnectionQuery(ConnectConfig config)
        {
            List<ConnectConfig> connection = new List<ConnectConfig>();
            try
            {
                if (ConnectionList == null)
                {
                    return connection;
                }
                connection = ConnectionList;

                if (!string.IsNullOrEmpty(config.DataSource))
                {
                    connection = connection.Where(s => s.DataSource.Contains(config.DataSource)).ToList();
                }

                if (!string.IsNullOrEmpty(config.UserID))
                {
                    connection = connection.Where(s => s.UserID.Contains(config.UserID)).ToList();
                }

                if (config.DataType != null)
                {
                    connection = connection.Where(s => s.DataType == config.DataType).ToList();
                }

                if (config.PassWord != null)
                {
                    connection = connection.Where(s => s.PassWord == config.PassWord).ToList();
                }
            }
            catch
            {
                throw new Exception("系统错误！！！");
            }
            return connection;
        }
    }
}
