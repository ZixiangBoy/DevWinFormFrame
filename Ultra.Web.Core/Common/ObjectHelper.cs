using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ultra.Web.Core.Common
{
    public static class ObjectHelper
    {
        public static T Create<T>(DataRow dr) where T: new()
        {
            if (dr == null)
            {
                return default(T);
            }
            if (dr.Table.Columns.Count < 1)
            {
                return default(T);
            }
            return ResloveSelfProperty<T>(dr);
        }

        public static List<T> Create<T>(DataRowCollection drcolection) where T: new()
        {
            if ((drcolection == null) || (drcolection.Count < 1))
            {
                return null;
            }
            List<T> list = new List<T>(drcolection.Count);
            foreach (DataRow row in drcolection)
            {
                list.Add(Create<T>(row));
            }
            return list;
        }

        public static List<T> Create<T>(DataRow[] drs) where T: new()
        {
            if ((drs == null) || (drs.Length < 1))
            {
                return null;
            }
            List<T> list = new List<T>(drs.Length);
            foreach (DataRow row in drs)
            {
                list.Add(Create<T>(row));
            }
            return list;
        }

        public static List<T> Create<T>(DataTable dt) where T: new()
        {
            if (dt == null)
            {
                return null;
            }
            return Create<T>(dt.Rows);
        }

        public static T DeepCopy<T>(T objsrc)
        {
            if (objsrc == null)
            {
                return objsrc;
            }
            return DeSerialize<T>(Serialize<T>(objsrc));
        }

        public static T DeSerialize<T>(string jsonstr)
        {
            return JsonConvert.DeserializeObject<T>(jsonstr);
        }

        public static T DeSerialize<T>(byte[] byts)
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(byts))
            {
                return (T) formatter.Deserialize(stream);
            }
        }

        public static List<Type> GetAllClassOfClass(Type tbase, Assembly asm)
        {
            if (null == asm)
            {
                return null;
            }
            Type[] types = asm.GetTypes();
            if (null == tbase)
            {
                return types.ToList<Type>();
            }
            return (from k in types
                where IsInheritClass(tbase, k)
                select k).ToList<Type>();
        }

        public static List<Type> GetAllClassOfClass(Type tbase, string asmFile)
        {
            if (!File.Exists(asmFile))
            {
                return null;
            }
            Assembly asm = LoadAsm(asmFile);
            return GetAllClassOfClass(tbase, asm);
        }

        public static List<Type> GetAllClassOfInterface(Assembly asm, string interfaceName = "")
        {
            if (null == asm)
            {
                return null;
            }
            Type[] types = asm.GetTypes();
            if (string.IsNullOrEmpty(interfaceName))
            {
                return types.ToList<Type>();
            }
            return (from k in types
                where IsInheritInterface(interfaceName, k)
                select k).ToList<Type>();
        }

        public static List<Type> GetAllClassOfInterface(string asmFile, string interfaceName = "")
        {
            if (!File.Exists(asmFile))
            {
                return null;
            }
            return GetAllClassOfInterface(LoadAsm(asmFile), interfaceName);
        }

        public static FieldInfo[] GetAllField(Type t)
        {
            return t.GetFields();
        }

        public static PropertyInfo[] GetAllProperty(Type t)
        {
            return t.GetProperties();
        }

        public static FieldInfo[] GetField<T>()
        {
            return typeof(T).GetFields();
        }

        public static string GetFieldDesc(FieldInfo fi)
        {
            if (null == fi)
            {
                return string.Empty;
            }
            object[] customAttributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if ((customAttributes == null) || (customAttributes.Count<object>() < 1))
            {
                return string.Empty;
            }
            DescriptionAttribute attribute = customAttributes.First<object>() as DescriptionAttribute;
            if (attribute == null)
            {
                return string.Empty;
            }
            return attribute.Description;
        }

        public static PropertyInfo[] GetProPerty<T>()
        {
            return typeof(T).GetProperties();
        }

        public static string GetPropertyDesc(PropertyInfo pi)
        {
            if (null == pi)
            {
                return string.Empty;
            }
            object[] customAttributes = pi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if ((customAttributes == null) || (customAttributes.Count<object>() < 1))
            {
                return string.Empty;
            }
            DescriptionAttribute attribute = customAttributes.First<object>() as DescriptionAttribute;
            if (attribute == null)
            {
                return string.Empty;
            }
            return attribute.Description;
        }

        public static string GetTypeUseage(FieldInfo fi)
        {
            if (IsRequired(fi))
            {
                return (fi.FieldType.GetGenericArguments()[0].FullName + "?");
            }
            return fi.FieldType.Name;
        }

        public static string GetTypeUseage(PropertyInfo pi)
        {
            if (!IsRequired(pi))
            {
                return (pi.PropertyType.GetGenericArguments()[0].FullName + "?");
            }
            return pi.PropertyType.Name;
        }

        public static bool IsBool(PropertyInfo pi)
        {
            Type propertyType = pi.PropertyType;
            if (!(propertyType == typeof(bool)))
            {
                return (propertyType == typeof(bool?));
            }
            return true;
        }

        public static bool IsDateTime(PropertyInfo pi)
        {
            Type propertyType = pi.PropertyType;
            if (!(propertyType == typeof(DateTime)))
            {
                return (propertyType == typeof(DateTime?));
            }
            return true;
        }

        public static bool IsGuid(PropertyInfo pi)
        {
            Type propertyType = pi.PropertyType;
            if (!(propertyType == typeof(Guid)))
            {
                return (propertyType == typeof(Guid?));
            }
            return true;
        }

        public static bool IsInheritClass(Type tbase, Type t)
        {
            return t.IsSubclassOf(tbase);
        }

        public static bool IsInheritInterface(string interfaceName, Type t)
        {
            return (null != t.GetInterface(interfaceName, true));
        }

        public static bool IsNumber(PropertyInfo pi)
        {
            Type propertyType = pi.PropertyType;
            if (((((!(propertyType == typeof(int)) && !(propertyType == typeof(int?))) && (!(propertyType == typeof(int)) && !(propertyType == typeof(int?)))) && ((!(propertyType == typeof(long)) && !(propertyType == typeof(long?))) && (!(propertyType == typeof(short)) && !(propertyType == typeof(short?))))) && (((!(propertyType == typeof(uint)) && !(propertyType == typeof(uint?))) && (!(propertyType == typeof(float)) && !(propertyType == typeof(float?)))) && ((!(propertyType == typeof(double)) && !(propertyType == typeof(double?))) && (!(propertyType == typeof(double)) && !(propertyType == typeof(double?)))))) && ((((!(propertyType == typeof(ushort)) && !(propertyType == typeof(ushort?))) && (!(propertyType == typeof(uint)) && !(propertyType == typeof(uint?)))) && ((!(propertyType == typeof(ulong)) && !(propertyType == typeof(ulong?))) && (!(propertyType == typeof(decimal)) && !(propertyType == typeof(decimal?))))) && (((!(propertyType == typeof(decimal)) && !(propertyType == typeof(decimal?))) && (!(propertyType == typeof(sbyte)) && !(propertyType == typeof(sbyte?)))) && !(propertyType == typeof(sbyte)))))
            {
                return (propertyType == typeof(sbyte?));
            }
            return true;
        }

        public static bool IsRequired(FieldInfo fi)
        {
            if (null == fi)
            {
                return false;
            }
            Type fieldType = fi.FieldType;
            return (fieldType.IsGenericType && (fieldType.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        public static bool IsRequired(PropertyInfo pi)
        {
            if (null == pi)
            {
                return false;
            }
            Type propertyType = pi.PropertyType;
            if (propertyType.IsGenericType)
            {
                return !(propertyType.GetGenericTypeDefinition() == typeof(Nullable<>));
            }
            return true;
        }

        internal static Assembly LoadAsm(string asmFile)
        {
            return Assembly.Load(File.ReadAllBytes(asmFile));
        }

        public static T ResloveSelfProperty<T>(DataRow dr) where T: new()
        {
            T local = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
            Type type = local.GetType();
            foreach (DataColumn column in dr.Table.Columns)
            {
                PropertyInfo info;
                if (null != (info = type.GetProperty(column.ColumnName)))
                {
                    Type propertyType = info.PropertyType;
                    if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                    }
                    object obj2 = (dr[column.ColumnName] == DBNull.Value) ? null : dr[column.ColumnName];
                    if (obj2 != null)
                    {
                        info.SetValue(local, (typeof(Guid) != propertyType) ? Convert.ChangeType(obj2, propertyType) : Guid.Parse(string.IsNullOrEmpty(obj2.ToString()) ? Guid.Empty.ToString() : obj2.ToString()), null);
                    }
                }
            }
            return local;
        }

        public static byte[] Serialize<T>(T objs)
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, objs);
                return stream.GetBuffer();
            }
        }

        public static string SerializeJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}

