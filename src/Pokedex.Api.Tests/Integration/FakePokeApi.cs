using Pokedex.Api.Domain.Clients.PokeApi;
using Refit;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Api.Tests.Integration
{
    class FakePokeApi : IPokeApi
    {
        Dictionary<string, PokemonSpecies> pokemon = new Dictionary<string, PokemonSpecies>()
        {
            { "ditto", new PokemonSpecies(132, "Ditto", false, new PokemonHabitat("urban"),
                new List<PokemonFlavorText> { new PokemonFlavorText("Capable of copying an enemy's genetic code to instantly transform itself into a duplicate of the enemy", new PokemonLanguage("en"))})},
            { "mewtwo", new PokemonSpecies(150, "Mewtwo", false, new PokemonHabitat("rare"),
                new List<PokemonFlavorText> { new PokemonFlavorText("It was created by a scientist after years of horrific gene splicing and DNA engineering experiments", new PokemonLanguage("en"))})},
            { "zubat", new PokemonSpecies(41, "Zubat", false, new PokemonHabitat("cave"),
                new List<PokemonFlavorText> { new PokemonFlavorText("Forms colonies in perpetually dark places. Uses ultrasonic waves to identify and approach targets", new PokemonLanguage("en"))})}
        };

        public Task<ApiResponse<PokemonSpecies>> GetPokemonSpeciesAsync(string name, CancellationToken cancellation = default)
        {
            this.pokemon.TryGetValue(name.ToLower(), out var pokemon);

            var responseCode = pokemon != null ? HttpStatusCode.OK : HttpStatusCode.NotFound;

            return Task.FromResult(new ApiResponse<PokemonSpecies>(new HttpResponseMessage(responseCode), pokemon!));
        }
    }
}
