using Pokedex.Api.Domain.Models;

namespace Pokedex.Api.Domain.Services
{
    public interface IPokemonService
    {
        public Task<Result<Pokemon>> GetPokemonAsync(string name, bool translate = false, CancellationToken cancellation = default);
    }
}
