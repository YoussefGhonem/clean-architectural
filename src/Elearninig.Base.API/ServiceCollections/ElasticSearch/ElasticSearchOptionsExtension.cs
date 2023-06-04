using Elearninig.Base.API.ServiceCollections.ElasticSearch.models;
using Microsoft.Extensions.Configuration;

namespace Elearninig.Base.API.ServiceCollections.ElasticSearch;
public static class ElasticSearchOptionsExtension
{
    public static ElasticSearchOptions GetElasticSearchOptions(this IConfiguration configuration)
    {
        // options pattern
        var elasticSearchOptions = configuration.GetSection("ElasticSearch").Get<ElasticSearchOptions>();
        if (elasticSearchOptions is null)
        {
            throw new Exception("Missing 'Elastic Search' configuration section from the appsettings.");
        }

        return elasticSearchOptions;
    }
}
