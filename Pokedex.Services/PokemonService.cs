using Pokedex.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pokedex.Models;
using Microsoft.Extensions.Configuration;

namespace Pokedex.Services
{
    public class PokemonService : IPokemonService
    {
        private IHttpClientFactory HttpClientFactory { get; }
        private string PokeApiUrl { get; }
        private string YodaApiUrl { get; }
        private string ShakespeareApiUrl { get; }
        public PokemonService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            HttpClientFactory = httpClientFactory;
            PokeApiUrl = configuration.GetSection("pokeapi")?.Value;
            YodaApiUrl = configuration.GetSection("yodaapi")?.Value;
            ShakespeareApiUrl = configuration.GetSection("shakespeareapi")?.Value;
        }
        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            try
            {
                var httpClient = HttpClientFactory.CreateClient("PokemonClient");
                var apiResponse = await httpClient.GetAsync($"{PokeApiUrl}{name}");
                if (apiResponse.IsSuccessStatusCode)
                {
                    string apiResult= apiResponse.Content.ReadAsStringAsync().Result;

                    apiResult = Unescape(apiResult);
                    var pokeApiResult = JsonConvert.DeserializeObject<PokeApiResult>(apiResult) ??
                                        throw new ArgumentNullException(
                                            "JsonConvert.DeserializeObject<PokeApiResult>(apiResponse)");

                    var pokemon = new Pokemon()
                    {
                        Habitat = pokeApiResult.Habitat.Name,
                        IsLengedary = pokeApiResult.IsLegendary,
                        Name = name
                    };
                    foreach (var item in pokeApiResult.FlavorTextEntries.Where(item => item.Language.Name == "en"))
                    {
                        pokemon.Description = Unescape(item.FlavorText);
                        break;
                    }

                    return pokemon;
                }
                else
                {
                    Console.Write($"For Name {name} recieved Response code :{apiResponse.StatusCode}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw e;
            }

        }

        public async Task<string> GetTranslatedValueByTypeAsync(string type, string description)
        {
            try
            {
                var httpClient = HttpClientFactory.CreateClient();
                var url = type == "yoda" ? $"{YodaApiUrl}?text={description}" : $"{ShakespeareApiUrl}?text={description}";
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string apiResult = response.Content.ReadAsStringAsync().Result;
                    var translatedContent = JsonConvert.DeserializeObject<TranslatedContent>(apiResult);
                    return translatedContent is {Content: { }}
                        ? translatedContent.Content.TranslatedValue
                        : string.Empty;
                }
                else
                {
                    Console.Write($"For description {description} recieved response code {response.StatusCode}.");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        private static string Unescape(string input)
        {
            input = input.Replace("\n", " ").Replace("\f", " ").Replace("\t", " ").Replace("\v", " ");
            return input;
        }
    }
}
