using Microsoft.Extensions.Configuration;

namespace Pokedex.Api.Domain.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T Bind<T>(this IConfiguration configuration, string? name = null) where T : new()
        {
            if (name != null && string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("cannot be empty or whitespace", nameof(name));

            var value = new T();

            configuration
                .GetSection(name ?? typeof(T).Name)
                .Bind(value);

            return value;
        }
    }
}
