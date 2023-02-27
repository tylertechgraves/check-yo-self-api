using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;

namespace check_yo_self_api.Server.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomSwaggerApi(this IApplicationBuilder app, IConfiguration configuration, IApiVersionDescriptionProvider provider)
    {
        // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
        app.UseSwaggerUi3(settings =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                settings.SwaggerRoutes.Add(new SwaggerUi3Route(description.GroupName, $"/swagger/{description.GroupName}/swagger.json"));
            }

            var client_id = configuration.GetValue<string>("OpenApiUI:client_id");
            if (!string.IsNullOrEmpty(client_id))
            {
                settings.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = client_id,
                    ClientSecret = string.Empty,
                    AppName = "Swagger UI App (leave secret blank)",
                    UsePkceWithAuthorizationCodeGrant = true
                };
            }
        });

        return app;
    }

    public static IApplicationBuilder AddDevMiddlewares(this IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
        return app;
    }

    public static IApplicationBuilder SetupMigrations(this IApplicationBuilder app)
    {
        // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
        try
        {
            var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
        }
        catch (Exception) { }
        return app;
    }
}
