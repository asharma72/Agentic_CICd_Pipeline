using Ecommerce.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.API.Tests.Integration
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Health_ReturnsOk()
        {
            var response = await _client.GetAsync("/health");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_Product_CreatesProduct()
        {
            var product = new { Name = "Test Product", Price = 10.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/products", content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Products_ReturnsProducts()
        {
            var response = await _client.GetAsync("/products");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Product_ReturnsProduct()
        {
            var response = await _client.GetAsync("/products/1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Put_Product_UpdatesProduct()
        {
            var product = new { Name = "Updated Product", Price = 12.99m };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(product), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/products/1", content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_Product_RemovesProduct()
        {
            var response = await _client.DeleteAsync("/products/1");
            response.EnsureSuccessStatusCode();
        }
    }
}