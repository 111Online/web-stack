using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NHS111.Utils.Parser
{
    public class PathwayTitleUriParser
    {
        //public static char DashChar { get; } = Convert.ToChar("-");

        private const char DashChar = '-';
        private static readonly Regex Pattern = new Regex(@"[!%^&*()/\\?]");


        public static string Parse(string title)
        {
            var urlDecodedTitle = HttpUtility.UrlDecode(title);
            return urlDecodedTitle != null ? ReplaceDashes(urlDecodedTitle) : string.Empty;

            //return urlDecodedTitle?replaceDashes(urlDecodedTitle) ?? string.Empty;
        }

        public static string EscapeSymbols(string title)
        {
            return Pattern.Replace(title, @"\\${0}");
        }

        private static string ReplaceDashes(string title)
        {
            var parsedTitle = new StringBuilder();
            var titleArrray = title.ToCharArray();

            // Loop through array.
            for (var i = 0; i < titleArrray.Length; i++)
            {
                // Get character from array.
                var character = titleArrray[i];
                // Don't replace with space if the next char is a dash
                if (!IsLastChar(i, titleArrray.Length) && IsEscapedDash(character, titleArrray[i + 1]))
                {
                    parsedTitle.Append(DashChar);
                    i++;
                }
                else
                {
                    parsedTitle.Append(ReplaceDash(character));
                }
            }

            return parsedTitle.ToString();
        }

        private static string ReplaceDash(char character)
        {
            return character.ToString().Replace(DashChar.ToString(), " ");
        }

        private static bool IsEscapedDash(char character, char nextCharacter)
        {
            return character == DashChar && nextCharacter == DashChar;
        }

        private static bool IsLastChar(int currentCharacterPosition, int characterLength)
        {
            return currentCharacterPosition == characterLength - 1;
        }
    }
}
