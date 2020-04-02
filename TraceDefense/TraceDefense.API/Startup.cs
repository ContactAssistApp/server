using System;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TraceDefense.DAL.Repositories;
using TraceDefense.DAL.Repositories.Cosmos;
using TraceDefense.DAL.Repositories.Mock;
using TraceDefense.DAL.Services;

namespace TraceDefense.API
{
    /// <summary>
    /// Service registration for the web application
    /// </summary>
    /// <remarks>
    /// Ignores missing documentation warnings.
    /// </remarks>
#pragma warning disable CS1591
    public class Startup
    {
        /// <summary>
        /// Application configuration singleton
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Creates a new <see cref="Startup"/> instance
        /// </summary>
        /// <param name="configuration">Application configuration singleton</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Get configuration for data repository
            services.Configure<CosmosCovidSafeSchemaOptions>(this.Configuration.GetSection("CosmosCovidSafeSchema"));

            // Configure data repository implementations
            services.AddTransient<CosmosConnectionFactory>();
            //services.AddSingleton<IProximityQueryRepository, CosmosMessageRepository>();
            services.AddSingleton<IMessageRepository, MockMessageRepository>();

            //services.AddSingleton<IProximityQueryRepository, CosmosMessageInfoRepository>();
            services.AddSingleton<IMessageInfoRepository, MockMessageInfoRepository>();


            // Configure service layer
            services.AddSingleton<IProximityQueryService, ProximityQueryService>();

            // Add Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = "API enabling CovidSafe client applications to communicate with backend services.",
                    Title = "CovidSafe",
                    Version = "v1"
                });

                // Set the comments path for the Swagger JSON
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            // Add Swagger UI
            app.UseSwagger();
        }
    }
#pragma warning restore CS1591
}
