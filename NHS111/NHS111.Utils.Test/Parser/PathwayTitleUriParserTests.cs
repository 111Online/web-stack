using NHS111.Utils.Parser;
using NUnit.Framework;
using System;

namespace NHS111.Utils.Test.Parser
{
    [TestFixture]
    public class PathwayTitleUriParserTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void all_dashes_in_title_replaced_with_spaces()
        {
            const string title = "a-title-with-lots-of-dashes-in-it";
            var result = PathwayTitleUriParser.Parse(title);

            Assert.AreEqual("a title with lots of dashes in it", result);
        }

        [Test]
        public void all_url_encoded_spaces_replaces_with_spaces()
        {
            const string urlEncodedTitle = "a title with lots of spaces";
            var result = PathwayTitleUriParser.Parse(Uri.EscapeUriString(urlEncodedTitle));

            Assert.AreEqual(urlEncodedTitle, result);
        }

        [Test]
        public void escaped_dashes_are_not_replaced_with_a_space()
        {
            const string urlEncodedTitle = "a-title-with-an-es--caped-dash";
            var result = PathwayTitleUriParser.Parse(Uri.EscapeUriString(urlEncodedTitle));

            Assert.AreEqual("a title with an es-caped dash", result);
        }

        [Test]
        public void escaped_symbols_are_replaced_with_double_backslash()
        {
            const string urlEncodedTitle = @"a title with-escaped symbols ?!%^&*()/\\";
            var result = PathwayTitleUriParser.EscapeSymbols(urlEncodedTitle);

            Assert.AreEqual(@"a title with-escaped symbols \\?\\!\\%\\^\\&\\*\\(\\)\\/\\\\\\", result);
        }
    }
}
