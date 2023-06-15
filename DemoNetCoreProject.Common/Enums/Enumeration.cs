namespace DemoNetCoreProject.Common.Enums
{
    public abstract class Enumeration
    {
        public string Text { get; private set; }
        public string Value { get; private set; }
        protected Enumeration(string value, string text) => (Value, Text) = (value, text);
    }
}
