using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTool.Model
{
    public class ChoseModel
    {
        /// <summary>
        /// 是否选择
        /// </summary>
        public bool CheckeRules { get; set; }

        /// <summary>
        /// 字段key
        /// </summary>
        public string FieldKey { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        public string CompareRule { get; set; }

        /// <summary>
        /// 限定条件Value
        /// </summary>
        public string CompareValue { get; set; }

        /// <summary>
        /// 连接方式
        /// </summary>
        public string Connect { get; set; }

        /// <summary>
        /// Sql
        /// </summary>
        public string Sql { get; set; }

    }
}
