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
        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
        }

        private string PrintToString(object obj, int nestingLevel)
        {
            //TODO apply configurations
            if (obj == null)
                return "null" + Environment.NewLine;

            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (finalTypes.Contains(obj.GetType()))
                return obj + Environment.NewLine;

            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties())
            {
                sb.Append(identation + propertyInfo.Name + " = " +
                          PrintToString(propertyInfo.GetValue(obj),
                              nestingLevel + 1));
            }
            return sb.ToString();
        }

        public PrintingConfig<TOwner> ExcludeType<TypeProperty>()
        {
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
            return this;
        }

        public PrintingConfig<TOwner> Clip(Expression<Func<TOwner, string>> stringProperty, int startIndex, int endIndex)

        {
            return this;
        }

        public PrintingConfig<TOwner> ExcludeProperty<Propetry>(Expression<Func<TOwner, Propetry>> expression)
        {
            return this;
        }

    }
}