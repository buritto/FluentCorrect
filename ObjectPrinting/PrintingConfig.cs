using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner>
    {
        private readonly HashSet<Type> excludeTypes = new HashSet<Type>();

        private readonly HashSet<string> excludePropert = new HashSet<string>();

        private readonly Dictionary<Type, Func<object, string>> serializationFuncsForDifferentType =
            new Dictionary<Type, Func<object, string>>();

        private readonly Dictionary<Type, CultureInfo> cultureForDifferentNumberBase = new Dictionary<Type, CultureInfo>();

        private readonly Dictionary<string, Func<object, string>> serializationFuncsForDifferentProperty =
            new Dictionary<string, Func<object, string>>();

        private readonly Dictionary<string, Func<string, string>> clipper = new Dictionary<string, Func<string, string>>();
        
        private readonly Action throwException = () => throw new InvalidOperationException("This property or type was excluded");

        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
        }

        private string PrintToString(Object obj, int nestingLevel)
        {
            if (obj == null)
                return "null" + Environment.NewLine;
            if (IsFinalType(obj.GetType()))
                return obj + Environment.NewLine;
            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            foreach (var property in obj.GetType().GetProperties())
            {
                if (CheckExclude(property))
                    continue;
                var serilizationProperty = ApplySerialization(property, obj);
                if (clipper.ContainsKey(property.Name) && serilizationProperty != null)
                {
                    serilizationProperty = clipper[property.Name](serilizationProperty.ToString());
                }
                sb.Append(identation + property.Name + " = " +
                          PrintToString(serilizationProperty,
                              nestingLevel + 1));
            }
            return sb.ToString();
        }

        private bool IsFinalType(Type getType)
        {
            return getType.Assembly.FullName.Contains("mscorlib") && 
             getType.GetMethod("ToString", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance,
             null ,new Type[]{} ,null ) != null;
        }

        private object ApplySerialization(PropertyInfo property, object obj)
        {
            if (serializationFuncsForDifferentProperty.ContainsKey(property.Name))
            {
                return serializationFuncsForDifferentProperty[property.Name](property.GetValue(obj));
            }
            return serializationFuncsForDifferentType.ContainsKey(property.PropertyType) ?
                serializationFuncsForDifferentType[property.PropertyType](property.GetValue(obj)) : property.GetValue(obj);
        }

        private bool CheckExclude(PropertyInfo property)
        {
            return excludePropert.Contains(property.Name) || excludeTypes.Contains(property.PropertyType);
        }

        public PrintingConfig<TOwner> ExcludeType<TypeProperty>()
        {
            if (!excludeTypes.Contains(typeof(TypeProperty)))
                excludeTypes.Add(typeof(TypeProperty));
            return this;
        }

        public PropertyPrintingConfig<T, TOwner> Printing<T>()
        {
            return new PropertyPrintingConfig<T, TOwner>(this, excludeTypes, serializationFuncsForDifferentType);
        }

        public PrintingConfig<TOwner> SerializingProperty<TypeProperty>(
            Expression<Func<TOwner, TypeProperty>> expression,
            Func<TypeProperty, string> serializationMethod)
        {
            var propInfo = GeiInformationProperty(expression);
            CheckCoorectAddSerialization(propInfo);
            if (!serializationFuncsForDifferentProperty.ContainsKey(propInfo.Name))
            {
                serializationFuncsForDifferentProperty.Add(propInfo.Name, x => serializationMethod((TypeProperty)x));
            }
            else
            {
                serializationFuncsForDifferentProperty[propInfo.Name] = x => serializationMethod((TypeProperty)x);
            }
            return this;
        }

        public PrintingConfig<TOwner> Clip(Expression<Func<TOwner, string>> stringProperty, int startIndex,
            int endIndex)
        {
            var propInfo = GeiInformationProperty(stringProperty);
            if (!clipper.ContainsKey(propInfo.Name))
            {
                clipper.Add(propInfo.Name, propertyToString => propertyToString.Substring(startIndex, endIndex));
            }
            else
            {
                clipper[propInfo.Name] =
                    propertyToString => propertyToString.Substring(startIndex, endIndex);
            }
            return this;
        }

        private void CheckCoorectAddSerialization(MemberInfo property)
        {
            if (excludePropert.Contains(property.Name) || excludeTypes.Contains(property.GetType()))
                throwException();
        }

        public PrintingConfig<TOwner> ExcludeProperty<Propetry>(Expression<Func<TOwner, Propetry>> expression)
        {

            var propInfo = GeiInformationProperty(expression);
            if (!excludePropert.Contains(propInfo.Name))
                excludePropert.Add(propInfo.Name);
            return this;
        }

        public PrintingConfig<TOwner> AddCulture(CultureInfo culture, Type type)
        {
            if (excludeTypes.Contains(type))
                throwException();
            if (cultureForDifferentNumberBase.ContainsKey(type))
            {
                cultureForDifferentNumberBase.Add(type, culture);
            }
            else
            {
                cultureForDifferentNumberBase[type] = culture;
            }
            return this;
        }

        private MemberInfo GeiInformationProperty(Expression e)
        {
            var body = ((LambdaExpression) e).Body;
            var member = (MemberExpression) body;
            return member.Member;
        }
    }
}