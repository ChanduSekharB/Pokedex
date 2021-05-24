using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    public class UnitTest1
    {
        private Mock<IPokemonService> PokemonService { get; set; }
        //private IConfiguration Configuration { get; set; }
        [TestInitialize]
        public void Init()
        {
            PokemonService = new Mock<IPokemonService>();

        }

        [TestMethod]
        public void GetPokemonByNamePassed()
        {
            var controller = new PokemonController(PokemonService.Object);
            var pokeServiceResponse = JsonConvert.DeserializeObject<Pokemon>(File.ReadAllText("TestData/PokemonServiceResponse.json"));
            PokemonService.Setup(x => x.GetPokemonByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(pokeServiceResponse);
            var response = controller.Pokemon("mewtwo").Result;
            Assert.IsNotNull(response);
        }
    }
}
