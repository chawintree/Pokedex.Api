using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Domain.Models;
using Pokedex.Api.Domain.Services;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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

        /// <summary>
        /// Returns the information about a pokemon
        /// </summary>
        /// <param name="name">The pokemon's name</param>
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPokemon([FromRoute] string name, CancellationToken cancellation = default)
        {
            var result = await pokemonService.GetPokemonAsync(name, false, cancellation);

            return ToActionResult(result);
        }

        /// <summary>
        /// Returns the information about a pokemon with a fancy description
        /// </summary>
        /// <param name="name">The pokemon's name</param>
        /// <returns></returns>
        [HttpGet("translated/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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