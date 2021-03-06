﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using check_yo_self_api.Server;
using check_yo_self_api.Server.Startup;
using check_yo_self_api.Server.Extensions;
using check_yo_self_api.Server.Entities.Config;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace check_yo_self_api
{
    public class Startup
    {
        static string title = "check-yo-self-api";
        
        public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _env = env;
            _logger = logger;
        }

        public IConfiguration Configuration { get; set; }
        private IHostingEnvironment _env { get; set; }
        private readonly ILogger<Startup> _logger;


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppConfig>(Configuration)
                .AddOptions()
                .AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials());
                })
                .AddResponseCompression(options =>
                {
                    options.MimeTypes = DefaultMimeTypes.Get;
                })
                .AddCustomDbContext(Configuration)
                .AddMemoryCache()
                .RegisterCustomServices()
                .AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN")
                .AddCustomizedMvc()
                .AddSwaggerDocument(config => {
                    config.Title = title;
                })
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddHttpClient()
                .AddNodeServices(); // added last because it returns void and breaks the fluent API

            services.AddHealthChecks();

            //Setup token validation method
            ConfigureTokenValidation(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsProduction())
            {
                app.UseResponseCompression();
            }
            else
            {
                app.AddDevMiddlewares();
            }

            app.UseHealthChecks("/health");

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".po"] = "text/plain";

            app.SetupMigrations()
                .UseXsrf()
                .UseCors("AllowAll")
                .UseStaticFiles()
                .UseAuthentication()
                // Enable middleware to serve generated Swagger as a JSON endpoint
                .UseOpenApi()
                .UseMvc(routes =>
                {
                    // default route for MVC/API controllers
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });

            IHttpContextAccessor httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            Context.Configure(httpContextAccessor);
        }

        private void ConfigureTokenValidation(IServiceCollection services)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    //Grab Configuration
                    var oidcAuthority = Configuration.GetValue<string>("OIDC:Authority");
                    var audience = Configuration.GetValue<string>("TokenValidation:Audience");

                    //
                    options.Authority = oidcAuthority;
                    options.ApiName = audience;

                    // setting to false to promote working in a docker container
                    options.RequireHttpsMetadata = false; // _env.IsProduction();
                });
        }

    }
}
