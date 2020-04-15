using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CovidSafe.API.Swagger
{
    /// <summary>
    /// Configures Swagger generation options
    /// </summary>
    /// <remarks>
    /// CodeCoverageExclusion: Does not provide core functionality.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public class SwaggerConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        /// <summary>
        /// Configures API version description handling
        /// </summary>
        readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Creates a new <see cref="SwaggerConfiguration"/> instance
        /// </summary>
        /// <param name="provider">API Version Description provider implementation</param>
        public SwaggerConfiguration(IApiVersionDescriptionProvider provider) => this._provider = provider;

        /// <inheritdoc/>
        public void Configure(SwaggerGenOptions options)
        {
            // Generate a SwaggerDoc for each API version discovered in the project
            foreach(var description in this._provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateInfoForApiVersion(description)
                );
            }
        }

        /// <summary>
        /// Builds the description portion of a SwaggerDoc for each API version in the project
        /// </summary>
        /// <param name="description">Target API version information</param>
        /// <returns><see cref="OpenApiInfo"/></returns>
        public static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            OpenApiInfo info = new OpenApiInfo()
            {
                Description = "Enables communication between CovidSafe client applications and backend data storage.",
                Title = "CovidSafe API",
                Version = description.ApiVersion.ToString(),
            };

            if(description.IsDeprecated)
            {
                info.Description = "This API version has been deprecated. Please migrate to a newer version.";
            }

            return info;
        }
    }
}
