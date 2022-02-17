using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pokedex.Api.Domain.Clients;
using Pokedex.Api.Domain.Clients.PokeApi;
using Pokedex.Api.Domain.Extensions;
using Pokedex.Api.Domain.Services;
using Refit;

namespace Pokedex.Api.Domain.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPokemonService, PokemonService>();
            services.AddSingleton<ITranslationService, TranslationService>();

            services.AddApiClient<IPokeApi>(configuration, "PokeApiSettings");
            services.AddApiClient<ITranslatorApi>(configuration, "TranslatorApiSettings");
        }

        static void AddApiClient<T>(this IServiceCollection services, IConfiguration configuration, string settingsName) where T : class
        {
            var settings = configuration.Bind<ApiSettings>(settingsName);

            if (settings.BaseUrl == null)
                throw new ArgumentNullException(nameof(settings.BaseUrl));

            var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer(
                new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }}));

            services.AddRefitClient<T>(refitSettings)
                .ConfigureHttpClient(c => 
                    c.BaseAddress = new Uri(settings.BaseUrl));
        }
    }
}
