using Lookif.Library.Common.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Lookif.Library.Common.Utilities
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets a all Type instances matching the specified class name with just non-namespace qualified class name.
        /// </summary>
        /// <param name="className">Name of the class sought.</param>
        /// <returns>Types that have the class name specified. They may not be in the same namespace.</returns>
        public static Type[] getTypeByName(string className, string FromSpecificAssembly = "")
        {
            List<Type> returnVal = new();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains(FromSpecificAssembly));
            foreach (Assembly a in assemblies)
            {
                Type[] assemblyTypes = a.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++)
                {
                    if (assemblyTypes[j].Name == className)
                    {
                        returnVal.Add(assemblyTypes[j]);
                    }
                }
            }

            return returnVal.ToArray();
        }


        public static Type getTypeByName(string className, Assembly FromSpecificAssembly)
        {

            Type[] assemblyTypes = FromSpecificAssembly.GetTypes();
            for (int j = 0; j < assemblyTypes.Length; j++)
            {
                if (assemblyTypes[j].Name == className)
                {
                   return  assemblyTypes[j];
                }
            }


            return default;
        }


        public static bool HasAttribute<T>(this MemberInfo type, bool inherit = false) where T : Attribute
        {
            return HasAttribute(type, typeof(T), inherit);
        }

        public static bool HasAttribute(this MemberInfo type, Type attribute, bool inherit = false)
        {
            return Attribute.IsDefined(type, attribute, inherit);
            //return type.IsDefined(attribute, inherit);
            //return type.GetCustomAttributes(attribute, inherit).Length > 0;
        }

        public static bool IsInheritFrom<T>(this Type type)
        {
            return IsInheritFrom(type, typeof(T));
        }

        public static bool IsInheritFrom(this Type type, Type parentType)
        {
            //the 'is' keyword do this too for values (new ChildClass() is ParentClass)
            return parentType.IsAssignableFrom(type);
        }

        public static bool BaseTypeIsGeneric(this Type type, Type genericType)
        {
            return type.BaseType?.IsGenericType == true && type.BaseType.GetGenericTypeDefinition() == genericType;
        }

        public static IEnumerable<Type> GetTypesAssignableFrom<T>(params Assembly[] assemblies)
        {
            return typeof(T).GetTypesAssignableFrom(assemblies);
        }

        public static IEnumerable<Type> GetTypesAssignableFrom(this Type type, params Assembly[] assemblies)
        {
            return assemblies.SelectMany(p => p.GetTypes()).Where(p => p.IsInheritFrom(type));
        }

        public static IEnumerable<Type> GetTypesHasAttribute<T>(params Assembly[] assemblies) where T : Attribute
        {
            return typeof(T).GetTypesHasAttribute(assemblies);
        }

        public static IEnumerable<Type> GetTypesHasAttribute(this Type type, params Assembly[] assemblies)
        {
            return assemblies.SelectMany(p => p.GetTypes()).Where(p => p.HasAttribute(type));
        }

        public static bool IsEnumerable(this Type type)
        {
            return type != typeof(string) && type.IsInheritFrom<IEnumerable>();
        }

        public static bool IsEnumerable<T>(this Type type)
        {
            return type != typeof(string) && type.IsInheritFrom<IEnumerable<T>>() && type.IsGenericType;
        }

        public static IEnumerable<Type> GetBaseTypesAndInterfaces(this Type type)
        {
            if ((type == null) || (type.BaseType == null))
                yield break;

            foreach (var i in type.GetInterfaces())
                yield return i;

            var currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }

        public static bool IsCustomType(this Type type)
        {
            //return type.Assembly.GetName().Name != "mscorlib";
            return type.IsCustomValueType() || type.IsCustomReferenceType();
        }

        public static bool IsCustomValueType(this Type type)
        {
            return type.IsValueType && !type.IsPrimitive && type.Namespace != null && !type.Namespace.StartsWith("System", StringComparison.Ordinal);
        }

        public static bool IsCustomReferenceType(this Type type)
        {
            return !type.IsValueType && !type.IsPrimitive && type.Namespace != null && !type.Namespace.StartsWith("System", StringComparison.Ordinal);
        }


        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src);
        }
        public static void SetPropValue(this object src, string propName, string value)
        {
            src.GetType().GetProperty(propName)?.SetValue(src, value.Trim());
        }

        //This is used when you want to get other entity or classes not basic types like int,string
        public static IEnumerable<PropertyInfo> GetRelatedEntities(string entityName, Type IsAssignableTo)
        {
            var type = getTypeByName(entityName).FirstOrDefault(x => x.Assembly.FullName.Contains("Core"));
            if (type is null || type == default)
                throw new LogicException($"جدول {entityName} موجود نیست");

            var RelatedClasses = type
                 .GetProperties()
                 .Where(
                            item =>
                                item.PropertyType.IsAssignableTo(IsAssignableTo) &&
                                item.PropertyType.IsAbstract == false &&
                                item.PropertyType.IsInterface == false
                        );

            return RelatedClasses;
        }

        //This is used when you want to get all tables 
        public static IEnumerable<Type> GetAllEntities(Type IsAssignableTo)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                   .SelectMany(s => s.GetTypes())
                   .Where(item => IsAssignableTo.IsAssignableFrom(item) &&
                                item.IsAbstract == false &&
                                item.IsInterface == false);

        }

        /// <summary>
        /// Get Property via type name
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetProperties(this string src, bool all = false, string FromSpecificAssembly = "")
        {
            var type = getTypeByName(src, FromSpecificAssembly).FirstOrDefault();
            if (type is null)
                throw new AppException($"There is no type named {src}");


            return (all) ?
                type.GetProperties().ToList() :
                type.GetProperties(System.Reflection.BindingFlags.Public
                                 | System.Reflection.BindingFlags.Instance
                                 | System.Reflection.BindingFlags.DeclaredOnly)
                                  .ToList();

            ;
        }




        public static PropertyInfo[] GetProperties(this object src)
        {
            return src.GetType().GetProperties();
        }
        public static T? GetAttribute<T>(this Type src) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(src, typeof(T));
        }





        public static T GetAttribute<T>(this MemberInfo member, bool isRequired)
            where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }

        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {

            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }



    }
}
