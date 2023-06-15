using DemoNetCoreProject.Common.Constants;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace DemoNetCoreProject.DataLayer.Services
{
    public class FileManager : IFileManager
    {
        private const string UnsafeDirectoryPattern = "[:]+|[\\/]+|[\\\\]+|[\\.]{2,}";
        private const string UnsafeFilePattern = "[:]+|[\\/]+|[\\\\]+";
        private readonly string _pathTemp;
        public FileManager(IConfiguration configuration)
        {
            _pathTemp = configuration.GetValue<string>(ConfigurationConstant.PathTemp);
        }
        private void UnsafePath(string path)
        {
            if (!path.StartsWith(_pathTemp))
            {
                throw new Exception("Unsafe Path");
            }
        }
        private static void UnsafeDirectory(string value)
        {
            if (Regex.IsMatch(value, UnsafeDirectoryPattern))
            {
                throw new Exception("Unsafe Directory");
            }
        }
        private static void UnsafeFile(string value)
        {
            if (Regex.IsMatch(value, UnsafeFilePattern))
            {
                throw new Exception("Unsafe File");
            }
        }
        public string CombineFilePath(string root, string value)
        {
            UnsafePath(root);
            UnsafeFile(value);
            return Path.Combine(root, value);
        }
        public string CombineDirectoryPath(string root, params string[] value)
        {
            UnsafePath(root);
            foreach (var path in value)
            {
                UnsafeDirectory(path);
            }
            return Path.Combine(new[] { root }.Concat(value).ToArray());
        }
        public void DeleteDirectory(string path)
        {
            UnsafePath(path);
            Directory.Delete(path, true);
        }
        public void DeleteFile(string path)
        {
            UnsafePath(path);
            File.Delete(path);
        }
    }
}
