using NHS111.Models.Mappers;
using NUnit.Framework;
using System;

namespace NHS111.Models.Test.Mappers.WebMappings
{
    [TestFixture]
    public class StaticTextToHtmlTests
    {
        private const char BackSlash = (char)47;

        [Test]
        public void ConvertDoubleSlash()
        {
            var result = StaticTextToHtml.Convert("Normal body temperature is 36C - 37C (96.8F - 98.6F).\\nEven if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery.");
            Assert.AreEqual("Normal body temperature is 36C - 37C (96.8F - 98.6F).<br/>Even if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery.", result);
        }

        [Test]
        public void ConvertSingleBreak()
        {
            var result = StaticTextToHtml.Convert("Normal body temperature is 36C - 37C (96.8F - 98.6F).\nEven if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery.");
            Assert.AreEqual("Normal body temperature is 36C - 37C (96.8F - 98.6F).<br/>Even if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery.", result);
        }

        [Test]
        public void ConvertNewLine()
        {
            var result = StaticTextToHtml.Convert("Normal body temperature is 36C - 37C (96.8F - 98.6F)." + Environment.NewLine + "Even if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery.");
            Assert.AreEqual("Normal body temperature is 36C - 37C (96.8F - 98.6F).<br/>Even if you have a high temperature, your hands and feet may still feel cool, or you may feel cold and shivery.", result);
        }
    }
}
