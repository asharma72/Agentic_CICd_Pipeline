using Ecommerce.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
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
        public async Task HealthEndpoint_ReturnsOk()
        {
            var response = await _client.GetAsync("/health");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOk()
        {
            var response = await _client.GetAsync("/products");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreated()
        {
            var product = new { Name = "Test Product", Price = 9.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/products", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetProductById_ReturnsOk()
        {
            var response = await _client.GetAsync("/products/1");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOk()
        {
            var product = new { Name = "Updated Test Product", Price = 10.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/products/1", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsOk()
        {
            var response = await _client.DeleteAsync("/products/1");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}