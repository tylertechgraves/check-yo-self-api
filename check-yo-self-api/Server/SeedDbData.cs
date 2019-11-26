using System;
using System.Collections.Generic;
using System.Linq;
using check_yo_self_api.Server.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace check_yo_self_api.Server
{
    public class SeedDbData
    {
        readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnv;
        public SeedDbData(IWebHost host, ApplicationDbContext context)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            var serviceScope = services.CreateScope();
            _hostingEnv = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();
            _context = context;
        }
    }
}
