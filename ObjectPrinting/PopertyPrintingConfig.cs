using System;
using System.Collections.Generic;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<T, TOwner> : IPropertyPrintingConfig<T, TOwner>
    {
        private readonly PrintingConfig<TOwner> config;
        private readonly HashSet<Type> excludeTypesInConfig;
        private readonly Dictionary<Type, Func<object, string>> serializationFuncsForDifferentType;

        public PropertyPrintingConfig(PrintingConfig<TOwner> config, HashSet<Type> excludeTypesInConfig,
            Dictionary<Type, Func<object, string>>
                serializationFuncsForDifferentType)
        {
            this.config = config;
            this.excludeTypesInConfig = excludeTypesInConfig;
            this.serializationFuncsForDifferentType = serializationFuncsForDifferentType;
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
