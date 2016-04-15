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
            string html = toConvert.Replace(Environment.NewLine, "<br/>");
            html = html.Replace("\n", "<br/>");

            return html;
        }
    }
}