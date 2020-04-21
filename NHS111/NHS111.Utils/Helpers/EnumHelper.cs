
namespace NHS111.Utils.Helpers
{
    using System;

    public class EnumHelper
    {
        public static T ParseEnum<T>(string value, T defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}