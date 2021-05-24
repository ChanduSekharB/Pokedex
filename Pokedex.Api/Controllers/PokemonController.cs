using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Services.Interfaces;

namespace Pokedex.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        public IPokemonService PokemonService { get; set; }
        public PokemonController(IPokemonService pokemonService)
        {
            PokemonService = pokemonService;
        }

        [HttpGet]
        [Route("pokemon/{name}")]
        public async Task<IActionResult> Pokemon(string name)
        {
            try
            {
                var response = await PokemonService.GetPokemonByNameAsync(name);
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }

        }

        [HttpGet]
        [Route("pokemon/translated/{name}")]
        public async Task<IActionResult> TranslatedPokemon(string name)
        {
            try
            {
                var response = await PokemonService.GetPokemonByNameAsync(name);
                if (response == null) throw new ArgumentNullException(nameof(response));
                string transactionType = "shakespeare";
                if ((response.Habitat != null && response.Habitat.ToLower() == "cave") || response.IsLengedary)
                {
                    transactionType = "yoda";
                }
                var translatedValue = await PokemonService.GetTranslatedValueByTypeAsync(transactionType, response.Description);
                if (translatedValue != null)
                    response.Description = translatedValue;
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return BadRequest(ex);
            }
        }

    }
}
