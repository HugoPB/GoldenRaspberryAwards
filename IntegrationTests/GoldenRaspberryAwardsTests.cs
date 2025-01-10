using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace IntegrationTests
{
    public class GoldenRaspberryAwardsTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public GoldenRaspberryAwardsTests()
        {
            _server = new TestServer(
            new WebHostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseStartup<StartupTests>());
            _client = _server.CreateClient();
        }


        [Fact]
        public async void GetAllAwards_ShouldReturnOK()
        {
            var response = await _client.GetAsync($"GoldenRaspberry/GetAllAwards");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void GetMinMaxWinnersInterval_ShouldReturnOK()
        {
            var response = await _client.GetAsync($"GoldenRaspberry/GetWinnersInterval");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}