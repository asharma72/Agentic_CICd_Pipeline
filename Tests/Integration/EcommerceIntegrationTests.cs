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
        public async Task GetHealth_ReturnsOk()
        {
            var response = await _client.GetAsync("/health");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task PostProduct_CreateProduct_ReturnsCreated()
        {
            var product = new { name = "Test Product", price = 10.99m };
            var json = System.Text.Json.JsonSerializer.Serialize(product);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/products", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetProducts_GetAllProducts_ReturnsOk()
        {
            var response = await _client.GetAsync("/products");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetProduct_GetProductById_ReturnsOk()
        {
            var productId = 1;
            var response = await _client.GetAsync($"/products/{productId}");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task PutProduct_UpdateProduct_ReturnsOk()
        {
            var productId = 1;
            var product = new { name = "Updated Product", price = 11.99m };
            var json = System.Text.Json.JsonSerializer.Serialize(product);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/products/{productId}", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteProduct_DeleteProduct_ReturnsNoContent()
        {
            var productId = 1;
            var response = await _client.DeleteAsync($"/products/{productId}");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}