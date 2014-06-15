using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ultra.Web.Core.Common
{
    [Serializable]
    public class ObjectDataMaper<T>
    {
        private static ObjectDataMaper<T> _ent;

        static ObjectDataMaper()
        {
            ObjectDataMaper<T>._ent = new ObjectDataMaper<T>();
        }

        public static DataTable DataTableSechma(params string[] propertyArgs)
        {
            if ((propertyArgs == null) || (propertyArgs.Length < 1))
            {
                return ObjectDataMaper<T>.TableSechma;
            }
            PropertyInfo[] properties = typeof(T).GetProperties();
            if ((properties == null) || (properties.Length < 1))
            {
                return null;
            }
            List<PropertyInfo> list = (from itm in properties
                join a in propertyArgs on itm.Name equals a
                select itm).ToList<PropertyInfo>();
            if ((list == null) || (list.Count < 1))
            {
                return null;
            }
            DataTable table = new DataTable();
            foreach (PropertyInfo info in list)
            {
                if (info.PropertyType.IsGenericType && (info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    table.Columns.Add(new DataColumn(info.Name, info.PropertyType.GetGenericArguments()[0]));
                }
                else
                {
                    table.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
            }
            return table;
        }

        public static DataTable DataTableSechmaEx(string[] propOrFields, out List<PropertyInfo> propList, out List<FieldInfo> fieldList)
        {
            propList = null;
            fieldList = null;
            if ((propOrFields == null) || (propOrFields.Length < 1))
            {
                return null;
            }
            PropertyInfo[] properties = typeof(T).GetProperties();
            FieldInfo[] fields = typeof(T).GetFields();
            List<PropertyInfo> source = (from i in properties
                where propOrFields.Any<string>(j => j == i.Name)
                select i).ToList<PropertyInfo>();
            List<FieldInfo> list2 = (from i in fields
                where propOrFields.Any<string>(j => j == i.Name)
                select i).ToList<FieldInfo>();
            fieldList = list2.ToList<FieldInfo>();
            propList = source.ToList<PropertyInfo>();
            DataTable table = new DataTable();
            foreach (FieldInfo info in list2)
            {
                if (info.FieldType.IsGenericType && (info.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    table.Columns.Add(new DataColumn(info.Name, info.FieldType.GetGenericArguments()[0]));
                }
                else
                {
                    table.Columns.Add(new DataColumn(info.Name, info.FieldType));
                }
            }
            foreach (PropertyInfo info2 in source)
            {
                if (!table.Columns.Contains(info2.Name))
                {
                    if (info2.PropertyType.IsGenericType && (info2.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        table.Columns.Add(new DataColumn(info2.Name, info2.PropertyType.GetGenericArguments()[0]));
                    }
                    else
                    {
                        table.Columns.Add(new DataColumn(info2.Name, info2.PropertyType));
                    }
                }
            }
            return table;
        }

        public DataTable GetDataTableSechma()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            if ((properties == null) || (properties.Length < 1))
            {
                return null;
            }
            DataTable table = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                if (info.PropertyType.IsGenericType && (info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    table.Columns.Add(new DataColumn(info.Name, info.PropertyType.GetGenericArguments()[0]));
                }
                else
                {
                    table.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
            }
            return table;
        }

        public DataTable GetDataTableSechmaEx(string[] propOrFields)
        {
            if ((propOrFields == null) || (propOrFields.Length < 1))
            {
                return null;
            }
            PropertyInfo[] properties = typeof(T).GetProperties();
            FieldInfo[] fields = typeof(T).GetFields();
            List<PropertyInfo> list = (from i in properties
                where propOrFields.Any<string>(j => j == i.Name)
                select i).ToList<PropertyInfo>();
            List<FieldInfo> list2 = (from i in fields
                where propOrFields.Any<string>(j => j == i.Name)
                select i).ToList<FieldInfo>();
            DataTable table = new DataTable();
            foreach (PropertyInfo info in list)
            {
                if (info.PropertyType.IsGenericType && (info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    table.Columns.Add(new DataColumn(info.Name, info.PropertyType.GetGenericArguments()[0]));
                }
                else
                {
                    table.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
            }
            foreach (FieldInfo info2 in list2)
            {
                if (info2.FieldType.IsGenericType && (info2.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    table.Columns.Add(new DataColumn(info2.Name, info2.FieldType.GetGenericArguments()[0]));
                }
                else
                {
                    table.Columns.Add(new DataColumn(info2.Name, info2.FieldType));
                }
            }
            return table;
        }

        public static ObjectDataMaper<T> Entity
        {
            get
            {
                return ObjectDataMaper<T>._ent;
            }
        }

        public static DataTable TableSechma
        {
            get
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                if ((properties == null) || (properties.Length < 1))
                {
                    return null;
                }
                DataTable table = new DataTable();
                foreach (PropertyInfo info in properties)
                {
                    if (info.PropertyType.IsGenericType && (info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        table.Columns.Add(new DataColumn(info.Name, info.PropertyType.GetGenericArguments()[0]));
                    }
                    else
                    {
                        table.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                    }
                }
                return table;
            }
        }
    }
}

