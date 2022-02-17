using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Pokedex.Api.Domain.Clients.PokeApi;
using Pokedex.Api.Domain.Models;
using Pokedex.Api.Domain.Services;
using Refit;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.Tests.Domain.Services
{
    public class TranslationServiceTests
    {
        readonly Fixture fixture = new();
        readonly TranslationService service;
        readonly ITranslatorApi translatorApi = Substitute.For<ITranslatorApi>();

        public TranslationServiceTests()
        {
            service = new TranslationService(translatorApi);
        }

        [Fact]
        public async Task TranslateDescriptionAsync_IsLegendary_GetsYodaTranslation()
        {
            var pokemon = fixture.Build<Pokemon>()
                .With(x => x.IsLegendary, true)
                .Create();
            var response = fixture.Build<TranslateResponse>()
                .With(x => x.Success, new TranslateSuccess(1))
                .Create();

            translatorApi.GetYodaTranslationAsync(Arg.Is<TranslateRequest>(x => x.Text == pokemon.Description))
                .Returns(new ApiResponse<TranslateResponse>(new HttpResponseMessage(HttpStatusCode.OK), response));

            await service.TranslateDescriptionAsync(pokemon);

            pokemon.Description.Should().Be(response.Contents.Translated);
        }

        [Theory]
        [InlineData("Cave")]
        [InlineData("CAVE")]
        [InlineData("cave")]
        public async Task TranslateDescriptionAsync_IsCaveHabitat_GetsYodaTranslation(string habitat)
        {
            var pokemon = fixture.Build<Pokemon>()
                .With(x => x.Habitat, habitat)
                .Create();
            var response = fixture.Build<TranslateResponse>()
                .With(x => x.Success, new TranslateSuccess(1))
                .Create();

            translatorApi.GetYodaTranslationAsync(Arg.Is<TranslateRequest>(x => x.Text == pokemon.Description))
                .Returns(new ApiResponse<TranslateResponse>(new HttpResponseMessage(HttpStatusCode.OK), response));

            await service.TranslateDescriptionAsync(pokemon);

            pokemon.Description.Should().Be(response.Contents.Translated);
        }

        [Fact]
        public async Task TranslateDescriptionAsync_NoRulesMet_GetsShakespeareTranslation()
        {
            var pokemon = fixture.Build<Pokemon>()
                .With(x => x.IsLegendary, false)
                .With(x => x.Habitat, string.Empty)
                .Create();
            var response = fixture.Build<TranslateResponse>()
                .With(x => x.Success, new TranslateSuccess(1))
                .Create();

            translatorApi.GetShakespeareTranslationAsync(Arg.Is<TranslateRequest>(x => x.Text == pokemon.Description))
                .Returns(new ApiResponse<TranslateResponse>(new HttpResponseMessage(HttpStatusCode.OK), response));

            await service.TranslateDescriptionAsync(pokemon);

            pokemon.Description.Should().Be(response.Contents.Translated);
        }

        [Fact]
        public async Task TranslateDescriptionAsync_ApiError_DoesNotTranslate()
        {
            var description = Guid.NewGuid().ToString();

            var pokemon = fixture.Build<Pokemon>()
                .With(x => x.IsLegendary, false)
                .With(x => x.Habitat, string.Empty)
                .With(x => x.Description, description)
                .Create();
            var response = fixture.Build<TranslateResponse>()
                .Create();

            translatorApi.GetShakespeareTranslationAsync(Arg.Is<TranslateRequest>(x => x.Text == pokemon.Description))
                .Returns(new ApiResponse<TranslateResponse>(new HttpResponseMessage(HttpStatusCode.InternalServerError), null!));

            await service.TranslateDescriptionAsync(pokemon);

            pokemon.Description.Should().Be(description);
        }

        [Fact]
        public async Task TranslateDescriptionAsync_UnsuccessfulResponse_DoesNotTranslate()
        {
            var description = Guid.NewGuid().ToString();

            var pokemon = fixture.Build<Pokemon>()
                .With(x => x.IsLegendary, false)
                .With(x => x.Habitat, string.Empty)
                .With(x => x.Description, description)
                .Create();
            var response = fixture.Build<TranslateResponse>()
                .With(x => x.Success, new TranslateSuccess(0))
                .Create();

            translatorApi.GetShakespeareTranslationAsync(Arg.Is<TranslateRequest>(x => x.Text == pokemon.Description))
                .Returns(new ApiResponse<TranslateResponse>(new HttpResponseMessage(HttpStatusCode.OK), response));

            await service.TranslateDescriptionAsync(pokemon);

            pokemon.Description.Should().Be(description);
        }
    }
}
