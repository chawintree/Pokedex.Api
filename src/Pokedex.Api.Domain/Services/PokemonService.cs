using Pokedex.Api.Domain.Clients.PokeApi;
using Pokedex.Api.Domain.Models;
using Refit;
using System.Net;

namespace Pokedex.Api.Domain.Services
{
    public interface IPokemonService
    {
        public Task<Result<Pokemon>> GetPokemonAsync(string name, bool translate = false, CancellationToken cancellation = default);
    }

    class PokemonService : IPokemonService
    {
        readonly IPokeApi pokeApi;
        readonly ITranslationService translationService;

        public PokemonService(IPokeApi pokeApi, ITranslationService translationService)
        {
            this.pokeApi = pokeApi;
            this.translationService = translationService;
        }

        public async Task<Result<Pokemon>> GetPokemonAsync(string name, bool translate = false, CancellationToken cancellation = default)
        {
            var apiResponse = await pokeApi.GetPokemonSpeciesAsync(name, cancellation);

            if (!apiResponse.IsSuccessStatusCode)
                return HandleApiError(apiResponse); 
           
            var pokemon = GetPokemon(apiResponse.Content);

            if (translate)
                await translationService.TranslateDescriptionAsync(pokemon, cancellation);

            return Result<Pokemon>.Success(pokemon);
        }

        Pokemon GetPokemon(PokemonSpecies species) => new Pokemon(
                species.Name, 
                species.FlavorTextEntries.First(x => x.Language.Name == "en").FlavorText, 
                species.Habitat?.Name, 
                species.IsLegendary);

        Result<Pokemon> HandleApiError(ApiResponse<PokemonSpecies> apiResponse)
        {
            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                return Result<Pokemon>.NotFound();

            return Result<Pokemon>.Failure(apiResponse.Error);
        }
    }
}
