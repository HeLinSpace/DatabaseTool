﻿using System.Collections.Generic;

namespace DataBaseTool.Extension
{
    /// <summary>
    /// 字典扩展
    /// </summary>
    public static class IDictionaryExtend
    {
        /// <summary>
        /// 根据key获取value
        /// </summary>
        /// <typeparam name="TKey">key类型</typeparam>
        /// <typeparam name="TValue">value类型</typeparam>
        /// <param name="dic">字典</param>
        /// <param name="key">key值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>获取的value值</returns>
        public static TValue GetVlaue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue = default(TValue))
        {
            var resultValue = default(TValue);
            if (dic == null)
            {
                return defaultValue;
            }
            if (dic.TryGetValue(key, out resultValue))
            {
                return resultValue;
            }
            return defaultValue;
        }
    }
}
