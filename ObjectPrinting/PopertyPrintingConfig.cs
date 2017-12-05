using System;
using System.Collections.Generic;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<T, TOwner> : IPropertyPrintingConfig<T, TOwner>
    {
        private readonly HashSet<Type> excludeTypesInConfig;
        private readonly Dictionary<Type, Func<object, string>> serializationFuncsForDifferentType;
        private readonly Action throwException = () => throw new InvalidOperationException("This property or type was excluded");

        public PropertyPrintingConfig(PrintingConfig<TOwner> config, HashSet<Type> excludeTypesInConfig,
            Dictionary<Type, Func<object, string>>
                serializationFuncsForDifferentType)
        {
            Config = config;
            this.excludeTypesInConfig = excludeTypesInConfig;
            this.serializationFuncsForDifferentType = serializationFuncsForDifferentType;
        }

        public PrintingConfig<TOwner> Config { get; }

        public PrintingConfig<TOwner> Using(Func<T, string> serializationFun)
        {
            if (excludeTypesInConfig.Contains(typeof(T)))
                throwException();
            if (!serializationFuncsForDifferentType.ContainsKey(typeof(T)))
            {
                serializationFuncsForDifferentType.Add(typeof(T), null);
            }
            serializationFuncsForDifferentType[typeof(T)] = x => serializationFun((T) x);
            return Config;
        }
    }
}
