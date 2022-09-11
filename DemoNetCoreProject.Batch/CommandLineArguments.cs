using System;

namespace DemoNetCoreProject.Batch
{
    public class CommandLineArguments
    {
        public static string ENVIRONMENT => Environment.GetCommandLineArgs().Skip(1).First();
        public static string PROG_ID => Environment.GetCommandLineArgs().Skip(2).First();
    }
}
