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
        public async Task GetHealth_ReturnsOkResponse()
        {
            var response = await _client.GetAsync("/health");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task PostProduct_CreateProduct_ReturnsCreatedResponse()
        {
            var product = new { name = "Test Product", price = 10.99m };
            var json = System.Text.Json.JsonSerializer.Serialize(product);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/products", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResponse()
        {
            var response = await _client.GetAsync("/products");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task PutProduct_UpdateProduct_ReturnsOkResponse()
        {
            var product = new { id = 1, name = "Updated Test Product", price = 10.99m };
            var json = System.Text.Json.JsonSerializer.Serialize(product);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/products/1", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteProduct_DeleteProduct_ReturnsNoContentResponse()
        {
            var response = await _client.DeleteAsync("/products/1");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}