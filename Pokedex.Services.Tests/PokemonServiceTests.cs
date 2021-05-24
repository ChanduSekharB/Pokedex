using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NuGet.Frameworks;
using System.Threading.Tasks;
using Moq.Protected;

namespace Pokedex.Services.Tests
{
    [TestClass]
    public class PokemonServiceTests
    {
        private Mock<IHttpClientFactory> MockHttpClientFactory { get; set; }
        private string PokeApiUrl { get; set; }
        private string YodaApiUrl { get; set; }
        private string ShakespeareApiUrl { get; set; }
        private PokemonService PokemonService { get; set; }
        private IConfiguration Configuration { get; set; }
        [TestInitialize]
        public void Init()
        {
            MockHttpClientFactory = new Mock<IHttpClientFactory>();
            Configuration = new Mock<IConfiguration>().Object;
            var inMemorySettings = new Dictionary<string, string> {
                    {"pokeapi", "https://pokeapi.co/api/v2/pokemon-species/"},
                    {"yodaapi", "https://api.funtranslations.com/translate/yoda.json"},
                    {"shakespeareapi", "https://api.funtranslations.com/translate/shakespeare.json"}
                };

            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [TestMethod]
        public void GetPokemonByNameAsync_ValidData()
        {
            var expected = File.ReadAllText("TestData/PokeResponse.json");
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expected)
                });

            var httpClient = new Mock<HttpClient>(mockMessageHandler.Object);

            MockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);
            PokemonService = new PokemonService(MockHttpClientFactory.Object, Configuration);
            var response = PokemonService.GetPokemonByNameAsync("mewtwo").Result;
            Assert.IsNotNull(response);

        }
        [TestMethod]
        public void GetPokemonByNameAsync_InValidData()
        {
            var expected = "Not Found";
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expected)
                });

            var httpClient = new Mock<HttpClient>(mockMessageHandler.Object);

            MockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);
            PokemonService = new PokemonService(MockHttpClientFactory.Object, Configuration);
            var response = PokemonService.GetPokemonByNameAsync("mewtwo").Result;
            Assert.IsNotNull(response);

        }
        
    }
}
