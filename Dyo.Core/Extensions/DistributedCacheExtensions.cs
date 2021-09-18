using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Dyo.Core.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync(this IDistributedCache cache,string recordId,
            object data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null )
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = unusedExpireTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<object> GetRecordAsync(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if(jsonData is null)
            {
                return default(object);
            }

            return JsonSerializer.Deserialize<object>(jsonData);
        }
    }
}
