using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Ultra.Web.Core.Common
{
    public static class DynamicFactory
    {
        private static Func<Type, Type> s_dynamicTypeCreator = new Func<Type, Type>(DynamicFactory.CreateDynamicType);
        private static ConcurrentDictionary<Type, Type> s_dynamicTypes = new ConcurrentDictionary<Type, Type>();

        private static Type CreateDynamicType(Type entityType)
        {
            AssemblyName name = new AssemblyName("DynamicAssembly_" + Guid.NewGuid());
            TypeBuilder builder3 = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run).DefineDynamicModule("DynamicModule_" + Guid.NewGuid()).DefineType(entityType.GetType() + "$DynamicType", TypeAttributes.Public);
            builder3.DefineDefaultConstructor(MethodAttributes.Public);
            foreach (PropertyInfo info in entityType.GetProperties())
            {
                builder3.DefineField(info.Name, info.PropertyType, FieldAttributes.Public);
            }
            return builder3.CreateType();
        }

        public static object ToDynamic(this object entity)
        {
            Type key = entity.GetType();
            Type orAdd = s_dynamicTypes.GetOrAdd(key, s_dynamicTypeCreator);
            object obj2 = Activator.CreateInstance(orAdd);
            foreach (PropertyInfo info in key.GetProperties())
            {
                object obj3 = info.GetValue(entity, null);
                orAdd.GetField(info.Name).SetValue(obj2, obj3);
            }
            return obj2;
        }
    }
}

