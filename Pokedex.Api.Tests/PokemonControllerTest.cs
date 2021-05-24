using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Pokedex.Api.Controllers;
using Pokedex.Models;
using Pokedex.Services.Interfaces;

namespace Pokedex.Api.Tests
{
    [TestClass]
    public class PokemonControllerTests
    {
        //private Mock<IHttpClientFactory> MockHttpClientFactory { get; set; }
        private Mock<IPokemonService> PokemonService { get; set; }
        //private IConfiguration Configuration { get; set; }
        [TestInitialize]
        public void Init()
        {
            PokemonService = new Mock<IPokemonService>();

        }

        [TestMethod]
        public void GetPokemon_Valid_Name()
        {
            var response = new PokemonController(PokemonService.Object).Pokemon("mewtwo");
            var pokeServiceResponse = JsonConvert.DeserializeObject<Pokemon>(File.ReadAllText("TestData/PokemonServiceResponse.json"));
            PokemonService.Setup(x => x.GetPokemonByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => pokeServiceResponse));

            Assert.IsNotNull(response);
        }
        [TestMethod]
        public void GetPokemon_InValid_Name()
        {
            Pokemon result = null;
            var response = new PokemonController(PokemonService.Object).Pokemon("mewtwo123afaf");
            PokemonService.Setup(x => x.GetPokemonByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => result));

            Assert.IsNull(((ObjectResult)response.Result).Value);
        }
        [TestMethod]
        public void GetPokemon_Empty_Name()
        {
            Pokemon result = null;
            var response = new PokemonController(PokemonService.Object).Pokemon(null);
            PokemonService.Setup(x => x.GetPokemonByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => result));
            Assert.IsNull(((ObjectResult)response.Result).Value);
        }

        [TestMethod]
        public void GetTranslatedPokemon_Valid_TranslatedValue()
        {
            var pokeServiceResponse = JsonConvert.DeserializeObject<Pokemon>(File.ReadAllText("TestData/PokemonServiceResponse.json"));
            PokemonService.Setup(x => x.GetPokemonByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => pokeServiceResponse));
            PokemonService.Setup(x => x.GetTranslatedValueByTypeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.Run(() => "Sample Output"));
            var response = new PokemonController(PokemonService.Object).TranslatedPokemon("mewtwo");
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(((Pokemon) ((ObjectResult) response.Result).Value).Description, "Sample Output");
        }
        [TestMethod]
        public void GetTranslatedPokemon_InValid_TranslatedValue()
        {
            var pokeServiceResponse = JsonConvert.DeserializeObject<Pokemon>(File.ReadAllText("TestData/PokemonServiceResponse.json"));
            PokemonService.Setup(x => x.GetPokemonByNameAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => pokeServiceResponse));
            PokemonService.Setup(x => x.GetTranslatedValueByTypeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.Run(() => string.Empty));
            var response = new PokemonController(PokemonService.Object).TranslatedPokemon("mewtwo");
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(((Pokemon)((ObjectResult)response.Result).Value).Description, string.Empty);
        }
       
    }
}
