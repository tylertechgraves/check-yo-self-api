using System.Collections.Generic;
using check_yo_self_api.Server;
using check_yo_self_api.Server.Entities.Config;
using check_yo_self_api.Server.Extensions;
using check_yo_self_api.Server.Startup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace check_yo_self_api;

public class Startup
{
    static readonly string _title = "check-yo-self-api";

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; set; }


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
                .AllowAnyMethod());
            })
            .AddResponseCompression(options =>
            {
                options.MimeTypes = DefaultMimeTypes.Get;
            })
            .AddCustomDbContext(Configuration)
            .AddMemoryCache()
            .RegisterCustomServices()
            .AddCustomizedMvc()
            .AddHttpContextAccessor()
            .AddHttpClient()
            .AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
            })
            // To add another api version, just add to this list
            .AddVersionedApiDocs(Configuration, _title, new List<string>() { "v1" })
            .AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'V";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        // https://docs.microsoft.com/en-us/aspnet/core/migration/31-to-50?view=aspnetcore-5.0&tabs=visual-studio#usedatabaseerrorpage-obsolete
        services.AddDatabaseDeveloperPageExceptionFilter();

        //Setup token validation method
        ConfigureTokenValidation(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        if (env.IsProduction())
        {
            app.UseResponseCompression();
        }
        else
        {
            app.AddDevMiddlewares();
            app.UseCustomSwaggerApi(Configuration, apiVersionDescriptionProvider);
        }

        app.SetupMigrations()
            .UseCors("AllowAll")
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapHealthChecks("/health").AllowAnonymous();
            })
            // Enable middleware to serve generated Swagger as a JSON endpoint
            .UseOpenApi();

        // Enable https on swagger in production. If you are planning to use http instead of https in production, 
        // please remove this code block
        app.UseOpenApi(configure =>
        {
            if (env.IsProduction())
            {
                configure.PostProcess = (document, _) => document.Schemes = new[] { NSwag.OpenApiSchema.Https };
            }
        });

        var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
        Context.Configure(httpContextAccessor);
    }

    private void ConfigureTokenValidation(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                //Grab Configuration
                var oidcAuthority = Configuration.GetValue<string>("OIDC:Authority");
                var audience = Configuration.GetValue<string>("TokenValidation:Audience");

                //
                options.Authority = oidcAuthority;
                options.Audience = audience;
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = !string.IsNullOrEmpty(audience)
                };

                // setting to false to promote working in a docker container
                options.RequireHttpsMetadata = false; // _env.IsProduction();
            });
    }

}
