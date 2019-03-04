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
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            return services;
        }        
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string DbDriver = configuration["Data:DatabaseDriver"];

                if (DbDriver == DatabaseDrivers.SqlServer)
                {
                    options.UseSqlServer(configuration[ConnectionStringKeys.SqlServer]);
                }
                else if (DbDriver == DatabaseDrivers.MySQL)
                {
                    options.UseMySQL(configuration[ConnectionStringKeys.MySql]);
                }
                else if (DbDriver == DatabaseDrivers.SqlLite)
                {
                    options.UseSqlite(configuration[ConnectionStringKeys.SqlLite]);
                }
            });
            return services;
        }
        public static IServiceCollection RegisterCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ApiExceptionFilter>();
            return services;
        }
    }
}
