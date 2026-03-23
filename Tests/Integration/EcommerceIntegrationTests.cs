using Ecommerce.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.API.Tests.Integration
{
    public class EcommerceApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public EcommerceApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Health_Endpoint_Returns_OK()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/health");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Post_Product_Endpoint_Returns_Created()
        {
            // Arrange
            var product = new { Name = "Test Product", Price = 10.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "/products") { Content = content };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Get_Products_Endpoint_Returns_OK()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/products");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Put_Product_Endpoint_Returns_OK()
        {
            // Arrange
            var productId = 1;
            var product = new { Name = "Updated Product", Price = 10.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"/products/{productId}") { Content = content };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Delete_Product_Endpoint_Returns_OK()
        {
            // Arrange
            var productId = 1;
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/products/{productId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}