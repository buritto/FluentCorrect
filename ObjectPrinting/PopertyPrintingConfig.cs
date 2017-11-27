using System;
using System.Collections.Generic;
using System.Globalization;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<T, TOwner> : IPropertyPrintingConfig<T, TOwner>
    {
        private PrintingConfig<TOwner> config;
        private HashSet<Type> excludeTypesInConfig;
        private Dictionary<Type, Func<object, string>> serializationFuncsForDifferentType;
        private Dictionary<Type, CultureInfo> cultureForDifferentNumberBase;

        private PropertyPrintingConfig(PrintingConfig<TOwner> config, HashSet<Type> excludeTypesInConfig, Dictionary<Type, Func<object, string>>
            serializationFuncsForDifferentType, Dictionary<Type, CultureInfo> cultureForDifferentNumberBase )
        {
            this.config = config;
            this.excludeTypesInConfig = excludeTypesInConfig;
            this.serializationFuncsForDifferentType = serializationFuncsForDifferentType;
            this.cultureForDifferentNumberBase = cultureForDifferentNumberBase;
        }

        public PrintingConfig<TOwner> Config => config;

        public PrintingConfig<TOwner> Using(Func<T, string> serializationFun)
        {
            if (excludeTypesInConfig.Contains(typeof(T)))
                throw new InvalidOperationException();
            if (!serializationFuncsForDifferentType.ContainsKey(typeof(T)))
            {
                serializationFuncsForDifferentType.Add(typeof(T), null);
            }
            serializationFuncsForDifferentType[typeof(T)] = x => serializationFun((T) x);
            return config;
        }

  
    }
}
