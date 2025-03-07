using AutoMapper;
using DemoNetCoreProject.Common.Utilities;

namespace DemoNetCoreProject.Common.Converter
{
    public class CommonConverter
    {
        public class RocDateToDateTimeConverter : ITypeConverter<string?, DateTime?>
        {
            public DateTime? Convert(string? source, DateTime? destination, ResolutionContext context)
            {
                return ConvertUtility.RocDateToDateTime(source);
            }
        }
        public class DateTimeToRocDateConverter : ITypeConverter<DateTime?, string?>
        {
            public string? Convert(DateTime? source, string? destination, ResolutionContext context)
            {
                return ConvertUtility.DateTimeToRocDate(source);
            }
        }
        public class ToIntegerConverter : ITypeConverter<string?, int?>
        {
            public int? Convert(string? source, int? destination, ResolutionContext context)
            {
                return ConvertUtility.ToInteger(source);
            }
        }

        public class ToStringConverter : ITypeConverter<int?, string>
        {
            public string Convert(int? source, string destination, ResolutionContext context)
            {
                return ConvertUtility.ToString(source);
            }
        }
    }
}
