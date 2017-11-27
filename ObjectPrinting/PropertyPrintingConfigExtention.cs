using System.Globalization;

namespace ObjectPrinting
{
    public static class PropertyPrintingConfigExtention
    {

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<int, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return propertyPrintingConfig.Config.AddCulture(culture,
                typeof(int));
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<double, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return propertyPrintingConfig.Config.AddCulture(culture,
                typeof(double));
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<long, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return propertyPrintingConfig.Config.AddCulture(culture,
                typeof(long));
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<float, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return propertyPrintingConfig.Config.AddCulture(culture,
                typeof(float));
        }

        public static PrintingConfig<TOwner> Clip<TOwner>(
            this PropertyPrintingConfig<string, TOwner> propertyPrintingConfig, CultureInfo culture)
        {
            return ((IPropertyPrintingConfig<string, TOwner>) propertyPrintingConfig).Config;
        }
    }
}