using System.IO;
using System.Security.Cryptography.X509Certificates;
using check_yo_self_api.Server.Entities;
using check_yo_self_api.Server.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using check_yo_self_api.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;

namespace check_yo_self_api.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {       
        public static IServiceCollection AddCustomizedMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ModelValidationFilter));
            })
            .AddNewtonsoftJson(options =>             
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            return services;
        }        
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string DbDriver = configuration["Data:DatabaseDriver"];

            if (DbDriver == DatabaseDrivers.SqlServer || DbDriver == DatabaseDrivers.SqlLite)
            {
                // Add framework services.
                services.AddDbContext<ApplicationDbContext>(options =>
                {  
                    if (DbDriver == DatabaseDrivers.SqlServer)
                    {
                        options.UseSqlServer(configuration[ConnectionStringKeys.SqlServer]);
                    }
                    else if (DbDriver == DatabaseDrivers.SqlLite)
                    {
                        options.UseSqlite(configuration[ConnectionStringKeys.SqlLite]);
                    }
                });
            }
            else if (DbDriver == DatabaseDrivers.MySQL)
            {
                services.AddDbContextPool<ApplicationDbContext>(options =>
                {
                options.UseMySql(configuration[ConnectionStringKeys.MySql], mySqlOptions => mySqlOptions
                    .ServerVersion(new ServerVersion(new Version(8, 0, 17)))); 
                });
            }
            return services;
        }
        public static IServiceCollection RegisterCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ApiExceptionFilter>();
            return services;
        }
    }
}
