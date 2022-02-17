using Pokedex.Api.Domain.Clients.PokeApi;
using Pokedex.Api.Domain.Models;
using Refit;
using System.Net;

namespace Pokedex.Api.Domain.Services
{
    public class PokemonService : IPokemonService
    {
        readonly IPokeApi pokeApi;

        public PokemonService(IPokeApi pokeApi)
        {
            this.pokeApi = pokeApi;
        }

        public async Task<Result<Pokemon>> GetPokemonAsync(string name, bool translate = false, CancellationToken cancellation = default)
        {
            var apiResponse = await pokeApi.GetPokemonSpeciesAsync(name, cancellation);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return HandleApiError(apiResponse); 
            }
           
            // TODO Add mapper
            var species = apiResponse.Content;
            var pokemon = new Pokemon(species.Name, species.FlavorTextEntries.First(x => x.Language.Name == "en").FlavorText, species.Habitat?.Name, species.IsLegendary);

            // TODO Add translation rulesets

            return Result<Pokemon>.Success(pokemon);
        }

        private Result<Pokemon> HandleApiError(ApiResponse<PokemonSpecies> apiResponse)
        {
            if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                return Result<Pokemon>.NotFound();

            return Result<Pokemon>.Failure(apiResponse.Error);
        }
    }
}
