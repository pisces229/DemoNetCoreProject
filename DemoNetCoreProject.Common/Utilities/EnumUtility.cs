using DemoNetCoreProject.Common.Dtos;
using DemoNetCoreProject.Common.Enums;
using System.Reflection;

namespace DemoNetCoreProject.Common.Utilities
{
    public class EnumUtility
    {
        public static List<CommonOptionOutputDto> GetOption<T>() where T : Enumeration =>
            typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>()
            .Select(s => new CommonOptionOutputDto() { Value = s.Value, Text = s.Text })
            .ToList();
        public static string? GetText<T>(string value) where T : Enumeration =>
            GetOption<T>()
            .Where(p => p.Value == value)
            .Select(s => s.Text)
            .FirstOrDefault();
    }
}
