using Newtonsoft.Json;
namespace Pokedex.Models
{
    public class FlavorTextEntry
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }
        [JsonProperty("language")]
        public NameUrl Language { get; set; }
      
    }
}
