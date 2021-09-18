using Dyo.Core.CrossCuttingConcerns.Caching;
using Dyo.Core.Utilities.IoC;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Dyo.WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CacheRemoveAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _pattern;
        private ICacheManager _cacheManager;

        public CacheRemoveAttribute(string pattern)
        {
            _pattern = pattern;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await _cacheManager.RemoveByPatternAsync(_pattern);

            await next();
        }
    }
}
