



using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PaymentGateway.Controllers.ApiHelpers
{


    public static class SwaggerGenExtensions
    {


        public static SwaggerGenOptions AddVersioningOverrides(this SwaggerGenOptions options)
        {

            // Apply the filters
            options.OperationFilter<RemoveVersionFromParameter>();
            options.DocumentFilter<ReplaceVersionsInPath>();

            // Ensure the routes are added to the right Swagger doc
            options.DocInclusionPredicate((version, desc) =>
            {
                var versions = desc.CustomAttributes()
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions)
                    .ToArray();

                var maps = desc.CustomAttributes()
                    .OfType<MapToApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions)
                    .ToArray();

                return versions.Any(v => $"v{v.MajorVersion}" == version)
                            && (!maps.Any() || maps.Any(v => $"v{v.MajorVersion}" == version)); ;
            });


            return options;
        }

        public static SwaggerGenOptions AddSwaggerDocs(this SwaggerGenOptions options)
        {
            foreach (var apiVersion in GetVersions())
                options.SwaggerDoc($"v{apiVersion.MajorVersion}", new OpenApiInfo { Title = Constants.ApplicationName, Version = $"{apiVersion.MajorVersion}" });

            return options;
        }

        public static IApplicationBuilder AddSwaggerUiEndpoints(this IApplicationBuilder app)
        {

            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersion in GetVersions())
                    c.SwaggerEndpoint($"/swagger/v{apiVersion.MajorVersion}/swagger.json", $"v{apiVersion.MajorVersion}");

            });


            return app;
        }

        private static ApiVersion[] DeclaredApiVersions;
        private static ApiVersion[] GetVersions()
        {
            {
                if (DeclaredApiVersions == null)
                    DeclaredApiVersions = typeof(Startup)
                                    .Assembly
                                    .GetExportedTypes()
                                    .SelectMany(c => c.GetCustomAttributes(false))
                                    .OfType<ApiVersionAttribute>()
                                    .SelectMany(s => s.Versions)
                                    .Union(new[] { Constants.DefaultApiVersion } )
                                    .Distinct()
                                    .ToArray();

                return DeclaredApiVersions;

            }

        }
    }


}