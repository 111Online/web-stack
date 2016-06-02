using NHS111.Utils.Configuration;
using NUnit.Framework;

namespace NHS111.Utils.Test.Configuration
{
    [TestFixture()]
    public class StatementParametersTests
    {
        [Test()]
        public void GenerateInsertStatementTest()
        {
            StatementParameters testParamaters = new StatementParameters();

            testParamaters.Add("testkey1", "test val 1");
            testParamaters.Add("testkey2", 33);
            testParamaters.Add("testkey3", "testval3");

            var result = testParamaters.GenerateInsertStatement("testTable");

            Assert.IsNotNull(result);

            Assert.AreEqual("INSERT INTO testTable (testkey1,testkey2,testkey3) VALUES(@testkey1,@testkey2,@testkey3)", result);
        }
    }
}
