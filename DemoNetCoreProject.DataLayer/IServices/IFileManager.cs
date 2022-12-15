using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface IFileManager
    {
        string CombineFilePath(string root, string value);
        string CombineDirectoryPath(string root, params string[] value);
        void DeleteDirectory(string path);
        void DeleteFile(string path);
    }
}
