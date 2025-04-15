using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace DemoNetCoreProject.DataLayer.Services
{
    public class RedisCache : ICache, IDisposable
    {
        protected readonly IDatabase _database;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        public RedisCache(IConfiguration configuration)
        {
            var configurationOptions = ConfigurationOptions.Parse(configuration.GetValue<string>("RedisConnectionStrings:Default"));
            configurationOptions.CertificateValidation += (request, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };
            _connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
            _database = _connectionMultiplexer.GetDatabase();
        }
        public async Task Add<T>(string key, T value, TimeSpan expiry)
        {
            var reuslt = _database.StringSet(key, JsonSerializer.SerializeToUtf8Bytes(value), expiry);
            await Task.FromResult(reuslt);
        }
        public async Task<bool> Exists(string key)
        {
            var reuslt = _database.KeyExists(key);
            return await Task.FromResult(reuslt);
        }
        public async Task<T> Get<T>(string key)
        {
            //JsonSerializer.Deserialize<T>(await _database.StringGetAsync(key));
            var result = JsonSerializer.Deserialize<T>(_database.StringGet(key));
            return await Task.FromResult(result!);
        }
        public async Task Replace<T>(string key, T value, TimeSpan expiry)
        {
            if (await Exists(key))
            {
                //await Remove(key);
                await Add(key, value, expiry);
            }
        }
        public async Task Remove(string key)
        {
            var result = _database.KeyDelete(key);
            await Task.FromResult(result);
        }
        public void Dispose()
        {
            if (_connectionMultiplexer != null)
            {
                _connectionMultiplexer.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
