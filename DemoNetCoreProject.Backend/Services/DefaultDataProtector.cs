using Microsoft.AspNetCore.DataProtection;

namespace DemoNetCoreProject.Backend.Services
{
    public class DefaultDataProtector
    {
        private readonly IDataProtector _dataProtector;
        public DefaultDataProtector(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("");
        }
        public string Protect(string plaintext) => _dataProtector.Protect(plaintext);
        public string Unprotect(string plaintext) => _dataProtector.Unprotect(plaintext);
    }
}
