using DataBaseTool.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DataBaseTool.Common
{
    public static class DataHelper
    {
        /// <summary> 
        ///  DataTable转换成泛型集合
        /// </summary> 
        /// <typeparam name="T">泛型集合类型</typeparam> 
        /// <param name="dt">DataTable</param> 
        /// <param name="dEnum">字典集合，Key为需要从转换为enum项的DataColumnName，Value为需要转换的枚举的类型</param> 
        /// <returns>以实体类为元素的泛型集合</returns> 
        public static List<T> DataTableConvertToListGenuric<T>(this DataTable dt, Dictionary<string, Type> dEnum = null) where T : new()
        {
            if (dt?.Rows.Count > 0)
            {
                // 定义集合 
                List<T> ts = new List<T>();
                // 获得此模型的类型 
                Type type = typeof(T);
                //定义一个临时变量 
                string tempName = string.Empty;
                //遍历DataTable中所有的数据行  
                foreach (DataRow dr in dt.Rows)
                {
                    T t = new T();
                    //如果T是值类型，则先进行装箱
                    object obj = null;
                    if (!t.GetType().IsClass)
                    {
                        obj = t;
                    }
                    //获得此模型的公共属性 
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    //遍历该对象的所有属性 
                    foreach (PropertyInfo pi in propertys)
                    {
                        //将属性名称赋值给临时变量   
                        tempName = pi.Name;
                        //检查DataTable是否包含此列（列名==对象的属性名）     
                        if (dt.Columns.Contains(tempName))
                        {
                            // 判断此属性是否有Setter   
                            if (!pi.CanWrite) continue;//该属性不可写，直接跳出   
                            //取值   
                            object value = dr[tempName];
                            //如果非空，则赋给对象的属性   
                            if (value != DBNull.Value)
                            {
                                //如果有枚举项
                                if (dEnum != null)
                                {
                                    var queryResult = from n in dEnum
                                                      where n.Key == tempName
                                                      select n;
                                    //枚举集合中包含与当前属性名相同的项
                                    if (queryResult.Count() > 0)
                                    {
                                        if (obj != null)
                                        {
                                            //将字符串转换为枚举对象
                                            pi.SetValue(obj, Enum.Parse(queryResult.FirstOrDefault().Value, value.ToString()), null);
                                        }
                                        else
                                        {
                                            //将字符串转换为枚举对象
                                            pi.SetValue(t, Enum.Parse(queryResult.FirstOrDefault().Value, value.ToString()), null);
                                        }
                                    }
                                    else
                                    {
                                        if (obj != null)
                                        {
                                            pi.SetValue(obj, value, null);
                                        }
                                        else
                                        {
                                            pi.SetValue(t, value, null);
                                        }
                                    }
                                }
                                else
                                {
                                    if (obj != null)
                                    {
                                        pi.SetValue(obj, value, null);
                                    }
                                    else
                                    {
                                        pi.SetValue(t, value, null);
                                    }
                                }
                            }
                        }
                    }
                    T ta = default(T);
                    //拆箱
                    if (obj != null)
                    {
                        ta = (T)obj;
                    }
                    else
                    {
                        ta = t;
                    }
                    //对象添加到泛型集合中 
                    ts.Add(ta);
                }
                return ts;
            }
            else
            {
                throw new ArgumentNullException("转换的集合为空.");
            }
        }

        public static List<string> DataTableConvertToListString(this DataTable dt)
        {
            List<string> ts = new List<string>();
            if (dt?.Rows.Count > 0)
            {
                //遍历DataTable中所有的数据行  
                foreach (DataRow dr in dt.Rows)
                {
                    //对象添加到泛型集合中 
                    ts.Add(dr[0].ToString());
                }
            }
            return ts;
        }

        /// <summary>
        /// 泛型集合转换成DataTable
        /// </summary>
        /// <typeparam name="T">泛型集合类型</typeparam>
        /// <param name="list">泛型集合对象</param>
        /// <returns></returns>
        public static DataTable ListGenuricConvertToDataTable<T>(List<T> list)
        {
            if (list?.Count > 0)
            {
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();
                DataTable dt = new DataTable(type.Name);
                foreach (var item in properties)
                {
                    dt.Columns.Add(new DataColumn(item.Name) { DataType = item.PropertyType });
                }
                foreach (var item in list)
                {
                    DataRow row = dt.NewRow();
                    foreach (var property in properties)
                    {
                        row[property.Name] = property.GetValue(item, null);
                    }
                    dt.Rows.Add(row);
                }
                return dt;
            }
            else
            {
                throw new ArgumentNullException("转换的集合为空.");
            }
        }


        public static TEntity GetReaderData<TEntity>(this IDataReader rdr) where TEntity : class, new()
        {
            var item = new TEntity();
            var filedList = new List<string>();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                filedList.Add(rdr.GetName(i).ToLower());
            }
            var properties = typeof(TEntity).GetProperties().Where(s => filedList.Contains(s.Name.ToLower()));
            foreach (var property in properties)
            {
                item.SetValue(property, rdr);
            }
            return item;
        }

        /// <summary>
        /// 设定值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <param name="rdr"></param>
        /// <param name="valueFormatters">值格式化列表</param>
        /// <param name="readName">读取名称</param>
        public static void SetValue<TEntity>(this TEntity entity, PropertyInfo property, IDataReader rdr,
            IDictionary<string, Func<object, object>> valueFormatters = null, string readName = null)
            where TEntity : class, new()
        {
            readName = readName ?? property.Name;

            // 如果有格式化对象则直接返回
            var valueFormatter = valueFormatters.GetVlaue(readName);
            if (valueFormatter != null)
            {
                var currentValue = rdr.GetValue<object>(readName);
                var formatterValue = valueFormatter(currentValue);
                property.SetValue(entity, formatterValue);
                return;
            }

            if (property.PropertyType.Equals(typeof(string)))
            {
                var value = rdr.GetValue<string>(readName);

                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);

            }
            else if (property.PropertyType.Equals(typeof(int)))
            {
                var value = rdr.GetValue<int>(readName);
                if (value == 0)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(int?)))
            {
                var value = rdr.GetValue<int?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(long)))
            {
                var value = rdr.GetValue<long>(readName);
                if (value == 0)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(long?)))
            {
                var value = rdr.GetValue<long?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(decimal)))
            {
                var value = rdr.GetValue<decimal>(readName);
                if (value == 0)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(decimal?)))
            {
                var value = rdr.GetValue<decimal?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(double)))
            {
                var value = rdr.GetValue<double>(readName);
                if (value == 0)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(double?)))
            {
                var value = rdr.GetValue<double?>(readName);
                if (value == null)
                {
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(DateTime)))
            {
                var value = rdr.GetValue<DateTime>(readName);
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(DateTime?)))
            {
                var value = rdr.GetValue<DateTime?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(bool)))
            {
                var value = rdr.GetValue<bool>(readName);
                if (value == false)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(bool?)))
            {
                var value = rdr.GetValue<bool?>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else if (property.PropertyType.Equals(typeof(byte[])))
            {
                var value = rdr.GetValue<byte[]>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
            else
            {
                var value = rdr.GetValue<object>(readName);
                if (value == null)
                {
                    return;
                }
                if (typeof(TEntity) == typeof(object))
                {
                    property.SetValue(entity, value);
                    return;
                }
                property.SetPropertyValue(entity, value);
            }
        }

    }
}
