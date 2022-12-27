using DemoNetCoreProject.Common.Dtos;
using System.Reflection;

namespace DemoNetCoreProject.Common.Enums
{
    public abstract class Enumeration
    {
        public string Text { get; private set; }
        public string Value { get; private set; }
        protected Enumeration(string value, string text) => (Value, Text) = (value, text);
        public static List<CommonOptionOutputDto> GetOption() =>
            typeof(Enumeration)
            .GetFields(BindingFlags.Public |  BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<Enumeration>()
            .Select(s => new CommonOptionOutputDto() { Value = s.Value, Text = s.Text })
            .ToList();
    }
}
