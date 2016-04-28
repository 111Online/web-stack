using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Domain;

namespace NHS111.Business.Builders
{
    public class AnswersForNodeBuilder : IAnswersForNodeBuilder
    {
        public string SelectAnswer(IEnumerable<Answer> answers, string value)
        {
            var selected = answers.Select(a => a.Title).OrderBy(a => a == "default").First(option =>
            {
                if (option.PrepareTextForComparison() == "default")
                    return true;
                if (value == null)
                    return false;

                if (option.StartsWith("=="))
                {
                    return (option.NormaliseAnswerText()).Equals(value.PrepareTextForComparison());
                }

                if (option.StartsWith(">="))
                {
                    return Convert.ToInt32(value) >= Convert.ToInt32(option.Substring(2));
                }
                if (option.StartsWith("<="))
                {
                    return Convert.ToInt32(value) <= Convert.ToInt32(option.Substring(2));
                }

                if (option.StartsWith(">"))
                {
                    return Convert.ToInt32(value) > Convert.ToInt32(option.Substring(1));
                }
                if (option.StartsWith("<"))
                {
                    return Convert.ToInt32(value) < Convert.ToInt32(option.Substring(1));
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
        string SelectAnswer(IEnumerable<Answer> answers, string value);
    }
}