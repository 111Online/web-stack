using System.Net.Http;

namespace NHS111.Utils.Extensions
{
    public static class StringExtension
    {
        public static HttpResponseMessage AsHttpResponse(this string s)
        {
            return new HttpResponseMessage { Content = new StringContent(s) };
        }

        public static string FirstToUpper(this string s)
        {
            if (s == null) 
                return null;
            
            if (s.Length > 1) 
                return char.ToUpper(s[0]) + s.Substring(1);
            
            return s.ToUpper();
        }

        public static string ToTitleCase(this string s) {
            if (string.IsNullOrEmpty(s))
                return s;

            s = s.ToLower();
            return FirstToUpper(s);
        }
    }
}