using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPrinting
{
    public static class PropertyPrintingConfigExtention
    {
        private static PrintingConfig<TOwner> AddCulture<TOwner>(PrintingConfig<TOwner> printingConfig, CultureInfo culture,
            Type type)
        {
            if (printingConfig.ExcludeTypes.Contains(type))
                throw new InvalidOperationException();
            if (!printingConfig.CultureForDifferentNumberBase.ContainsKey(type))
            {
                printingConfig.CultureForDifferentNumberBase.Add(type, null);
            }
            printingConfig.CultureForDifferentNumberBase[type] = culture;
            return printingConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this PropertyPrintingConfig<int, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return AddCulture(((IPropertyPrintingConfig<int, TOwner>)propertyPrintingConfig).Config, culture, typeof(int));
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this PropertyPrintingConfig<double, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return AddCulture(((IPropertyPrintingConfig<double, TOwner>)propertyPrintingConfig).Config, culture, typeof(double));
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this PropertyPrintingConfig<long, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return AddCulture(((IPropertyPrintingConfig<long, TOwner>)propertyPrintingConfig).Config, culture, typeof(long));
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this PropertyPrintingConfig<float, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return AddCulture(((IPropertyPrintingConfig<float, TOwner>)propertyPrintingConfig).Config, culture, typeof(float));
        }

        public static PrintingConfig<TOwner> Clip<TOwner>(this PropertyPrintingConfig<string, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return ((IPropertyPrintingConfig<string, TOwner>)propertyPrintingConfig).Config;
        }
    }
}
