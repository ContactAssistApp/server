﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using CovidSafe.API.Swagger;
using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Repositories.Cosmos;
using CovidSafe.DAL.Repositories.Cosmos.Client;
using CovidSafe.DAL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
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

            #region Database Configuration

            // Get configuration sections
            services.Configure<CosmosSchemaConfigurationSection>(this.Configuration.GetSection("CosmosSchema"));

            // Create Cosmos Connection, based on connection string location
            if(!String.IsNullOrEmpty(this.Configuration.GetConnectionString("CosmosConnection"))) {
                services.AddTransient(cf => new CosmosConnectionFactory(this.Configuration.GetConnectionString("CosmosConnection")));
            }
            else
            {
                // Attempt to pull from generic 'CosmosConnection' setting
                // Throws exception if not defined
                services.AddTransient(cf => new CosmosConnectionFactory(this.Configuration["CosmosConnection"]));
            }
            
            // Configure data repository implementations
            services.AddTransient<CosmosContext>();
            services.AddSingleton<IInfectionReportRepository, CosmosInfectionReportRepository>();

            #endregion

            // Add AutoMapper profiles
            services.AddAutoMapper(
                typeof(v20200415.MappingProfiles),
                typeof(v20200505.MappingProfiles)
            );

            // Configure service layer
            services.AddSingleton<IInfectionReportService, InfectionReportService>();

            // Enable API versioning
            services.AddApiVersioning(o =>
            {
                o.ApiVersionReader = new QueryStringApiVersionReader("api-version");
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(
                    DateTime.Parse(this.Configuration["DefaultApiVersion"])
                );
                // Share supported API versions in headers
                o.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // Enable API version in URL
                    options.SubstituteApiVersionInUrl = true;
                    options.DefaultApiVersion = new ApiVersion(
                        DateTime.Parse(this.Configuration["DefaultApiVersion"])
                    );
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
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

            // Add SwaggerUI
            app.UseSwaggerUI(c =>
            {
                // Enable UI for multiple API versions
                // Descending operator forces latest version to appear first
                foreach(ApiVersionDescription description in provider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion))
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant()
                    );
                }
            });
        }
    }
#pragma warning restore CS1591
}
