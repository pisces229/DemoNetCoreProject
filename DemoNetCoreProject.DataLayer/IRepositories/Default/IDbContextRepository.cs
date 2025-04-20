using DemoNetCoreProject.DataLayer.Entities;

namespace DemoNetCoreProject.DataLayer.IRepositories.Default
{
    public interface IDbContextRepository
    {
        Task<List<Person>> GetPerson();
        Task<List<Person>> GetPersonWithAddress();
        Task<List<Address>> GetAddress();
        Task<List<Address>> GetAddressWithPerson();
        Task<List<Person>> GetPerson(string name);
        Task<List<Address>> GetAddress(string addressText, string personName);
    }
}
