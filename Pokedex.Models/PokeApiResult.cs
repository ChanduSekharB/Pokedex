using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Pokedex.Models
{

    public class PokeApiResult
    {
        [JsonProperty("habitat")]
        public NameUrl Habitat { get; set; }
        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }
        [JsonProperty("flavor_text_entries")]
        public List<FlavorTextEntry> FlavorTextEntries { get; set; }
    }
}
