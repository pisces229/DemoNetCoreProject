using System.Security.Cryptography;
using System.Text;

namespace DemoNetCoreProject.Common.Utilities
{
    internal class ConvertUtility
    {
        #region Mima
        public static string Mima(long ticks, string value)
        {
            var result = new StringBuilder();
            using (var sha = SHA256.Create())
            {
                var binary = sha.ComputeHash(Encoding.Default.GetBytes(ticks.ToString() + value));
                foreach (var data in binary)
                {
                    result.Append(data.ToString("x2"));
                }
            }
            return result.ToString();
        }
        #endregion

        #region Date
        public static DateTime? RocDateToDateTime(string? value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length == 7)
            {
                var year = (Convert.ToInt32(value[..3]) + 1911).ToString("0000");
                var month = value.Substring(3, 2);
                var day = value.Substring(5, 2);
                return DateTime.Parse($"{year}-{month}-{day}");
            }
            else
            {
                return null;
            }
        }
        public static string DateTimeToRocDate(DateTime? value)
        {
            if (value != null)
                return $"{value.Value.Year - 1911:000}{value.Value.Month:00}{value.Value.Day:00}";
            else
                return "";
        }
        public static string MonthDayFormat(DateTime? value)
        {
            if (value != null)
                return $"{value.Value.Month:00}/{value.Value.Day:00}";
            else
                return "";
        }
        public static string DateOnlyToRocDate(DateOnly? value)
        {
            if (value != null)
                return $"{value.Value.Year - 1911:000}{value.Value.Month:00}{value.Value.Day:00}";
            else
            {
                return "";
            }
        }
        public static string RocDateFormat(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length == 7)
                return value.Insert(3, "/").Insert(6, "/");
            else
                return "";
        }

        public static string DateTimeFormat(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length == 8)
                return value.Insert(4, "/").Insert(7, "/");
            else
                return "";
        }
        #endregion

        #region Year
        public static string YearToRocYear(int? value)
        {
            if (value != null)
                return (value.Value - 1911).ToString();
            else
                return "";
        }
        public static int? RocYearToYear(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return Convert.ToInt32(value) + 1911;
            else
                return null;
        }
        #endregion

        #region Integer
        public static string ToString(int? value)
        {
            if (value != null)
                return Convert.ToString(value)!;
            else
                return "";
        }
        public static int? ToInteger(string? value)
        {
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out _))
                return Convert.ToInt32(value!);
            else
                return null;
        }
        #endregion

        #region Decimal
        public static string ToString(decimal? value)
        {
            if (value != null)
                return Convert.ToString(value)!;
            else
                return "";
        }
        public static decimal? ToDecimal(string? value)
        {
            if (!string.IsNullOrEmpty(value) && decimal.TryParse(value, out _))
                return Convert.ToDecimal(value!);
            else
                return null;
        }
        #endregion
    }
}
