using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.Extensions
{
    public static class GeneralExtensions
    {
        public static IList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
        {
            return new ReadOnlyCollection<T>(source.EmptyIfNull().ToArray());
        }

        /// <summary>
        /// Performs delegate action on source object if it is not default.
        /// </summary>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <param name="source">Source object to perform action on.</param>
        /// <param name="callback">Action that will be performed on source object if it is != emptySourceValue.</param>
        /// <param name="getDefault">Default value will be used if source object is == emptySourceValue.</param>
        /// <param name="emptySourceValue">Value that represents empty source.</param>
        /// <returns>Callback result or default.</returns>
        public static TResult IfNotDefault<TResult, TSource>(
            this TSource source,
            Func<TSource, TResult> callback,
            Func<TResult> getDefault = null,
            TSource emptySourceValue = default(TSource))
        {
            return EqualityComparer<TSource>.Default.Equals(source, emptySourceValue) ?
                (getDefault ?? (() => default(TResult)))()
                : callback(source);
        }


        public static TResult IfNotDefault<TResult, TSource>(
            this TSource source,
            TResult result,
            TResult defaultResult = default(TResult),
            TSource emptySourceValue = default(TSource))
        {
            return EqualityComparer<TSource>.Default.Equals(source, emptySourceValue)
                ? defaultResult
                : result;
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
        public static IEnumerable<KeyValuePair<int, T>> WithIndices<T>(this IEnumerable<T> source)
        {
            return source.Select((x, i) => new KeyValuePair<int, T>(i, x));
        }



        public static TOut Tap<TIn, TOut>(this TIn obj, Func<TIn, TOut> callback)
        {
            return callback(obj);
        }

        public static T Tap<T>(this T obj, Action<T> callback)
        {
            callback(obj);
            return obj;
        }

        public static T GetAttributeOfEnumValue<T>(this object enumVal)
            where T : System.Attribute
        {
            T enumAttribute = null;

            try
            {
                var type = enumVal.GetType();
                var memInfo = type.GetMember(enumVal.ToString());
                if (memInfo == null || !memInfo.Any())
                {
                    return null;
                }

                var attributes = memInfo.FirstOrDefault().GetCustomAttributes(typeof(T), false);

                if (attributes != null)
                {
                    enumAttribute = attributes.EmptyIfNull().Any() ? (T)attributes.FirstOrDefault() : null;
                }
            }
            catch (Exception ex)
            {

            }

            return enumAttribute;
        }

        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> source)
        {
            return new LinkedList<T>(source.EmptyIfNull());
        }

        public static string GetPropertyName<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var type = typeof(TSource);

            var memberExpression = propertyLambda.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            var propInfo = memberExpression.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo.Name;
        }

        public static TProperty GetPropertyValue<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            return (TProperty)typeof(TSource)
                .GetProperty(GetPropertyName(source, propertyLambda))
                .GetGetMethod(true)
                .Invoke(source, null);
        }

        public static void SetPropertyValue<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda, TProperty value)
        {
            typeof(TSource)
                .GetProperty(GetPropertyName(source, propertyLambda))
                .GetSetMethod(true)
                .Invoke(source, new object[] { value });
        }
    }
}
