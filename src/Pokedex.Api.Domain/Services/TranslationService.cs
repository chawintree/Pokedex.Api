using Pokedex.Api.Domain.Clients.PokeApi;
using Pokedex.Api.Domain.Models;
using Refit;

namespace Pokedex.Api.Domain.Services
{
    public interface ITranslationService
    {
        public Task TranslateDescriptionAsync(Pokemon pokemon);
    }

    class TranslationService : ITranslationService
    {
        readonly ITranslatorApi translatorApi;

        const string Cave = nameof(Cave);

        public TranslationService(ITranslatorApi translatorApi)
        {
            this.translatorApi = translatorApi;
        }

        public async Task TranslateDescriptionAsync(Pokemon pokemon)
        {
            ApiResponse<TranslateResponse> apiResponse;

            if (pokemon.IsLegendary || string.Equals(pokemon.Habitat, Cave, StringComparison.InvariantCultureIgnoreCase))
                apiResponse = await translatorApi.GetYodaTranslationAsync(new TranslateRequest(pokemon.Description));
            else
                apiResponse = await translatorApi.GetShakespeareTranslationAsync(new TranslateRequest(pokemon.Description));

            if (apiResponse.IsSuccessStatusCode && apiResponse.Content.Success.Total != 0)
                pokemon.Description = apiResponse.Content.Contents.Translated;

            return;
        }
    }

}
