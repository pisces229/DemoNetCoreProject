using AutoMapper;
using DemoNetCoreProject.Backend.Profiles;

namespace DemoNetCoreProject.Backend
{
    public class LoadBackendRegister
    {
        public static IEnumerable<Profile> Profiles() => new Profile[]
        {
            new CommonProfile(),
            new DefaultProfile(),
        };
    }
}
