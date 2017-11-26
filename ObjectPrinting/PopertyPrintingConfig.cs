using System;
using System.Collections.Generic;
using System.Linq;
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

        public PrintingConfig<TOwner>Using(Func<Type, string> serializationFun)
        {
            if (!config.SerializationFuncs.ContainsKey(typeof(T)))
            {
               config.SerializationFuncs.Add(typeof(T), serializationFun);
                return config;
            }
            config.SerializationFuncs[typeof(T)] = serializationFun;
            return config;      
        }

    }


}
