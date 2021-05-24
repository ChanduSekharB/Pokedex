using Newtonsoft.Json;
namespace Pokedex.Models
{
    public class TranslatedContent
    {
        [JsonProperty("contents")]
        public Content Content { get; set; }
    }
}
