using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner>
    {
        private List<Type> excludeTypes = new List<Type>();

        private List<string> excludePropert = new List<string>();

        private Dictionary<Type, Func<object, string>> serializationFuncsForDifferentType =
            new Dictionary<Type, Func<object, string>>();

        private Dictionary<Type, CultureInfo> cultureForDifferentNumberBase = new Dictionary<Type, CultureInfo>();

        private Dictionary<string, Func<object, string>> serializationFuncsForDifferentProperty =
            new Dictionary<string, Func<object, string>>();

        private Dictionary<string, Func<string, string>> Clipper = new Dictionary<string, Func<string, string>>();

        private Type[] finalTypes = new[]
        {
            typeof(int), typeof(double), typeof(float), typeof(string),
            typeof(DateTime), typeof(TimeSpan)
        };

        public List<Type> ExcludeTypes { get => excludeTypes; set => excludeTypes = value; }
        public List<string> ExcludePropert { get => excludePropert; set => excludePropert = value; }
        public Dictionary<Type, Func<object, string>> SerializationFuncsForDifferentType { get => serializationFuncsForDifferentType; set => serializationFuncsForDifferentType = value; }
        public Dictionary<Type, CultureInfo> CultureForDifferentNumberBase { get => cultureForDifferentNumberBase; set => cultureForDifferentNumberBase = value; }
        public Dictionary<string, Func<object, string>> SerializationFuncsForDifferentProperty { get => serializationFuncsForDifferentProperty; set => serializationFuncsForDifferentProperty = value; }
        public Dictionary<string, Func<string, string>> Clipper1 { get => Clipper; set => Clipper = value; }
        public Type[] FinalTypes { get => finalTypes; set => finalTypes = value; }

        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
        }


        public string PrintToString(Object obj, int nestingLevel)
        {
            if (obj == null)
                return "null" + Environment.NewLine;
            if (FinalTypes.Contains(obj.GetType()))
                return obj + Environment.NewLine;
            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            foreach (var property in obj.GetType().GetProperties())
            {
                if (CheckExclude(property))
                    continue;
                var serilizationProperty = ApplySerialization(property, obj);
                if (Clipper1.ContainsKey(property.Name))
                {
                    serilizationProperty = Clipper1[property.Name](serilizationProperty.ToString());
                }
                sb.Append(identation + property.Name + " = " +
                          PrintToString(serilizationProperty,
                              nestingLevel + 1));
            }
            return sb.ToString();
        }

        private object ApplySerialization(PropertyInfo property, object obj)
        {
            if (SerializationFuncsForDifferentProperty.ContainsKey(property.Name))
            {
                return SerializationFuncsForDifferentProperty[property.Name](property.GetValue(obj));
            }
            if (SerializationFuncsForDifferentType.ContainsKey(property.PropertyType))
            {
                return SerializationFuncsForDifferentType[property.PropertyType](property.GetValue(obj));
            }
            return property.GetValue(obj);
            ;
        }

        private bool CheckExclude(PropertyInfo property)
        {
            return ExcludePropert.Contains(property.Name) || ExcludeTypes.Contains(property.PropertyType);
        }

        public PrintingConfig<TOwner> ExcludeType<TypeProperty>()
        {
            if (!ExcludeTypes.Contains(typeof(TypeProperty)))
                ExcludeTypes.Add(typeof(TypeProperty));
            return this;
        }

        public PropertyPrintingConfig<T, TOwner> Printing<T>()
        {
            return new PropertyPrintingConfig<T, TOwner>(this);
        }

        public PrintingConfig<TOwner> SerializingProperty<TypeProperty>(
            Expression<Func<TOwner, TypeProperty>> expression,
            Func<TypeProperty, string> serializationMethod)
        {
            var propInfo =
                ((MemberExpression) expression.Body)
                .Member as PropertyInfo;
            CheckCoorectAddSerialization(propInfo);
            if (!SerializationFuncsForDifferentProperty.ContainsKey(propInfo.Name))
            {
                SerializationFuncsForDifferentProperty.Add(propInfo.Name, null);
            }
            SerializationFuncsForDifferentProperty[propInfo.Name] = x => serializationMethod((TypeProperty) x);
            return this;
        }

        public PrintingConfig<TOwner> Clip(Expression<Func<TOwner, string>> stringProperty, int startIndex,
            int endIndex)
        {
            var propInfo =
                ((MemberExpression) stringProperty.Body)
                .Member as PropertyInfo;
            if (!Clipper1.ContainsKey(propInfo.Name))
                Clipper1.Add(propInfo.Name, null);
            Clipper1[propInfo.Name] =
                propertyToString => propertyToString.Substring(startIndex, endIndex);
            return this;
        }

        private void CheckCoorectAddSerialization(PropertyInfo property)
        {
            if (ExcludePropert.Contains(property.Name) || ExcludeTypes.Contains(property.PropertyType))
                throw new InvalidOperationException();
        }

        public PrintingConfig<TOwner> ExcludeProperty<Propetry>(Expression<Func<TOwner, Propetry>> expression)
        {
            var propInfo =
                ((MemberExpression) expression.Body)
                .Member as PropertyInfo;
            if (!ExcludePropert.Contains(propInfo.Name))
                ExcludePropert.Add(propInfo.Name);
            return this;
        }
    }
}