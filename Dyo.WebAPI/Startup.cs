using Autofac;
using Dyo.Business.DependencyResolvers;
using Dyo.Business.DependencyResolvers.Autofac;
using Dyo.Business.Mapping;
using Dyo.Core.DataAccess.MongoDB;
using Dyo.Core.DependencyResolvers;
using Dyo.Core.Extensions;
using Dyo.Core.Utilities.IoC;
using Dyo.Core.Utilities.Security.Encryption;
using Dyo.Core.Utilities.Security.JWT;
using Dyo.WebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dyo.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddOptions();

            //services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDBSettings"));

            services.AddSingleton<IMongoDBSettings>(serviceProvider =>
                    serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value);

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));


            services.AddAutoMapper(typeof(ModelToDtoProfile), typeof(DtoToModelProfile));

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidIssuer = tokenOptions.Issuer,
                      ValidAudience = tokenOptions.Audience,
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                  };
              });

            services.AddDependencyResolvers(new ICoreModule[] {
               new CoreModule()
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacBusinessModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(opts =>
                {
                    opts.WithOrigins(new string[]
                    {
                        "http://localhost:3000",
                        "http://localhost:3001"
                        // whatever domain/port u are using
                    });

                    opts.AllowAnyHeader();
                    opts.AllowAnyMethod();
                    opts.AllowCredentials();
                });
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
