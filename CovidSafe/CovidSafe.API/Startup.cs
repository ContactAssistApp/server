using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using CovidSafe.API.Swagger;
using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Repositories.Cosmos;
using CovidSafe.DAL.Repositories.Cosmos.Client;
using CovidSafe.DAL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiContrib.Core.Formatter.Protobuf;

namespace CovidSafe.API
{
    /// <summary>
    /// Service registration for the web application
    /// </summary>
    /// <remarks>
    /// CS1591: Ignores missing documentation warnings.
    /// CodeCoverageExclusion: Required DI injections and core Startup procedures.
    /// </remarks>
#pragma warning disable CS1591
    [ExcludeFromCodeCoverage]
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
            // Enable AppInsights
            services.AddApplicationInsightsTelemetry();

            // Controller setup
            services
                .AddMvc(
                    option =>
                    {
                        // Use default ProtobufFormatterOptions
                        ProtobufFormatterOptions formatterOptions = new ProtobufFormatterOptions();
                        option.InputFormatters.Insert(1, new ProtobufInputFormatter(formatterOptions));
                        option.OutputFormatters.Insert(1, new ProtobufOutputFormatter(formatterOptions));
                        option.FormatterMappings.SetMediaTypeMappingForFormat(
                            "protobuf", 
                            MediaTypeHeaderValue.Parse("application/x-protobuf")
                        );
        }
                )
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Get configuration sections
            services.Configure<CosmosSchemaConfigurationSection>(this.Configuration.GetSection("CosmosSchema"));

            // Configure data repository implementations
            services.AddTransient(cf => new CosmosConnectionFactory(this.Configuration["CosmosConnection"]));
            services.AddTransient<CosmosContext>();
            services.AddSingleton<IMatchMessageRepository, CosmosMatchMessageRepository>();

            // Configure service layer
            services.AddSingleton<IMessageService, MessageService>();

            // Enable API versioning
            services.AddApiVersioning(o =>
            {
                // Share supported API versions in headers
                o.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // API versions formatted as 'v'major[.minor][-status]
                    options.GroupNameFormat = "'v'VVV";
                    // Enable API version in URL
                    options.SubstituteApiVersionInUrl = true;
                }
            );

            // Add Swagger generator
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
            services.AddSwaggerGen(c =>
            {
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

            // Add Swagger
            app.UseSwagger(c =>
            {
                // Add API hosts
                c.PreSerializeFilters.Add((swagger, httpRequest) =>
                {
                    // Parse host list from configuration
                    List<OpenApiServer> servers = this.Configuration["SwaggerHosts"]
                        .Split(';')
                        .Select(s => new OpenApiServer
                        {
                            Url = s
                        })
                        .ToList();

                    // Set servers (hosts) property
                    swagger.Servers = servers;
                });
            });
        }
    }
#pragma warning restore CS1591
}
