using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Domain.Models;
using Pokedex.Api.Domain.Services;

namespace Pokedex.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        readonly IPokemonService pokemonService;
        public PokemonController(IPokemonService pokemonService)
        {
            this.pokemonService = pokemonService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            var result = await pokemonService.GetPokemonAsync(name);

            return ToActionResult(result);
        }

        [HttpGet("translated/{name}")]
        public async Task<IActionResult> GetTranslated([FromRoute] string name)
        {
            var result = await pokemonService.GetPokemonAsync(name, true);

            return ToActionResult(result);
        }

        IActionResult ToActionResult<T>(Result<T> result)
        {
            return result.Status switch
            {
                Status.Success => Ok(result),
                Status.NotFound => NotFound(),
                Status.Error => StatusCode(500),
                _ => StatusCode(500)
            };
        }
    }
}