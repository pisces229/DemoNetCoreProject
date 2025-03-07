namespace DemoNetCoreProject.Batch
{
    public class CommandLineArguments
    {
        public static string PROG_ID => Environment.GetCommandLineArgs().Skip(1).First();
    }
}
