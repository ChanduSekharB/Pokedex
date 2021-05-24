using Newtonsoft.Json;
namespace Pokedex.Models
{
    public class Content
    {
        [JsonProperty("translated")]
        public string TranslatedValue { get; set; }
       
    }
}
