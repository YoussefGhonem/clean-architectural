using Elearninig.Base.API.Variables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Elearninig.Base.API.ServiceCollections.ElasticSearch
{
    public static class ServiceCollectionsExtension
    {
        // this method allows you to add Elasticsearch-related services and configure Serilog logging with Elasticsearch sink
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetElasticSearchOptions();
            var configurationRoot = (IConfigurationRoot)configuration;

            // Serilog Configuration : Elasticsearch provides a scalable and efficient storage solution for log data
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext() // This enriches log events with information from the logging context.
                .Enrich.WithExceptionDetails() //  This enriches log events with additional details about exceptions.
                .Enrich.WithMachineName() // This enriches log events with the machine name.
                .WriteTo.Debug() // This writes log events to the debug output.
                .WriteTo.Console() // This writes log events to the console output.
                                   // Writes log events to Elasticsearch using the Elasticsearch sink ( used to send data from a source system or application to Elasticsearch for indexing and storage..)
                .WriteTo.Elasticsearch(ConfigureElasticSink(config.Url!))
                .Enrich.WithProperty("Environment", EnvVariables.ENVIRONMENT_NAME)
                .ReadFrom.Configuration(configurationRoot)//This reads additional configuration settings for Serilog from the IConfigurationRoot instance.
                .CreateLogger();

            return services;
        }

        //This is a private method that configures the Elasticsearch sink options for Serilog.
        //It takes a URL parameter that represents the Elasticsearch endpoint.
        private static ElasticsearchSinkOptions ConfigureElasticSink(string url)
        {
            return new ElasticsearchSinkOptions(new Uri(url))
            {
                AutoRegisterTemplate = true, // : This enables automatic template registration with Elasticsearch.

                // In the context of Elasticsearch, the 'IndexFormat' refers to the naming format used for indexing data in Elasticsearch
                // When documents are stored in Elasticsearch,they are organized and stored in indexes.
                // The IndexFormat determines the structure and naming pattern of these indexes. 
                // This sets the format for the Elasticsearch index. It uses a combination of the application name,
                // environment name, and the current year and month to form a unique index name.
                IndexFormat =
                    $"{Assembly.GetEntryAssembly()?.GetName().Name?.ToLower().Replace(".", "-")}-{EnvVariables.ENVIRONMENT_NAME?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }
}
