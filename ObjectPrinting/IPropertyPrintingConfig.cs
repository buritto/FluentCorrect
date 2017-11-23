using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPrinting
{
    public interface IPropertyPrintingConfig<T, TOwner>
    {
        PrintingConfig<TOwner> Config { get; }
    }
}
