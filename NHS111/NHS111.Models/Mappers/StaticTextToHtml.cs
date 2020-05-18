using System;

namespace NHS111.Models.Mappers
{
    public class StaticTextToHtml
    {
        /// <summary>
        /// Convert new lines to html tags
        /// </summary>       
        public static string Convert(string toConvert)
        {
            if (toConvert == null)
                return null;

            string html = toConvert.Replace(Environment.NewLine, "<br/>");

            //Hedge Endie fix for escaped \n from json.
            //json adds the extra \ so itself doesn't break onto a new line.
            html = html.Replace("\\n", "<br/>");
            html = html.Replace("\n", "<br/>");

            return html;
        }
    }
}