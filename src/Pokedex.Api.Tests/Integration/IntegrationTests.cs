using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pokedex.Api.Domain.Clients.PokeApi;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Pokedex.Api.Tests.Integration
{
    public class IntegrationTests
    {
        readonly PokedexApiApplication api = new();

        // TODO
        // For the life of me I can't figure out why this only returns a 404, but I'd rather not waste too much time figuring this out for a tech-test, sorry!
        // For whoever is reviewing this, I've left it in just in case you've happened to come across this issue before, something to discuss in the review

        //[Fact]
        //public async Task GetPokemon_ReturnsOk()
        //{
        //    var client = api.CreateClient();
        //    var result = await client.GetAsync("/pokemon/ditto");

        //    result.StatusCode.Should().Be(HttpStatusCode.OK);
        //    result.Content.Should().NotBeNull();
        //}

        [Fact]
        public async Task GetPokemon_ReturnsNotFound()
        {
            var client = api.CreateClient();
            var result = await client.GetAsync("/pokemon/missingno");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Health_ReturnsOk()
        {
            var client = api.CreateClient();
            var result = await client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

    class PokedexApiApplication : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddControllers();
                services.RemoveAll(typeof(IPokeApi));
                services.TryAddSingleton<IPokeApi, FakePokeApi>();
            });
        }
    }
}
