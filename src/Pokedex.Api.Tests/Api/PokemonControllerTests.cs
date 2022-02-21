using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Pokedex.Api.Controllers;
using Pokedex.Api.Domain.Models;
using Pokedex.Api.Domain.Services;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.Tests.Api
{
    public class PokemonControllerTests
    {
        readonly Fixture fixture = new();
        readonly PokemonController controller;
        readonly IPokemonService pokemonService = Substitute.For<IPokemonService>();

        const string pokemonName = "Testomon";

        public PokemonControllerTests()
        {
            controller = new PokemonController(pokemonService);
        }

        [Fact]
        public async Task GetPokemon_ReturnsSuccess()
        {
            var pokemon = fixture.Create<Pokemon>();

            pokemonService.GetPokemonAsync(pokemonName)
                .Returns(Result<Pokemon>.Success(pokemon));

            var result = await controller.GetPokemon(pokemonName);

            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should().Be(pokemon);
        }

        [Fact]
        public async Task GetPokemon_PokemonNotFound_ReturnsNotFound()
        {
            pokemonService.GetPokemonAsync(pokemonName)
                .Returns(Result<Pokemon>.NotFound());

            var result = await controller.GetPokemon(pokemonName);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetPokemon_ServiceErrors_ReturnsServerError()
        {
            pokemonService.GetPokemonAsync(pokemonName)
                .Returns(Result<Pokemon>.Failure());

            var result = await controller.GetPokemon(pokemonName);

            result.Should().BeOfType<StatusCodeResult>();
            ((StatusCodeResult)result).StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task GetTranslated_ReturnsSuccess()
        {
            var pokemon = fixture.Create<Pokemon>();

            pokemonService.GetPokemonAsync(pokemonName, true)
                .Returns(Result<Pokemon>.Success(pokemon));

            var result = await controller.GetTranslated(pokemonName);

            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should().Be(pokemon);
        }

        [Fact]
        public async Task GetTranslated_PokemonNotFound_ReturnsNotFound()
        {
            pokemonService.GetPokemonAsync(pokemonName, true)
                .Returns(Result<Pokemon>.NotFound());

            var result = await controller.GetTranslated(pokemonName);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetTranslated_ServiceErrors_ReturnsServerError()
        {
            pokemonService.GetPokemonAsync(pokemonName, true)
                .Returns(Result<Pokemon>.Failure());

            var result = await controller.GetTranslated(pokemonName);

            result.Should().BeOfType<StatusCodeResult>();
            ((StatusCodeResult)result).StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
