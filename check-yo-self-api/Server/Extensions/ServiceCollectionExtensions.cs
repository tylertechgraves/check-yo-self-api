using check_yo_self_api.Server.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using check_yo_self_api.Configuration;
using System.Collections.Generic;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Security;
using System.Linq;
using System;
using NSwag;

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
                    var version = configuration[ConnectionStringKeys.MySqlVersion];

                var versionArray = version.Split('.');
                if (versionArray.Length != 3)
                    throw new Exception("MySql version must be specified in the form of x.x.x (e.g. 5.7.12)");

                var result = 0;
                var versionIntArray = new int[3];
                for (var i = 0; i < 3; i++)
                {
                    if (int.TryParse(versionArray[i], out result))
                    {
                        versionIntArray[i] = result;
                    }
                    else
                        throw new Exception("MySql version can only container integers (e.g. 5.7.12)");
                }

                options.UseMySql(configuration[ConnectionStringKeys.MySql], new MySqlServerVersion(new Version(versionIntArray[0], versionIntArray[1], versionIntArray[2])));
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

        public static IServiceCollection AddVersionedApiDocs(this IServiceCollection services, IConfiguration configuration, string title, List<string> versionNumbers, IList<IOperationProcessor> customOperationProcessors = null, IList<IDocumentProcessor> customDocumentProcessors = null)
        {
            foreach (var version in versionNumbers)
            {
                services.AddOpenApiDocument(config =>
                {
                    config.Title = title;
                    config.Version = version;
                    config.DocumentName = version;
                    config.ApiGroupNames = new[] { version };

                    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));

                    if (customOperationProcessors != null && customOperationProcessors.Any())
                    {
                        foreach (var processor in customOperationProcessors)
                        {
                            config.OperationProcessors.Add(processor);
                        }
                    }

                    var authorization_endpoint = configuration.GetValue<string>("OpenApiUI:authorization_endpoint");
                    var token_endpoint = configuration.GetValue<string>("OpenApiUI:token_endpoint");
                    var additional_scopes = configuration.GetValue<string>("OpenApiUI:additional_scopes", string.Empty);
                    var include_oidc_scopes = configuration.GetValue<bool>("OpenApiUI:include_oidc_scopes");
                    if (!string.IsNullOrEmpty(authorization_endpoint) && !string.IsNullOrEmpty(token_endpoint))
                    {
                        if (include_oidc_scopes)
                            additional_scopes += " openid profile email";
                        var scopes = additional_scopes.Split(" ", StringSplitOptions.RemoveEmptyEntries).Distinct().ToDictionary(s => s, s => string.Empty);

                        config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("OpenIdConnect"));
                        config.DocumentProcessors.Add(
                            new SecurityDefinitionAppender("OpenIdConnect",
                                new OpenApiSecurityScheme
                                {
                                    Type = OpenApiSecuritySchemeType.OAuth2,
                                    Flow = OpenApiOAuth2Flow.AccessCode,
                                    Flows = new OpenApiOAuthFlows
                                    {
                                        AuthorizationCode = new OpenApiOAuthFlow
                                        {
                                            AuthorizationUrl = authorization_endpoint,
                                            TokenUrl = token_endpoint,
                                            Scopes = scopes
                                        }
                                    }
                                })
                        );
                    }

                    config.DocumentProcessors.Add(
                        new SecurityDefinitionAppender("JWT",
                            new OpenApiSecurityScheme
                            {
                                Type = OpenApiSecuritySchemeType.ApiKey,
                                Name = "Authorization",
                                In = OpenApiSecurityApiKeyLocation.Header,
                                Description = "Type into the textbox: Bearer {your JWT token}."
                            })
                    );

                    if (customDocumentProcessors != null && customDocumentProcessors.Any())
                    {
                        foreach (var processor in customDocumentProcessors)
                        {
                            config.DocumentProcessors.Add(processor);
                        }
                    }
                }
                );
            }
            return services;
        }
    }
}
