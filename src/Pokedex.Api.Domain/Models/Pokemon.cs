namespace Pokedex.Api.Domain.Models
{
    public class Pokemon
    {
        public Pokemon(string name, string description, string? habitat, bool isLegendary)
        {
            Name = name;
            Description = description;
            Habitat = habitat;
            IsLegendary = isLegendary;
        }

        public string Name { get; init; }
        public string Description { get; set; }
        public string? Habitat { get; init; }
        public bool IsLegendary { get; init; }
    }
}
