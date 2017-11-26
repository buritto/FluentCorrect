using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectPrinting.Tests;

namespace ObjectPrinting
{
    public static class PrintingConfigExtention
    {
        private static Type[] finalTypes = new[]
        {
            typeof(int), typeof(double), typeof(float), typeof(string),
            typeof(DateTime), typeof(TimeSpan)
        };
        public static string GetDefaultSerializ(this Person person)
        {
            return DefaultPrint(person, 0);
        }

        private static string DefaultPrint(object obj, int nestingLevel)
        {
            if (obj == null)
                return "null" + Environment.NewLine;
            if (finalTypes.Contains(obj.GetType()))
                return obj + Environment.NewLine;

            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties())
            {
                sb.Append(identation + propertyInfo.Name + " = " +
                          DefaultPrint(propertyInfo.GetValue(obj),
                              nestingLevel + 1));
            }
            return sb.ToString();
        }

        public static string GetDefaultSerializ(this Person person, Func<PrintingConfig<Person>> config)
        {
            var pritingConfig = config();
            return pritingConfig.PrintToString(person);
        }
        
    }
}
