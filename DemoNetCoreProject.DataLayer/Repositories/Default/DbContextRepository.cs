using DemoNetCoreProject.DataLayer.Entities;
using DemoNetCoreProject.DataLayer.IRepositories.Default;
using DemoNetCoreProject.DataLayer.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCoreProject.DataLayer.Repositories.Default
{
    public class DbContextRepository(DefaultDbContext _dbContext) : IDbContextRepository
    {
        public Task<List<Person>> GetPerson()
            => _dbContext.People.ToListAsync();
        public Task<List<Person>> GetPerson(string name)
            => _dbContext.People.Where(w => w.Name == name).ToListAsync();
        public Task<List<Person>> GetPersonWithAddress()
            => _dbContext.People.Include(i => i.Addresses).ToListAsync();
        public Task<List<Address>> GetAddress()
            => _dbContext.Addresses.ToListAsync();
        public Task<List<Address>> GetAddressWithPerson()
            => _dbContext.Addresses.Include(i => i.Person).ToListAsync();
    }
}
