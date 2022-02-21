using Pokedex.Api.Domain.Clients.PokeApi;
using Pokedex.Api.Domain.Models;
using Refit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Api.Domain.Services
{
    public interface ITranslationService
    {
        public Task TranslateDescriptionAsync(Pokemon pokemon, CancellationToken cancellation = default);
    }

    class TranslationService : ITranslationService
    {
        readonly ITranslatorApi translatorApi;

        const string Cave = nameof(Cave);

        public TranslationService(ITranslatorApi translatorApi)
        {
            this.translatorApi = translatorApi;
        }

        public async Task TranslateDescriptionAsync(Pokemon pokemon, CancellationToken cancellation = default)
        {
            ApiResponse<TranslateResponse> apiResponse;

            if (pokemon.IsLegendary || string.Equals(pokemon.Habitat, Cave, StringComparison.InvariantCultureIgnoreCase))
                apiResponse = await translatorApi.GetYodaTranslationAsync(new TranslateRequest(pokemon.Description), cancellation);
            else
                apiResponse = await translatorApi.GetShakespeareTranslationAsync(new TranslateRequest(pokemon.Description), cancellation);

            if (apiResponse.IsSuccessStatusCode && apiResponse.Content.Success.Total != 0)
                pokemon.Description = apiResponse.Content.Contents.Translated;

            return;
        }
    }

}
