using DemoNetCoreProject.Common.Enums;

namespace DemoNetCoreProject.DataLayer.Enums
{
    public class DefaultEnum : Enumeration
    {
        public static DefaultEnum First = new("1", "FIRST");
        public static DefaultEnum Second = new("2", "SECOND");
        public static DefaultEnum Third = new("3", "THIRD");
        public DefaultEnum(string value, string text)
            : base(value, text)
        {
        }
    }
}
