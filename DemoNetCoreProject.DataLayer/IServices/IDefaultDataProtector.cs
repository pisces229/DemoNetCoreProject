using System;

namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface IDefaultDataProtector
    {
        string Protect(string value);
        string Unprotect(string value);
    }
}
