using NHS111.Models.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Business.Builders
{
    public class AnswersForNodeBuilder : IAnswersForNodeBuilder
    {
        public Answer SelectAnswer(IEnumerable<Answer> answers, string value)
        {
            var selected = answers.OrderBy(a => a.Order).FirstOrDefault(option =>
            {
                if (option.Title.PrepareTextForComparison() == "default")
                    return true;
                if (value == null)
                    return false;

                if (option.Title.StartsWith("=="))
                {
                    return (option.Title.NormaliseAnswerText()).Equals(value.PrepareTextForComparison());
                }

                if (option.Title.StartsWith(">="))
                {
                    return Convert.ToInt32(value) >= Convert.ToInt32(option.Title.Substring(2));
                }
                if (option.Title.StartsWith("<="))
                {
                    return Convert.ToInt32(value) <= Convert.ToInt32(option.Title.Substring(2));
                }

                if (option.Title.StartsWith(">"))
                {
                    return Convert.ToInt32(value) > Convert.ToInt32(option.Title.Substring(1));
                }
                if (option.Title.StartsWith("<"))
                {
                    return Convert.ToInt32(value) < Convert.ToInt32(option.Title.Substring(1));
                }

                throw new Exception(string.Format("No logic implemented for option '{0}'", option));
            });

            return selected;
        }
    }

    internal static class AnswerStringExtensions
    {
        internal static string NormaliseAnswerText(this string answerText)
        {
            return answerText.RemoveEscapedQuotes().RemoveNumericalOperators().PrepareTextForComparison();
        }

        internal static string PrepareTextForComparison(this string input)
        {
            return input.RemoveEscapedQuotes().ToLower();
        }

        private static string RemoveEscapedQuotes(this string input)
        {
            return input.Replace("\\", "").Replace("\"", "");
        }

        private static string RemoveNumericalOperators(this string input)
        {
            return input.Substring(2);
        }
    }

    public interface IAnswersForNodeBuilder
    {
        Answer SelectAnswer(IEnumerable<Answer> answers, string value);
    }
}