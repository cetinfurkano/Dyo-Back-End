using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dyo.Core.Extensions
{
   public static class IDatabaseExtensions
    {
        public static async Task SetRecordAsync(this IDatabase database, string recordId,
            object data,
            TimeSpan? absoluteExpireTime = null
            )
        {
            //var options = new DistributedCacheEntryOptions();

            //options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            //options.SlidingExpiration = unusedExpireTime;

            //var jsonData = JsonSerializer.Serialize(data);
            //await cache.SetStringAsync(recordId, jsonData, options);

            var expiry = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            var jsonData = JsonSerializer.Serialize(data);
            await database.StringSetAsync(recordId, jsonData, expiry);
        }

        public static async Task<object> GetRecordAsync(this IDatabase database, string recordId)
        {
            var jsonData = await database.StringGetAsync(recordId);
            if (jsonData.IsNull)
            {
                return default(object);
            }

            return JsonSerializer.Deserialize<object>(jsonData);
        }

    }
}
