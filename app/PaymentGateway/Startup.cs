using System;
using System.Collections.Generic;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PaymentGateway.Auth;
using PaymentGateway.Controllers.ApiHelpers;
using PaymentGateway.Models.Auth;
using PaymentGateway.Services;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            ConfigAuth(services);
            services.AddProblemDetails();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            })
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
                    });

            services.AddApiVersioning();
            services.AddApiVersioning(config =>
            {
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


                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });


            })
            .AddSwaggerExamplesFromAssemblyOf<Startup>()
            .AddSwaggerGenNewtonsoftSupport();




        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            Console.WriteLine($"\n\n ######    ENVIRONMENT = {env.EnvironmentName}   #####");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseProblemDetails();

            app.UseSwagger()
            .AddSwaggerUiEndpoints();

            app.UseRouting();

            // Always after routing
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMvc();

            app
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapControllers();
                });

        }

        private void ConfigAuth(IServiceCollection services)
        {


            services.AddAuthentication(CustomTokenAuthenticationOptions.CustomTokenAuthenticationSchema)
                    .AddScheme<CustomTokenAuthenticationOptions, CustomAuthenticationHandler>(CustomTokenAuthenticationOptions.CustomTokenAuthenticationSchema, opts => { });
            services.AddAuthorization();

            services.AddSingleton<ICredentialTokenManager>(provider =>
            {

                var validCreds = new[] {
                    new AuthCredentials("User1", "Pwd1"),
                    new AuthCredentials("User2", "Pwd2"),
                };

                var appConfig = provider.GetRequiredService<IAppConfig>();

                return new CredentialTokenManager(appConfig.TokenExpiry, validCreds);

            });

        }

    }
}
