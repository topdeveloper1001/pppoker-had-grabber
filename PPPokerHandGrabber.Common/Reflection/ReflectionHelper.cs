//-----------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Text;

namespace PPPokerHandGrabber.Common.Reflection
{
    public static class ReflectionHelper
    {
        public static object GetMemberValue(object sourceObject, string memberName)
        {
            string[] propertyChain = memberName.Split('.');
            object currentObject = sourceObject;
            foreach (string propertyName in propertyChain)
            {
                PropertyInfo property;
                currentObject = GetPropertyValue(currentObject, propertyName, out property);
            }
            return currentObject;
        }

        public static object GetPropertyValue(object sourceObject, string propertyName, out PropertyInfo property)
        {
            object value = null;

            property = GetPropertyInfo(sourceObject, propertyName);

            if (property != null && property.CanRead)
            {
                value = property.GetValue(sourceObject, null);
            }

            return value;
        }

        public static PropertyInfo GetPropertyInfo(object sourceObject, string propertyName)
        {
            PropertyInfo prop = null;

            if (sourceObject != null)
            {
                prop = GetPropertyInfo(sourceObject.GetType(), propertyName);
            }

            return prop;
        }

        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo prop = null;

            if (type != null && !String.IsNullOrEmpty(propertyName))
            {
                prop = type.GetProperty(propertyName);
            }

            return prop;
        }

        public static bool SetPropertyValue(object destinationObject, string propertyName, object value)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(destinationObject, propertyName);

            if (propertyInfo == null ||
                !propertyInfo.CanWrite ||
                (value != null && !propertyInfo.PropertyType.IsAssignableFrom(value.GetType())) ||
                (value == null && !IsTypeNullable(propertyInfo.PropertyType)))
            {
                return false;
            }

            propertyInfo.SetValue(destinationObject, value == null ? null : value, null);

