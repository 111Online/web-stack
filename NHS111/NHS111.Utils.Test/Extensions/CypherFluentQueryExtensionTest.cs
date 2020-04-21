using Neo4jClient;
using Neo4jClient.Cypher;
using NUnit.Framework;
using System;

namespace NHS111.Utils.Test.Extensions
{
    [TestFixture]
    public class CypherFluentQueryExtensionTest
    {
        private readonly ICypherFluentQuery _cypherFluentQuery = new CypherFluentQuery(new GraphClient(new Uri("http://foo/bar/")));

        [Test]
        public void Match_does_not_include_live_only_filter_by_default()
        {
            var query = _cypherFluentQuery
                .Match("(p:Pathway)")
                .Query
                .QueryText;

            Assert.AreEqual("MATCH (p:Pathway)", query);
        }

        [Test]
        public void Match_does_not_include_live_only_filter_when_specified()
        {
            var query = _cypherFluentQuery
                .Match("(p:Pathway)")
                .Query
                .QueryText;

            Assert.AreEqual("MATCH (p:Pathway)", query);
        }

        [Test]
        public void Where_does_not_include_live_only_filter_by_default()
        {
            var query = _cypherFluentQuery
                .Match("(p:Pathway)")
                .Where("p.module = \"1\"")
                .Query
                .QueryText;

            Assert.AreEqual("MATCH (p:Pathway)\r\nWHERE p.module = \"1\"", query);
        }

        [Test]
        public void Where_does_not_include_live_only_filter_when_specified()
        {
            var query = _cypherFluentQuery
                .Match("(p:Pathway)")
                .Where("p.module = \"1\"")
                .Query
                .QueryText;

            Assert.AreEqual("MATCH (p:Pathway)\r\nWHERE p.module = \"1\"", query);
        }
    }
}
