using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Domain.Models;
using Pokedex.Api.Domain.Services;
using System.Net;

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
        public async Task<IActionResult> Get([FromRoute] string name, CancellationToken cancellation = default)
        {
            var result = await pokemonService.GetPokemonAsync(name, false, cancellation);

            return ToActionResult(result);
        }

        [HttpGet("translated/{name}")]
        public async Task<IActionResult> GetTranslated([FromRoute] string name, CancellationToken cancellation = default)
        {
            var result = await pokemonService.GetPokemonAsync(name, true, cancellation);

            return ToActionResult(result);
        }

        IActionResult ToActionResult<T>(Result<T> result)
        {
            return result.Status switch
            {
                Status.Success => Ok(result.Value),
                Status.NotFound => NotFound(),
                Status.Error => StatusCode((int)HttpStatusCode.InternalServerError),
                _ => StatusCode((int)HttpStatusCode.InternalServerError)
            };
        }
    }
}