using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Api.Domain.Clients.PokeApi
{
    public interface IPokeApi
    {
        [Get("/pokemon-species/{name}")]
        Task<ApiResponse<PokemonSpecies>> GetPokemonSpeciesAsync(string name, CancellationToken cancellation = default);
    }

    public record PokemonSpecies(int Id, string Name, bool IsLegendary, PokemonHabitat? Habitat, IEnumerable<PokemonFlavorText> FlavorTextEntries);

    public record PokemonHabitat(string Name);

    public record PokemonFlavorText(string FlavorText, PokemonLanguage Language);

    public record PokemonLanguage(string Name);
}
