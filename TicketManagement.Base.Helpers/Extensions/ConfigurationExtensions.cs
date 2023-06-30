using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace TicketManagement.Base.Helpers.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetEnvironmentValue<T>(this IConfiguration configuration, string key)
        {
            var env = configuration.GetValue<string>("EnvironmentName");
            if (!string.IsNullOrEmpty(env) && env.Trim().ToLower() == "production")
            {
                return GetProductionValue<T>(configuration, key);
            }

            return GetDevelopmentValue<T>(configuration, key);
        }

        public static T GetEnvironmentSection<T>(this IConfiguration configuration, string key)
        {
            var env = configuration.GetValue<string>("EnvironmentName");
            if (!string.IsNullOrEmpty(env) && env.Trim().ToLower() == "production")
            {
                return GetProductionSection<T>(configuration, key);
            }

            return GetDevelopmentSection<T>(configuration, key);
        }

        public static IConfigurationSection GetEnvironmentSection(this IConfiguration configuration, string key)
        {
            var env = configuration.GetValue<string>("EnvironmentName");
            if (!string.IsNullOrEmpty(env) && env.Trim().ToLower() == "production")
            {
                return GetProductionSection<IConfigurationSection>(configuration, key);
            }

            return GetDevelopmentSection<IConfigurationSection>(configuration, key);
        }

        //public static T GetFromEnvironment<T> (this IConfigurationSection configurationSection)
        //{
        //    return 
        //}

        private static T GetDevelopmentSection<T>(IConfiguration configuration, string key)
        {
            return configuration.GetSection(key).Get<T>();
        }

        private static T GetProductionSection<T>(IConfiguration configuration, string key)
        {
            var client = GetSecretClient(configuration);
            var developmentValue = GetDevelopmentSection<T>(configuration, key);
            if (developmentValue is null) return developmentValue;
            foreach (var property in developmentValue.GetType().GetProperties())
            {
                var propValue = (string?)property.GetValue(developmentValue, null);
                string? value = null;
                if (propValue != null)
                {
                    try
                    {
                        value = client.GetSecret(propValue).Value.Value;
                    }
                    catch (Exception e)
                    {
                        value = propValue;
                    }
                }
                property.SetValue(developmentValue, value ?? propValue, null);
            }
            return developmentValue;
        }

        private static T GetDevelopmentValue<T>(IConfiguration configuration, string key)
        {
            return configuration.GetValue<T>(key);
        }

        private static T GetProductionValue<T>(IConfiguration configuration, string key)
        {
            var client = GetSecretClient(configuration);
            var developmentValue = GetDevelopmentValue<T>(configuration, key);
            if (developmentValue is null) return developmentValue;
            if (developmentValue.GetType().Equals(typeof(string))) return developmentValue;
            return (T)(object)client.GetSecret((string)(object)developmentValue).Value.Value;
        }

        private static SecretClient GetSecretClient(IConfiguration configuration)
        {
            var clientId = configuration.GetValue<string>("AzureVaultConfig:ClientId");
            var aZVaultUri = configuration.GetValue<string>("AzureVaultConfig:VaultUri");

            var options = new SecretClientOptions()
            {
                Retry =
            {
                Delay = TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
            };
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            { ManagedIdentityClientId = clientId });
            return new SecretClient(new Uri(aZVaultUri), credential, options);
        }
    }
}
