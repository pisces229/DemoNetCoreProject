using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.Extensions.Caching.Memory;

namespace DemoNetCoreProject.DataLayer.Services
{
    internal class MemoryCache : ICache, IDisposable
    {
        protected readonly IMemoryCache _memoryCache;
        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public async Task Add<T>(string key, T value, TimeSpan expiry)
        {
            await Task.Run(() => _memoryCache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiry)));
        }
        public async Task<bool> Exists(string key)
        {
            return await Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }
        public async Task<T> Get<T>(string key)
        {
            return await Task.FromResult(_memoryCache.Get<T>(key));
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
            await Task.Run(() => _memoryCache.Remove(key));
        }
        public void Dispose()
        {
            if (_memoryCache != null)
            {
                _memoryCache.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
