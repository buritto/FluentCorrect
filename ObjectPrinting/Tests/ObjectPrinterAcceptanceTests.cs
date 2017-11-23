using NUnit.Framework;
using System.Globalization;
using System.Reflection;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {
        [Test]
        public void Demo()
        {
            var person = new Person { Name = "Alex", Age = 19 };

            var printer = ObjectPrinter.For<Person>()
                //1. Исключить из сериализации свойства определенного типа
                .ExcludeType<int>()
                //2. Указать альтернативный способ сериализации для определенного типа
                .Printing<int>()
                .Using(p => p.ToString())
                //3. Для числовых типов указать культуру
                .Printing<double>()
                .Using(CultureInfo.CurrentCulture)
                //4. Настроить сериализацию конкретного свойства
                .SerializingProperty(p1 => p1.Age, age => age.ToString())
                //5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
                .Clip(p2 => p2.Name, 0, 2)
                //6. Исключить из сериализации конкретного свойства
                .ExcludeProperty(p3 => p3.Height);

            string s1 = printer.PrintToString(person);

            //7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию		
            var defaultSerializ = person.GetDefaultSerializ();
            //8. ...с конфигурированием
            var defaultSerializWithConfig = person.GetDefaultSerializ(config => config.ExcludeProperty(p => p.Age));

        }
    }
}