            return true;
        }

        public static string GetPropertyNameFromExpression<T>(Expression<Func<T>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            return memberExpression.Member.Name;
        }

        public static IList<KeyValuePair<string, object>> GetObjectsChainFromExpression<T>(object rootObject, Expression<Func<T>> expression)
        {
            List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            var names = GetNamesFromExpression(expression);
            string currentFullName = string.Empty;

            KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(names.First(), rootObject);

            result.Add(keyValuePair);

            foreach (string name in names.Skip(1))
            {
                currentFullName += (currentFullName == string.Empty ? string.Empty : ".") + name;
                keyValuePair = new KeyValuePair<string, object>(name, GetMemberValue(rootObject, currentFullName));
                result.Add(keyValuePair);
            }

            return result;
        }

        public static string GetNamesStringFromExpression<T>(Expression<Func<T>> expression)
        {
            return String.Join(".", GetNamesFromExpression(expression));
        }

        public static IEnumerable<string> GetNamesFromExpression<T>(Expression<Func<T>> expression)
        {
            var lambda = (LambdaExpression)expression;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            List<string> result = new List<string>();
            Expression currentExpression = memberExpression;

            while (currentExpression is MemberExpression)
            {
                string memberName = ((MemberExpression)currentExpression).Member.Name;
                result.Insert(0, memberName);
                currentExpression = ((MemberExpression)currentExpression).Expression;
            }

            return result;
        }

        public static bool IsTypeNullable(Type type)
        {
            return (!type.IsValueType || Nullable.GetUnderlyingType(type) != null);
        }

        public static string GetPath<T>(Expression<Func<T, object>> expression)
        {
            string result = string.Empty;
            Expression expr = expression.Body;

            if (expr is UnaryExpression && expr.NodeType == ExpressionType.Convert && (expr as UnaryExpression).Operand is LambdaExpression)
            {
                expr = ((expr as UnaryExpression).Operand as LambdaExpression).Body;
            }

            while (true)
            {
                if (expr is ParameterExpression)
                {
                    return result;
                }
                if (expr is UnaryExpression)
                {
                    expr = (expr as UnaryExpression).Operand;
                }
                else if (expr is MemberExpression)
                {
                    MemberExpression me = expr as MemberExpression;
                    PropertyInfo pi = me.Member as PropertyInfo;
                    result = pi.Name + (string.IsNullOrEmpty(result) ? string.Empty : ("." + result));
                    expr = me.Expression;
                }
                else if (expr is MethodCallExpression)
                {
                    var methodCallExpession = (MethodCallExpression)expr;
                    result = methodCallExpession.Method.Name + (string.IsNullOrEmpty(result) ? string.Empty : ("." + result));
                    return result;
                }
                else
                {
                    throw new ArgumentException(string.Format("Can not use expression '{0}' as property path. Check all path elements to be properties.", expression), "expression");
                }
            }
        }

        public static string GetPath<T>(this T owner, Expression<Func<T, object>> expression)
        {
            return GetPath(expression);
        }

        public static IEnumerable<string> GetPaths<T>(this T owner, params Expression<Func<T, object>>[] expressions)
        {
            return expressions.Select(GetPath);
        }

        public static Type[] GetThisTypeInterfaces(Type type)
        {
            Type[] allInterfaces = type.GetInterfaces();
            if (allInterfaces.Length == 0 || type.BaseType == null) return allInterfaces;
            return allInterfaces.Where(intf => !intf.IsAssignableFrom(type.BaseType)).ToArray();
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(Type objectType, Type attributeType)
        {
            IEnumerable<Type> objectTypeAndInterfaces = objectType.GetInterfaces().Union(new[] { objectType });
            return objectTypeAndInterfaces
                .SelectMany(typeOrInterface =>
                    typeOrInterface.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => Attribute.GetCustomAttributes(p, attributeType, true).Any()));
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(object obj, Type attributeType)
        {
            return GetPropertiesWithAttribute(obj.GetType(), attributeType);
        }

        public static Dictionary<PropertyInfo, T> GetPropertiesWithAttribute<T>(object obj) where T : Attribute
        {
            if (obj == null) throw new ArgumentException(@"DH: parameter 'obj' can't be null");
            Dictionary<PropertyInfo, T> properties = new Dictionary<PropertyInfo, T>();
            var propertyList = GetPropertiesWithAttribute(obj, typeof(T));
            foreach (var propertyInfo in propertyList)
            {
                T attribute = Attribute.GetCustomAttribute(propertyInfo, typeof(T), false) as T;
                if (attribute != null)
                    properties.Add(propertyInfo, attribute);
            }
            return properties;
        }

        public static Dictionary<string, PropertyInfo> GetPropertiesWithAttribute<T>(object obj, Func<T, string> getKeyFromAttribute) where T : Attribute
        {
            var propertyToAttributeMap = GetPropertiesWithAttribute<T>(obj);
            return propertyToAttributeMap.ToDictionary(pair => getKeyFromAttribute(pair.Value), pair => pair.Key);
        }

        public static bool HasProperty(this object source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName) != null;
        }

        private const string IndexedPropertyName = "Item";
        private const char PropertySeparator = '.';

        public static object GetPropertyValue(object source, string propertyPathName)
        {
            if (propertyPathName == null)
                throw new ArgumentNullException("propertyPathName");

            if (propertyPathName.Equals(Char.ToString(PropertySeparator)))
                return source;

            string[] propertyPathParts = propertyPathName.Split(PropertySeparator);
            List<PropertyData> properties = new List<PropertyData>();
            foreach (string propertyPathPart in propertyPathParts)
            {
                IEnumerable<PropertyData> partProperties = ParseProperty(propertyPathPart);
                properties.AddRange(partProperties);
            }
            return properties.Aggregate(source, GetSimplePropertyValue);
        }

        private static IEnumerable<PropertyData> ParseProperty(string property)
        {
            string[] parts = property.Split('[', ']').Where(s => !String.IsNullOrEmpty(s)).ToArray();

            List<PropertyData> result = new List<PropertyData> { new PropertyData { PropertyName = parts[0] } };
            for (int i = 1; i < parts.Length; i++)
            {
                string[] index = parts[i].Split(',');
                result.Add(new PropertyData { PropertyName = IndexedPropertyName, IndexParameters = index });
            }

            return result;
        }

        private static object GetSimplePropertyValue(object source, PropertyData propertyData)
        {
            if (source == null)
                return null;

            bool isArrayIndexer = source.GetType().IsArray && propertyData.PropertyName == IndexedPropertyName && propertyData.IndexParameters != null;
            Type sourceType = isArrayIndexer ? typeof(IList) : source.GetType();

            object[] index =
                propertyData.IndexParameters != null
                    ? ParseIndexParameters(sourceType, propertyData.PropertyName, propertyData.IndexParameters)
                    : null;

            object value;
            try
            {
                value = sourceType.GetProperty(propertyData.PropertyName).GetValue(source, index);
            }
            catch (Exception)
            {
                value = null;
            }

            return value;
        }

        private static object[] ParseIndexParameters(Type sourceType, string propertyName, string[] stringIndexParameters)
        {
            ParameterInfo[] indexParametersInfo = sourceType.GetProperty(propertyName).GetIndexParameters();
            object[] indexParameters =
                stringIndexParameters.Length > 0
                    ? ConvertToTypedParameters(stringIndexParameters, indexParametersInfo)
                    : null;
            return indexParameters;
        }

        private static object[] ConvertToTypedParameters(string[] stringParameters, ParameterInfo[] parameterInfos)
        {
            if (stringParameters.Length != parameterInfos.Length)
                throw new TargetParameterCountException();

            object[] typedParameters = new object[stringParameters.Length];
            for (int i = 0; i < stringParameters.Length; i++)
            {
                typedParameters[i] = Convert.ChangeType(stringParameters[i], parameterInfos[i].ParameterType);
            }
            return typedParameters;
        }

        public static bool SetEnumerablePropertyValue(object destinationObject, string propertyName, IEnumerable value, bool useSetter = false)
        {
            if (value == null) return false;
            PropertyInfo propertyInfo = destinationObject.GetType().GetProperty(propertyName);
            if (propertyInfo == null) return false;
            Type propertyType = propertyInfo.PropertyType;
            IList propertyValue = (IList)GetPropertyValue(destinationObject, propertyName);
            if (propertyValue != null && !useSetter)
            {
                propertyValue.Clear();
                foreach (var item in value)
                {
                    propertyValue.Add(item);
                }
                return true;
            }
            if (propertyType.IsArray)
            {
                if (value.GetType().IsArray)
                    return SetPropertyValue(destinationObject, propertyName, value);
                var arrayValue = value.Cast<object>().ToArray();
                Array array = Array.CreateInstance(propertyType.GetElementType(), arrayValue.Length);
                for (int i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++)
                    array.SetValue(arrayValue[i], i);
                return SetPropertyValue(destinationObject, propertyName, array);
            }
            if (propertyType.TypeIsInterface(typeof(IEnumerable)))
            {
                Type genericArgumentType = propertyType.IsGenericType ? propertyType.GetGenericArguments().FirstOrDefault() : null;
                if (genericArgumentType != null)
                {
                    IList list = Activator.CreateInstance(typeof(List<>).MakeGenericType(new[] { genericArgumentType })) as IList;
                    if (list != null)
                    {
                        foreach (var item in value)
                        {
                            list.Add(item);
                        }
                        SetPropertyValue(destinationObject, propertyName, list);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool SetDictionaryPropertyValue(object destinationObject, string propertyName, IDictionary value, bool useSetter = false)
        {
            if (value == null) return false;
            PropertyInfo propertyInfo = destinationObject.GetType().GetProperty(propertyName);
            if (propertyInfo == null) return false;
            Type propertyType = propertyInfo.PropertyType;
            IDictionary propertyValue = (IDictionary)GetPropertyValue(destinationObject, propertyName);
            if (propertyValue != null && !useSetter)
            {
                propertyValue.Clear();
                foreach (var key in value.Keys)
                {
                    propertyValue.Add(key, value[key]);
                }
                return true;
            }
            if (propertyType.IsGenericType && IsDictionary(propertyType))
            {
                Type[] genericTypes = propertyType.GetGenericArguments();
                if (genericTypes.Length == 2)
                {
                    IDictionary dictionary = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(genericTypes)) as IDictionary;
                    if (dictionary != null)
                    {
                        foreach (var key in value.Keys)
                        {
                            dictionary.Add(key, value[key]);
                        }
                        SetPropertyValue(destinationObject, propertyName, dictionary);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsDictionary(Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type)
                || (type.IsGenericType &&
                       (typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition())
                      || typeof(Dictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition())));
        }

        public static Func<object, object> CreatePropertyValueGetter(Type type, string propertyName)
        {
            var param = Expression.Parameter(typeof(object), "e");
            Expression body = Expression.PropertyOrField(Expression.TypeAs(param, type), propertyName);
            var getterExpression = Expression.Lambda<Func<object, object>>(body, param);
            return getterExpression.Compile();
        }

        public static string GetReadableName(this Type type)
        {
            if (!type.IsGenericType)
                return type.Name;
            StringBuilder sb = new StringBuilder();

            sb.Append(type.Name.Substring(0, type.Name.LastIndexOf("`", StringComparison.Ordinal)));
            sb.Append(type.GetGenericArguments().Aggregate("<",
                (aggregate, t) => aggregate + (aggregate == "<" ? "" : ",") + GetReadableName(t)
                ));
            sb.Append(">");

            return sb.ToString();
        }

    }
}