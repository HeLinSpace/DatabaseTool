using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTool.Model
{
    /// <summary>
    /// 表单Api字段配置
    /// </summary>
    public class ConnectConfig
    {

        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataTypes? DataType { get; set; }

        /// <summary>
        /// 数据库链接地址
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 链接名
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DataBase { get; set; }
    }
}
