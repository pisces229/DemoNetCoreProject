namespace DemoNetCoreProject.DataLayer.IServices
{
    public interface ICache
    {
        Task Add<T>(string key, T value, TimeSpan expiry);
        Task<bool> Exists(string key);
        Task<T> Get<T>(string key);
        Task Replace<T>(string key, T value, TimeSpan expiry);
        Task Remove(string key);
        void Dispose();
    }
}
