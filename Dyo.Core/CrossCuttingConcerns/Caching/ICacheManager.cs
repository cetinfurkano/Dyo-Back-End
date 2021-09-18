using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        Task<object> GetAsync(string key);
        Task AddAsync(string key, object value, int duration);
        Task<bool> IsAddAsync(string key);
        Task RemoveAsync(string key);
        Task RemoveByPatternAsync(string pattern);
    }
}
