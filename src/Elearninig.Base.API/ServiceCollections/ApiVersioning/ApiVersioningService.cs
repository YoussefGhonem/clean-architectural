using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Elearninig.Base.API.ServiceCollections.ApiVersioning
{
    public static class ApiVersioningService 
    {
        public static IServiceCollection AddApiVersioningService(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Sets the default API version to 1.0. This version will be used if no version is specified in the request.
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // Enables reporting of API versions in the response headers. This allows clients to determine the available API versions.
                config.ReportApiVersions = true;
                // Instructs the API to assume the default version when the requested version is not explicitly specified.
                config.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddVersionedApiExplorer(config =>
            {
                // Specifies the format for the API version group names. In this case,
                // the format is set to 'v'VVV, where VVV represents the version number.
                // For example, "v1" for version 1.0.
                config.GroupNameFormat = "'v'VVV";
                // Indicates that the API version should be substituted in the URL when generating URLs
                // for the API endpoints.This ensures that the appropriate version is included in the URL.
                config.SubstituteApiVersionInUrl = true;

                //This affects how the API version is extracted from the request.
                         // config.ApiVersionParameterSource
            });
            return services;

        }
    }
}
