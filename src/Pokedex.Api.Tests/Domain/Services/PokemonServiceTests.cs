using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Pokedex.Api.Domain.Clients.PokeApi;
using Pokedex.Api.Domain.Models;
using Pokedex.Api.Domain.Services;
using Refit;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.Tests.Domain.Services
{
    public class PokemonServiceTests
    {
        readonly Fixture fixture = new();
        readonly PokemonService service;
        readonly IPokeApi pokeApi = Substitute.For<IPokeApi>();
        readonly ITranslationService translationService = Substitute.For<ITranslationService>();

        const string pokemonName = "Testomon";

        public PokemonServiceTests()
        {
            service = new PokemonService(pokeApi, translationService);
        }

        [Fact]
        public async Task GetPokemonAsync_ReturnsPokemon()
        {
            var response = fixture.Build<PokemonSpecies>()
                .With(x => x.FlavorTextEntries, new List<PokemonFlavorText> { ValidFlavorText() })
                .Create();

            pokeApi.GetPokemonSpeciesAsync(pokemonName)
                .Returns(new ApiResponse<PokemonSpecies>(new HttpResponseMessage(HttpStatusCode.OK), response));

            var result = await service.GetPokemonAsync(pokemonName);

            result.Status.Should().Be(Status.Success);
            result.Value.Should().NotBeNull();

            var pokemon = result.Value!;
            pokemon.Name.Should().Be(response.Name);
            pokemon.Habitat.Should().Be(response.Habitat?.Name);
            pokemon.Description.Should().BeOneOf(response.FlavorTextEntries
                    .Where(x => x.Language.Name == "en").Select(x => x.FlavorText));
            pokemon.IsLegendary.Should().Be(response.IsLegendary);
        }

        [Fact]
        public async Task GetPokemonAsync_NotFoundResponse_ReturnsNotFoundResult()
        {
            pokeApi.GetPokemonSpeciesAsync(pokemonName)
                .Returns(new ApiResponse<PokemonSpecies>(new HttpResponseMessage(HttpStatusCode.NotFound), null!));

            var result = await service.GetPokemonAsync(pokemonName);

            result.Status.Should().Be(Status.NotFound);
        }

        [Fact]
        public async Task GetPokemonAsync_ApiError_ReturnsFailureResult()
        {
            var exception = await ApiException.Create(new HttpRequestMessage(), null!, new HttpResponseMessage());

            pokeApi.GetPokemonSpeciesAsync(pokemonName)
                .Returns(new ApiResponse<PokemonSpecies>(new HttpResponseMessage(HttpStatusCode.InternalServerError), null!, exception));

            var result = await service.GetPokemonAsync(pokemonName);

            result.Status.Should().Be(Status.Error);
            result.Error.Should().Be(exception);
        }

        [Fact]
        public async Task GetPokemonAsync_TranslateRequired_CallsTranslationService()
        {
            var response = fixture.Build<PokemonSpecies>()
                .With(x => x.FlavorTextEntries, new List<PokemonFlavorText> { ValidFlavorText() })
                .Create();

            pokeApi.GetPokemonSpeciesAsync(pokemonName)
                .Returns(new ApiResponse<PokemonSpecies>(new HttpResponseMessage(HttpStatusCode.OK), response));

            var result = await service.GetPokemonAsync(pokemonName, true);

            result.Status.Should().Be(Status.Success);
            await translationService.Received().TranslateDescriptionAsync(Arg.Any<Pokemon>());
        }

        static PokemonFlavorText ValidFlavorText() => new ("Hi!", new PokemonLanguage("en"));
    }
}
