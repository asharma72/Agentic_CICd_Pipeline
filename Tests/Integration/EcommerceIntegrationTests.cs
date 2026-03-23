using Ecommerce.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.API.Tests.Integration
{
    public class EcommerceApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EcommerceApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task HealthEndpoint_ReturnsOk()
        {
            var response = await _client.GetAsync("/health");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreated()
        {
            var product = new { name = "Test Product", price = 10.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/products", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task ReadProduct_ReturnsOk()
        {
            var response = await _client.GetAsync("/products/1");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOk()
        {
            var product = new { name = "Updated Test Product", price = 12.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/products/1", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            var response = await _client.DeleteAsync("/products/1");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}