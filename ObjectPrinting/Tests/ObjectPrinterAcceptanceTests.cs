using System;
using NUnit.Framework;
using System.Globalization;
using System.Reflection;
using FluentAssertions;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {
        private Person person = new Person
        {
            Name = "Alex", Age = 19, Height = 176.22,
            Parent = new Person()
            {
                Age = 22, Height = 170,  
            }
        };

        [Test]
        public void ExcludeType()
        {
            var printer = ObjectPrinter.For<Person>().ExcludeType<int>();
            Assert.AreEqual("1",printer.PrintToString(person));
        }

        [Test]
        public void Demo()
        {


            var printer = ObjectPrinter.For<Person>()
                //1. Исключить из сериализации свойства определенного типа
                .ExcludeType<int>()
                //2. Указать альтернативный способ сериализации для определенного типа
                .Printing<double>()
                .Using(p => p.ToString())
                //3. Для числовых типов указать культуру
                .Printing<double>()
                .Using(CultureInfo.CurrentCulture)
                //4. Настроить сериализацию конкретного свойства
                .SerializingProperty(p => p.Name, name => "mr/ms" + name)
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