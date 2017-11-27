using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<T, TOwner> : IPropertyPrintingConfig<T, TOwner>
    {

        private PrintingConfig<TOwner> config;

        public PropertyPrintingConfig(PrintingConfig<TOwner> config)
        {
            this.config = config;
        }

        public PrintingConfig<TOwner> Config => config;

        public PrintingConfig<TOwner>Using(Func<T, string> serializationFun)
        {
            if (config.ExcludeTypes.Contains(typeof(T)))
                throw new InvalidOperationException();
            if (!config.SerializationFuncsForDifferentType.ContainsKey(typeof(T)))
            {
               config.SerializationFuncsForDifferentType.Add(typeof(T), null);
            }
            config.SerializationFuncsForDifferentType[typeof(T)] = x => serializationFun((T)x);
            return config;      
        }
    }


}
