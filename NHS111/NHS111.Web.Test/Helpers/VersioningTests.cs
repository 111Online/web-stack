using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NHS111.Web.Helpers;
using NUnit.Framework;
namespace NHS111.Web.Helpers.Tests
{
    [TestFixture()]
    public class VersioningTests
    {
        [Test()]
        public void GetVersionedUriRefTest()
        {
            string EXPECTED_HASH = "640ab2bae07bedc4c163f679a746f7ab7fb5d1fa";
            string keyToPass = "~/content/css_NhsUk/question.css";

            var mockFileIO = new Mock<IFileIO>();
            mockFileIO.Setup(i => i.OpenRead(It.IsAny<string>())).Returns(() =>ToStream("Test"));

            var mockPathProvider = new Mock<IPathProvider>();
            mockPathProvider.Setup(i => i.ToAbsolute(It.IsAny<string>())).Returns((string passdString) => passdString);

            Versioning._fileIO = mockFileIO.Object;
            Versioning._pathProvider = mockPathProvider.Object;

            Parallel.For(0, 2, i =>
                Assert.AreEqual(keyToPass + "?" + EXPECTED_HASH, Versioning.GetVersionedUriRef("~/content/css_NhsUk/question.css"))
                );
        }

        public Stream ToStream(string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

}
