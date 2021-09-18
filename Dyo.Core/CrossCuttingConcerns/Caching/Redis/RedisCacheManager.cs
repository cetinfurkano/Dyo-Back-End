using Dyo.Core.Extensions;
using Dyo.Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        IConnectionMultiplexer _connectionMultiplexer;
        IDatabase _cache;
        public RedisCacheManager()
        {
            _connectionMultiplexer = ServiceTool.ServiceProvider.GetService<IConnectionMultiplexer>();
            _cache = _connectionMultiplexer.GetDatabase();
        }

        public async Task AddAsync(string key, object value, int duration)
        {
             await _cache.SetRecordAsync(key, value, TimeSpan.FromSeconds(duration));
        }

        public async Task<object> GetAsync(string key)
        {
            return await _cache.GetRecordAsync(key);
        }

        public async Task<bool> IsAddAsync(string key)
        {
            return await _cache.KeyExistsAsync(key);
            
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }
        public async Task RemoveByPatternAsync(string pattern)
        {
                var server = _connectionMultiplexer.GetServer("localhost", 6379);

                if (server != null)
                {
                    foreach (var key in server.Keys(pattern: $"*{pattern}*"))
                    {
                       await _cache.KeyDeleteAsync(key);
                    }
                }
            
        }

        
    }
}
