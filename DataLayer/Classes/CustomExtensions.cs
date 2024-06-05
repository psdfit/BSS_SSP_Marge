using System;

namespace DataLayer.Classes
{
    public static class CustomExtensions
    {
        public static int ParseInt(this string value, int defaultIntValue = 0)
        {
            if (int.TryParse(value, out int parsedInt))
            {
                return parsedInt;
            }

            return defaultIntValue;
        }

        public static decimal ParseDecimal(this string value, decimal defaultIntValue = 0)
        {
            if (decimal.TryParse(value, out decimal parsedDecimal))
            {
                return parsedDecimal;
            }

            return defaultIntValue;
        }

        public static string GetDateSTR(this string val)
        {
            if (String.IsNullOrEmpty(val))
                return val;
            return Convert.ToDateTime(val).Date.ToString();
        }

        public static DateTime GetDate(this string val)
        {
            if (String.IsNullOrEmpty(val))
                return new DateTime();
            return Convert.ToDateTime(val);
        }

        public static int? ParseNullableInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return value.ParseInt();
        }
    }
}