using Pokedex.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Services.Interfaces
{
    public interface IPokemonService
    {
        Task<Pokemon> GetPokemonByNameAsync(string name);
        Task<string> GetTranslatedValueByTypeAsync(string type, string description);
    }
}
