using NHS111.Utils.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Utils.Test.Extensions
{
    [TestFixture]
    public class ListExtension_Test
    {
        class MyClass { }

        [Test]
        public void InList_should_return_list_with_item()
        {
            string myItem = "test";
            List<string> list = myItem.InList<string>();
            Assert.That(list, Is.TypeOf(typeof(List<string>)));
            Assert.That(myItem, Is.TypeOf(typeof(string)));
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.FirstOrDefault(), Is.EqualTo(myItem));
        }

        [Test]
        public void InList_should_return_list_with_item_empty_string()
        {
            string myItem = "";
            List<string> list = myItem.InList<string>();
            Assert.That(list, Is.TypeOf(typeof(List<string>)));
            Assert.That(myItem, Is.TypeOf(typeof(string)));
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.FirstOrDefault(), Is.EqualTo(myItem));
        }

        [Test]
        public void InList_should_return_list_with_item_null_string()
        {
            string myItem = null;
            List<string> list = myItem.InList<string>();
            Assert.That(list, Is.TypeOf(typeof(List<string>)));
            Assert.That(myItem, Is.Null);
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.FirstOrDefault(), Is.EqualTo(myItem));
        }

        [Test]
        public void InList_should_return_list_with_item_empty_MyClass()
        {
            MyClass myItem = new MyClass();
            List<MyClass> list = myItem.InList<MyClass>();
            Assert.That(list, Is.TypeOf(typeof(List<MyClass>)));
            Assert.That(myItem, Is.TypeOf(typeof(MyClass)));
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.FirstOrDefault(), Is.EqualTo(myItem));
        }

        [Test]
        public void InList_should_return_list_with_item_null_MyClass()
        {
            MyClass myItem = null;
            List<MyClass> list = myItem.InList<MyClass>();
            Assert.That(list, Is.TypeOf(typeof(List<MyClass>)));
            Assert.That(myItem, Is.Null);
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.FirstOrDefault(), Is.EqualTo(myItem));
        }
    }
}