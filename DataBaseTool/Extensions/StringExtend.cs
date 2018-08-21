using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseTool.Extension
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtend
    {
        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="value">获取的对象</param>
        /// <returns>返回结果</returns>
        public static string EnsureNumericOnly(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            var result = new StringBuilder();
            foreach (char c in value)
            {
                if (Char.IsDigit(c))
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 字符串分割扩展
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <param name="separator">分割符号</param>
        /// <returns>分割结果</returns>
        public static IEnumerable<string> SplitAndRemoveEmptyRepeat(this string input,
            params string[] separator)
        {
            IEnumerable<string> result = new List<string>();
            if (string.IsNullOrWhiteSpace(input) || separator.IsNullOrEmpty())
            {
                return result;
            }
            result = input.Split(separator, StringSplitOptions.RemoveEmptyEntries).RemoveEmptyRepeat();
            return result;
        }

        /// <summary>
        /// 格式化字符串列表（去除空白和重复项）
        /// </summary>
        /// <param name="source">源</param>
        /// <returns>格式化结果</returns>
        public static IEnumerable<string> RemoveEmptyRepeat(this IEnumerable<string> source)
        {
            return source.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct();
        }

        /// <summary>
        /// 根据最大长度截取字符串
        /// </summary>
        /// <param name="value">截取源</param>
        /// <param name="maxLength">截取长度</param>
        /// <returns>截取结果</returns>
        public static string EnsureMaximumLength(this string value, int maxLength)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }
            return value;
        }
    }
}
