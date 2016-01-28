using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Utils.Configuration;
using NUnit.Framework;
namespace NHS111.Utils.Configuration.Tests
{
    [TestFixture()]
    public class StatementParamatersTests
    {
        [Test()]
        public void GenerateInsertStatementTest()
        {
            StatementParamaters testParamaters = new StatementParamaters();

            testParamaters.Add("testkey1", "test val 1");
            testParamaters.Add("testkey2", 33);
            testParamaters.Add("testkey3", "testval3");

            var result = testParamaters.GenerateInsertStatement("testTable");

            Assert.IsNotNull(result);

            Assert.AreEqual("INSERT INTO testTable (testkey1,testkey2,testkey3) VALUES(@testkey1,@testkey2,@testkey3)", result);
        }
    }
}
