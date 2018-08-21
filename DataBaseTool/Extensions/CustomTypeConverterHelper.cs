using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataBaseTool.Extension
{
    /// <summary>
    /// 自定义类型转换
    /// </summary>
    public static class CustomTypeConverterHelper
    {
        /// <summary>
        /// 获取需要转换的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeConverter GetCustomTypeConverter(Type type)
        {
            if (type == typeof(List<int>))
            {
                return new GenericListTypeConverter<int>();
            }
            if (type == typeof(List<int?>))
            {
                return new GenericListTypeConverter<int?>();
            }
            if (type == typeof(List<decimal>))
            {
                return new GenericListTypeConverter<decimal>();
            }
            if (type == typeof(List<decimal?>))
            {
                return new GenericListTypeConverter<decimal?>();
            }
            if (type == typeof(List<double>))
            {
                return new GenericListTypeConverter<double>();
            }
            if (type == typeof(List<double?>))
            {
                return new GenericListTypeConverter<double?>();
            }
            if (type == typeof(List<DateTime>))
            {
                return new GenericListTypeConverter<DateTime>();
            }
            if (type == typeof(List<string>))
            {
                return new GenericListTypeConverter<string>();
            }
            return TypeDescriptor.GetConverter(type);
        }
    }
}
