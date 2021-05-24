using Newtonsoft.Json;
namespace Pokedex.Models
{
    public class NameUrl
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
