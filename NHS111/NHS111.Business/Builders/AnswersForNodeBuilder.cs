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
                if (option == "default")
                    return true;
                if (value == null)
                    return false;

                if (option.StartsWith("=="))
                {
                    return option.Substring(2) == value;
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

    public interface IAnswersForNodeBuilder
    {
        string SelectAnswer(IEnumerable<Answer> answers, string value);
    }
}