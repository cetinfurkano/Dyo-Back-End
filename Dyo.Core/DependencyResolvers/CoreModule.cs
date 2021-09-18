using Dyo.Core.CrossCuttingConcerns.Caching;
using Dyo.Core.CrossCuttingConcerns.Caching.Redis;
using Dyo.Core.Utilities.Cloud;
using Dyo.Core.Utilities.Cloud.CloudinaryCloud;
using Dyo.Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;



namespace Dyo.Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, RedisCacheManager>();
            serviceCollection.AddSingleton<ICloudRepo, CloudinaryImageUpload>();
        }
       
    }
}
