using NUnit.Framework;
using System.ComponentModel;

namespace NHS111.Functional.Tests.Tools
{
    public class SchemaValidation
    {
        public enum ResponseSchemaType
        {
            Pathway,
            Question,
            Answer,
            Answers,
            CheckServiceDetailsById
        }

        public static void AssertValidResponseSchema(string result, ResponseSchemaType schemaType)
        {
            switch (schemaType)
            {
                case ResponseSchemaType.Pathway:
                    AssertValidPathwayResponseSchema(result);
                    break;
                case ResponseSchemaType.Answers:
                    AssertValidAnswersResponseSchema(result);
                    break;
                case ResponseSchemaType.Question:
                    AssertValidQuestionResponseSchema(result);
                    break;
                case ResponseSchemaType.Answer:
                    AssertValidAnswerResponseSchema(result);
                    break;
                case ResponseSchemaType.CheckServiceDetailsById:
                    AssertValidCheckServiceDetailsByIdResponseSchema(result);
                    break;
                default:
                    throw new InvalidEnumArgumentException(string.Format("{0}{1}{2}", "ResponseSchemaType of ", schemaType.ToString(), "is unsupported"));
            }
        }

        private static void AssertValidAnswerResponseSchema(string result)
        {
            Assert.IsFalse(result.Contains("\"Question"));
            Assert.IsFalse(result.Contains("\"group"));
            Assert.IsFalse(result.Contains("\"topic"));
            Assert.IsFalse(result.Contains("\"questionNo"));
            Assert.IsFalse(result.Contains("\"jtbs"));
            Assert.IsFalse(result.Contains("\"jtbsText"));
            Assert.IsFalse(result.Contains("\"Answers"));
            Assert.IsFalse(result.Contains("\"Labels"));
        }

        private static void AssertValidAnswersResponseSchema(string result)
        {
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Contains("\"order"));
        }
        private static void AssertValidPathwayResponseSchema(string result)
        {
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"pathwayNo"));
            Assert.IsTrue(result.Contains("\"gender"));
            Assert.IsTrue(result.Contains("\"minimumAgeInclusive"));
            Assert.IsTrue(result.Contains("\"maximumAgeExclusive"));
            Assert.IsTrue(result.Contains("\"module"));
            Assert.IsTrue(result.Contains("\"symptomGroup"));
            Assert.IsTrue(result.Contains("\"group"));
        }

        private static void AssertValidQuestionResponseSchema(string result)
        {
            Assert.IsTrue(result.Contains("\"Question"));
            Assert.IsTrue(result.Contains("\"order"));
            Assert.IsTrue(result.Contains("\"topic"));
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"questionNo"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"jtbs"));
            Assert.IsTrue(result.Contains("\"jtbsText"));
            Assert.IsTrue(result.Contains("\"Answers"));
            Assert.IsTrue(result.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Contains("\"Labels"));
            Assert.IsTrue(result.Contains("\"State"));
        }

        private static void AssertValidCheckServiceDetailsByIdResponseSchema(string result)
        {
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"odsCode"));
            Assert.IsTrue(result.Contains("\"contactDetails"));
        }

    }
}
