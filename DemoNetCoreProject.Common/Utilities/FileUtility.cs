using System.Text.RegularExpressions;

namespace DemoNetCoreProject.Common.Utilities
{
    public class FileUtility
    {
        private static readonly Regex UnsafeDirectoryRegex = new("[:]+|[\\/]+|[\\\\]+|[\\.]{2,}");
        private static readonly Regex UnsafeFileRegex = new("[:]+|[\\/]+|[\\\\]+");
        public static void UnsafeDirectory(string value)
        {
            if (UnsafeDirectoryRegex.IsMatch(value))
            {
                throw new Exception("Unsafe Directory Name");
            }
        }
        public static void UnsafeFile(string value)
        {
            if (UnsafeFileRegex.IsMatch(value))
            {
                throw new Exception("Unsafe File Name");
            }
        }
        public static FileInfo GetFile(DirectoryInfo root, string name)
        {
            UnsafeFile(name);
            return new FileInfo(Path.Combine(root.FullName, name));
        }
        public static DirectoryInfo GetDirectory(DirectoryInfo root, params string[] paths)
        {
            paths.ToList().ForEach(f => UnsafeDirectory(f));
            return new DirectoryInfo(Path.Combine(new[] { root.FullName }.Concat(paths).ToArray()));
        }
    }
}
