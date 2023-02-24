using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace check_yo_self_api.Server
{
    public class ProcessDbCommands
    {
        public static void Process(string[] args, IWebHost host)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));

            using var scope = services.CreateScope();
            var db = GetApplicationDbContext(scope);
            if (args.Contains("dropdb"))
            {
                Console.WriteLine("Dropping database");
                db.Database.EnsureDeleted();
            }

            Console.WriteLine("Migrating database");
            db.Database.Migrate();
        }

        private static ApplicationDbContext GetApplicationDbContext(IServiceScope services)
        {
            var db = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return db;
        }
    }
}