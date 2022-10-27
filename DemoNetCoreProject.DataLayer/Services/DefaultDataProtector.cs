using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.AspNetCore.DataProtection;

namespace DemoNetCoreProject.DataLayer.Services
{
    public class DefaultDataProtector : IDefaultDataProtector
    {
        private readonly IDataProtector _dataProtector;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        public DefaultDataProtector(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _dataProtector = dataProtectionProvider.CreateProtector("");
        }
        public string Protect(string plaintext) => _dataProtector.Protect(plaintext);
        public string Unprotect(string plaintext) => _dataProtector.Unprotect(plaintext);
    }
}
