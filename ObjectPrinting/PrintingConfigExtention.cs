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
        public static string GetDefaultSerializ(this Person person)
        {
            return person.ToString();
        }

        public static string GetDefaultSerializ(this Person person, Func<PrintingConfig<Person>, PrintingConfig<Person>> config)
        {
            return person.ToString();
        }
    }
}
