using System;

namespace Pokedex.Models
{
    
    /// <summary>
    /// Properties presented in this class will be exposed through Pokedex Api
    /// </summary>
    public class Pokemon
    {
        #region Properties
        public string Name { get; set; }
        public string Habitat { get; set; }
        public bool IsLengedary { get; set; }
        public string Description { get; set; }
        #endregion
    }
}
