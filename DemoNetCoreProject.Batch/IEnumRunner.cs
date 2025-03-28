namespace DemoNetCoreProject.Batch
{
    public interface IEnumRunner
    {
        Task Run();
    }
    public enum EnumRunner
    {
        First,
        Second,
    }
}
