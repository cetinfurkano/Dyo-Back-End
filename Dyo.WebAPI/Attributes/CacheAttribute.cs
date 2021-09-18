using Dyo.Core.CrossCuttingConcerns.Caching;
using Dyo.Core.Utilities.IoC;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Dyo.WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _duration;
        private readonly ICacheManager _cacheManager;
        public CacheAttribute(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var methodName = string.Format($"{controllerActionDescriptor.MethodInfo.ReflectedType.FullName}.{controllerActionDescriptor.MethodInfo.Name}");
            var args = context.ActionArguments.Keys.ToList();
            var key = $"{methodName}({string.Join(",", args.Select(x => x?.ToString() ?? "<Null>"))})";

            if (await _cacheManager.IsAddAsync(key))
            {
                var data = await _cacheManager.GetAsync(key);
                context.Result = new OkObjectResult(data);
                return;
            }
           
            var resultContext = await next();
            
            if (resultContext.Result is OkObjectResult okObjectResult)
            {
                var cachedData = okObjectResult.Value;
                await _cacheManager.AddAsync(key, cachedData, _duration);
            }
         }
    }
}
