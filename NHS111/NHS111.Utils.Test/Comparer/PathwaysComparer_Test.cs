//TODO restore test

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NUnit.Framework;
//using Moq;
//using NHS111.Models.Models.Domain;
//using NHS111.Utils.Comparer;

//namespace NHS111.Utils.Test.Comparer
//{
//    [TestFixture]
//    public class PathwaysComparer_Test
//    {
//        PathwaysComparer PathwaysComparer;

//        [SetUp]
//        public void SetUp()
//        {
//            PathwaysComparer = new PathwaysComparer();
//        }


//        [Test]
//        public void should_do_comparison()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway() 
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            }; 

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(true));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway1.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway2.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.EqualTo(UniquePathway2.Title));

//        }

//        [Test]
//        public void should_do_comparison_empty_PathwayNo()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = "",
//                Title = "Title"
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "",
//                Title = "Title"
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(true));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo(""));
//            Assert.That(UniquePathway1.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo(""));
//            Assert.That(UniquePathway2.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.EqualTo(UniquePathway2.Title));

//        }

//        [Test]
//        public void should_do_comparison_empty_Title()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = ""
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = ""
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(true));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway1.Title, Is.EqualTo(""));
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway2.Title, Is.EqualTo(""));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.EqualTo(UniquePathway2.Title));

//        }

//        [Test]
//        public void should_do_comparison_UniquePathway1_PathwayNo_different()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNoDifferent",
//                Title = "Title"
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(false));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo("PathwayNoDifferent"));
//            Assert.That(UniquePathway1.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway2.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway1.PathwayNo, Is.Not.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.EqualTo(UniquePathway2.Title));

//        }

//        [Test]
//        public void should_do_comparison_UniquePathway1_PathwayNo_null()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = null,
//                Title = "Title"
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(false));
//            Assert.That(UniquePathway1.PathwayNo, Is.Null);
//            Assert.That(UniquePathway1.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway2.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway1.PathwayNo, Is.Not.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.EqualTo(UniquePathway2.Title));

//        }

//        [Test]
//        public void should_do_comparison_UniquePathway1_Title_different()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "TitleDifferent"
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(false));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway1.Title, Is.EqualTo("TitleDifferent"));
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway2.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.Not.EqualTo(UniquePathway2.Title));

//        }

//        [Test]
//        public void should_do_comparison_UniquePathway1_Title_null()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = null
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(false));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway1.Title, Is.Null);
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway2.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.Not.EqualTo(UniquePathway2.Title));

//        }
//        [Test]
//        public void should_do_comparison_UniquePathway2_PathwayNo_different()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNoDifferent",
//                Title = "Title"
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(false));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway1.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo("PathwayNoDifferent"));
//            Assert.That(UniquePathway2.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway1.PathwayNo, Is.Not.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.EqualTo(UniquePathway2.Title));

//        }

//        [Test]
//        public void should_do_comparison_UniquePathway2_PathwayNo_null()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = null,
//                Title = "Title"
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(false));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway1.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway2.PathwayNo, Is.Null);
//            Assert.That(UniquePathway2.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway1.PathwayNo, Is.Not.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.EqualTo(UniquePathway2.Title));

//        }

//        [Test]
//        public void should_do_comparison_UniquePathway2_Title_different()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "TitleDifferent"
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(false));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway1.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway2.Title, Is.EqualTo("TitleDifferent"));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.Not.EqualTo(UniquePathway2.Title));

//        }


//        [Test]
//        public void should_do_comparison_UniquePathway2_Title_null()
//        {

//            //Arrange
//            UniquePathway UniquePathway1 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            UniquePathway UniquePathway2 = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = null
//            };

//            //Act
//            var result = PathwaysComparer.Equals(UniquePathway1, UniquePathway2);

//            //Assert
//            Assert.That(result, Is.EqualTo(false));
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway1.Title, Is.EqualTo("Title"));
//            Assert.That(UniquePathway2.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway2.Title, Is.Null);
//            Assert.That(UniquePathway1.PathwayNo, Is.EqualTo(UniquePathway2.PathwayNo));
//            Assert.That(UniquePathway1.Title, Is.Not.EqualTo(UniquePathway2.Title));

//        }


//        [Test]
//        public void should_return_hashCode()
//        {
//            //Arrange
//            UniquePathway UniquePathway = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = "Title"
//            };

//            //Act
//            var result = PathwaysComparer.GetHashCode(UniquePathway);

//            //Assert
//            Assert.That(UniquePathway.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway.Title, Is.EqualTo("Title"));
//            Assert.That(result, Is.EqualTo(UniquePathway.PathwayNo.GetHashCode() ^ UniquePathway.Title.GetHashCode()));


//        }

//        [Test]
//        public void should_return_hashCode_PathwayNo_empty()
//        {
//            //Arrange
//            UniquePathway UniquePathway = new UniquePathway()
//            {
//                PathwayNo = "",
//                Title = "Title"
//            };

//            //Act
//            var result = PathwaysComparer.GetHashCode(UniquePathway);

//            //Assert
//            Assert.That(UniquePathway.PathwayNo, Is.EqualTo(""));
//            Assert.That(UniquePathway.Title, Is.EqualTo("Title"));
//            Assert.That(result, Is.EqualTo(UniquePathway.PathwayNo.GetHashCode() ^ UniquePathway.Title.GetHashCode()));

//        }

//        [Test]
//        public void should_return_hashCode_Title_empty()
//        {
//            //Arrange
//            UniquePathway UniquePathway = new UniquePathway()
//            {
//                PathwayNo = "PathwayNo",
//                Title = ""
//            };

//            //Act
//            var result = PathwaysComparer.GetHashCode(UniquePathway);

//            //Assert
//            Assert.That(UniquePathway.PathwayNo, Is.EqualTo("PathwayNo"));
//            Assert.That(UniquePathway.Title, Is.EqualTo(""));
//            Assert.That(result, Is.EqualTo(UniquePathway.PathwayNo.GetHashCode() ^ UniquePathway.Title.GetHashCode()));

//        }

//        [Test]
//        public void should_return_hashCode_both_empty()
//        {
//            //Arrange
//            UniquePathway UniquePathway = new UniquePathway()
//            {
//                PathwayNo = "",
//                Title = ""
//            };

//            //Act
//            var result = PathwaysComparer.GetHashCode(UniquePathway);

//            //Assert
//            Assert.That(UniquePathway.PathwayNo, Is.EqualTo(""));
//            Assert.That(UniquePathway.Title, Is.EqualTo(""));
//            Assert.That(result, Is.EqualTo(UniquePathway.PathwayNo.GetHashCode() ^ UniquePathway.Title.GetHashCode()));

//        }

//    }
//}
