﻿using System.Reflection;
using System.Collections.Concurrent;
using DataBaseTool.Common;

namespace DataBaseTool.Extension
{
    /// <summary>
    /// 属性反射工厂
    /// </summary>
    public static class ReflectionFactory
    {
        /// <summary>
        /// 创建属性工厂
        /// </summary>
        /// <typeparam name="TFrom">主类型</typeparam>
        /// <typeparam name="TTo">从类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="getProperty">获取属性</param>
        /// <param name="setProperty">设定属性</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns>属性访问接口</returns>
        public static IPropertyAccessor<TFrom, TTo, TValue> Create<TFrom, TTo, TValue>(PropertyInfo getProperty,PropertyInfo setProperty , bool isCache)
            where TFrom : class
            where TTo : class
        {
            if (isCache)
            {
                
                var key = typeof(TFrom).Name + "_" + typeof(TTo).Name + getProperty.Name + "_" + setProperty.Name;
                IPropertyAccessor<TFrom, TTo, TValue> propertyAccessor = null;

                if (!Singleton<ConcurrentDictionary<string, IPropertyAccessor<TFrom, TTo, TValue>>>.Instance.TryGetValue(key, out propertyAccessor) || propertyAccessor == null)
                {
                    propertyAccessor = new PropertyAccessor<TFrom, TTo, TValue>(getProperty, setProperty);
                    Singleton<ConcurrentDictionary<string, IPropertyAccessor<TFrom, TTo, TValue>>>.Instance.TryAdd(key, propertyAccessor);
                }
                return propertyAccessor;
            }
            return new PropertyAccessor<TFrom, TTo, TValue>(getProperty, setProperty);
        }
    }
}
