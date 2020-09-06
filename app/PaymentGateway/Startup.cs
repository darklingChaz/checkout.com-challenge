using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PaymentGateway.Controllers.ApiHelpers;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => {
                        options.EnableEndpointRouting = false;
                    })
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });

            services.AddApiVersioning();
            services.AddApiVersioning(config => {
                config.DefaultApiVersion = Constants.DefaultApiVersion;
                config.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.AddSwaggerDocs()
                       .AddVersioningOverrides();

                options.MapType<Version>(() => new OpenApiSchema { Type = "string" });
                options.ExampleFilters();
                options.EnableAnnotations();
            })
            .AddSwaggerExamplesFromAssemblyOf<Startup>()
            .AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
               .AddSwaggerUiEndpoints();
            
            app.UseMvc();
            app.UseRouting();

            app
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapControllers();
                });

        }
    }
}
