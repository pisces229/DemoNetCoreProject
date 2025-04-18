using DemoNetCoreProject.DataLayer.Entities;

namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    public interface IDbContextRepository
    {
        Task<List<Person>> GetPerson();
        Task<List<Person>> GetPerson(string name);
        Task<List<Person>> GetPersonWithAddress();
        Task<List<Address>> GetAddress();
        Task<List<Address>> GetAddressWithPerson();
    }
}
