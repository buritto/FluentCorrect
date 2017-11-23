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
        public static PrintingConfig<TOwner> Using<TOwner>(this PropertyPrintingConfig<int, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return ((IPropertyPrintingConfig<int, TOwner>)propertyPrintingConfig).Config;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this PropertyPrintingConfig<double, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return ((IPropertyPrintingConfig<double, TOwner>)propertyPrintingConfig).Config;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this PropertyPrintingConfig<long, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return ((IPropertyPrintingConfig<long, TOwner>)propertyPrintingConfig).Config;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(this PropertyPrintingConfig<float, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return ((IPropertyPrintingConfig<float, TOwner>)propertyPrintingConfig).Config;
        }

        public static PrintingConfig<TOwner> Clip<TOwner>(this PropertyPrintingConfig<string, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return ((IPropertyPrintingConfig<string, TOwner>)propertyPrintingConfig).Config;
        }
    }
}
