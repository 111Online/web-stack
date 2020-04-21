
namespace NHS111.Models.Test.Mappers
{
    using NHS111.Models.Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class StaticTextToHtmlTests
    {
        [Test]
        public void Convert_WithNull_DoesntThrowNullRefException()
        {
            StaticTextToHtml.Convert(null);
        }
    }
}
