using System;
using ObjectPrinting.Tests;

namespace ObjectPrinting
{
    public static class PersonExtention
    {
        public static string GetDefaultSerializ(this Person person)
        {
            return ObjectPrinter.For<Person>().PrintToString(person);
        }

        public static string GetDefaultSerializ(this Person person, Func<PrintingConfig<Person>> config)
        {
            var pritingConfig = config();
            return pritingConfig.PrintToString(person);
        }
    }
}