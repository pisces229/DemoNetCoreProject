using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DemoNetCoreProject.DataLayer.Services
{
    public class DatabaseCache : ICache, IDisposable
    {
        private readonly IDistributedCache _distributedCache;
        public DatabaseCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task Add<T>(string key, T value, TimeSpan expiry)
        {
            await _distributedCache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expiry
            });
        }
        public async Task<bool> Exists(string key)
        {
            var reuslt = _distributedCache.Get(key);
            return await Task.FromResult(reuslt != null);
        }
        public async Task<T> Get<T>(string key)
        {
            var result = JsonSerializer.Deserialize<T>(_distributedCache.Get(key));
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
            await _distributedCache.RemoveAsync(key);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
