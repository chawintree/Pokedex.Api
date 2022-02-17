namespace Pokedex.Api.Domain.Models
{
    public record Pokemon(string Name, string Description, string? Habitat, bool IsLegendary);
}
